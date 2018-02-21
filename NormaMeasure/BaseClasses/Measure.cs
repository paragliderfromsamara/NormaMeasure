﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NormaMeasure.BaseClasses
{
    public enum MEASURE_STATUS : byte
    {
        POLAR = 0,
        DISCHARGE = 1,
        IS_GOING = 2,
        FINISHED = 3,
        NOT_STARTED = 4,
        STOPED = 5,
        INIT = 6,
        WILL_STOP = 7
    }


    public enum MEASURE_TYPE : byte
    {
        HAND = 0,
        AUTO = 1,
        VERIFICATION = 2,
        CALIBRATION = 3
    }
    public class MeasureBase
    {
        public MEASURE_TYPE Type;
        protected int number = 0;
        protected int cycleNumber;
        protected int statCycleNumber;
        public string Name = String.Empty;
        public List<MeasureResultCollection> ResultCollectionsList = new List<MeasureResultCollection>();
        protected int curResultListId = -1;
        public MeasureResultCollection CurrentCollection
        {
            get
            {
                if (curResultListId > -1 && ResultCollectionsList.Count > curResultListId) return ResultCollectionsList[curResultListId];
                else return null;
            }
        }
        /// <summary>
        /// Номер испытания в данном цикле
        /// </summary>
        public int StatCycleNumber
        {
            get { return statCycleNumber; }
            set { this.statCycleNumber = value; }
        }
        /// <summary>
        /// Номер испытания в текущем списке результатов. Инкрементируется для каждого последующего нажатия кнопки запуска измерения
        /// Сбрасывается когда добавляется новый лист результатов
        /// </summary>
        public int Number
        {
            get { return number; }
            set
            {
                this.number = value;
                CycleNumber = 1;
            }
        }
        public int CycleNumber
        {
            get { return cycleNumber; }
            set
            {
                cycleNumber = value;
                statCycleNumber = 1;
            }
        }




        protected MEASURE_STATUS measureStatus;
        public MEASURE_STATUS MeasureStatus
        {
            get { return measureStatus; }
            set
            {
                MEASURE_STATUS was = measureStatus;
                if (was == MEASURE_STATUS.STOPED && value == MEASURE_STATUS.FINISHED) return;
                this.measureStatus = value;
                if (was != value) updatedMeasureStatusHandle();
            }
        }
        public bool IsStarted
        {
            get
            {
                return (measureStatus != MEASURE_STATUS.STOPED && measureStatus != MEASURE_STATUS.NOT_STARTED && measureStatus != MEASURE_STATUS.FINISHED);
            }
        }
        protected System.Timers.Timer MeasureTimer;
        protected Thread measureThread = null;
        protected int measSeconds;

        /// <summary>
        /// Ручные испытания
        /// </summary>
        protected virtual void handMeasureThreadFunction() { }
        /// <summary>
        /// Автоматические
        /// </summary>
        protected virtual void autoMeasureThreadFunction() { }



        /// <summary>
        /// Калибровка
        /// </summary>
        protected virtual void calibrationMeasureThreadFunction() { }

        /// <summary>
        /// Поверочные испытания
        /// </summary>
        protected virtual void verificationMeasureThreadFunction() { }
        protected virtual void measureTimerHandler() { }

        protected virtual void initByMeasureType(){}

        protected virtual void updatedMeasureStatusHandle()
        {
        }


        public MeasureBase()
        {
            this.Type = MEASURE_TYPE.AUTO;
            this.MeasureStatus = MEASURE_STATUS.NOT_STARTED;
        }

        public MeasureBase(MEASURE_TYPE type)
        {
            this.Type = type;
            this.MeasureStatus = MEASURE_STATUS.NOT_STARTED;
            initByMeasureType();
            initTimer();
        }

        protected void resetTime()
        {
            measSeconds = 0;
        }

        private void initTimer()
        {
            MeasureTimer = new System.Timers.Timer(1000);
            MeasureTimer.AutoReset = true;
            MeasureTimer.Elapsed += MeasureTimer_Elapsed;
            resetTime();
        }

        private void MeasureTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs arg)
        {
            measSeconds++;
            measureTimerHandler();
            if (MeasureStatus == MEASURE_STATUS.FINISHED || MeasureStatus == MEASURE_STATUS.STOPED) MeasureTimer.Stop();
        }



        public bool Start()
        {
            if (!this.IsStarted)
            {
                this.Number++;
                if (!this.setResultList()) return false;
                switch (Type)
                {
                    case MEASURE_TYPE.AUTO:
                        measureThread = new Thread(autoMeasureThreadFunction);
                        break;
                    case MEASURE_TYPE.HAND:
                        measureThread = new Thread(handMeasureThreadFunction);
                        break;
                    case MEASURE_TYPE.VERIFICATION:
                        measureThread = new Thread(verificationMeasureThreadFunction);
                        break;
                    case MEASURE_TYPE.CALIBRATION:
                        measureThread = new Thread(calibrationMeasureThreadFunction);
                        break;
                }
                if (measureThread == null)
                {
                    this.MeasureStatus = MEASURE_STATUS.NOT_STARTED;
                }
                else
                {
                    this.MeasureStatus = MEASURE_STATUS.INIT;
                    measureThread.Start();
                    MeasureTimer.Start();
                }
                
            }
            return IsStarted;
        }


        protected virtual bool stop()
        {
            if (measureThread != null)
            {
                measureThread.Abort();
                measureThread = null;
                MeasureTimer.Stop();
                return true;
            }
            else return false;
        }

        public virtual void StopWithStatus(MEASURE_STATUS status)
        {
            if (Properties.Settings.Default.IsTestApp) if (this.stop()) this.MeasureStatus = status;
                else this.MeasureStatus = status;

        }

        public static void measureError(string m)
        {
            MessageBox.Show("Ошибка испытания", m, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected int getMeasureIndexByName(string name)
        {
            for (int i = 0; i < ResultCollectionsList.Count; i++) if (name == ResultCollectionsList[i].Name) return i;
            return -1;
        }

        protected virtual string getName()
        {
            return Name;
        }

        protected bool setResultList()
        {
            string mName = getName();
            curResultListId = getMeasureIndexByName(mName);
            if (curResultListId == -1)
            {
                this.ResultCollectionsList.Add(new MeasureResultCollection(mName));
                this.curResultListId = this.ResultCollectionsList.Count() - 1;
            }else
            {
                switch (this.Type)
                {
                    case MEASURE_TYPE.HAND:
                        DialogResult r = MessageBox.Show(String.Format("Список результатов с идентификатором \"{0}\" уже существует, при запуске нового испытания сотрутся предыдущие результаты. Вы согласны?", mName), "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (r == DialogResult.Yes) this.ResultCollectionsList[curResultListId].Clear();
                        else if (r == DialogResult.No) return false;
                        break;
                    case MEASURE_TYPE.CALIBRATION:
                        this.ResultCollectionsList[curResultListId].Clear();
                        break;
                }
            }
            return true;
        }

        ~MeasureBase()
        {
            if (measureThread != null)
            {
                measureThread.Abort();
                measureThread = null;
            }
            MeasureTimer.Dispose();
        }
    }


}
