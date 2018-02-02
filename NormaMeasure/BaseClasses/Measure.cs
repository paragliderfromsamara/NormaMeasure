using System;
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
        HAND = 1,
        AUTO = 2,
        VERIFICATION = 3
    }
    public class MeasureBase
    {
        protected int number = 0;
        protected int cycleNumber;
        protected int statCycleNumber;
        public List<MeasureResultCollection> ResultCollectionsList = new List<MeasureResultCollection>();
        /// <summary>
        /// Номер испытания в данном цикле
        /// </summary>
        public int StatCycleNumber
        {
            get { return statCycleNumber; }
            set { this.statCycleNumber = value; }
        }
        /// <summary>
        /// Номер испытания в текущем сеансе
        /// </summary>
        public int Number
        {
            get { return number; }
            set
            {
                this.number = value;
                CycleNumber = 1;
                ResultCollectionsList.Add(new MeasureResultCollection(CycleNumber));
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



        public MEASURE_TYPE MeasureType = MEASURE_TYPE.HAND;
        protected MEASURE_STATUS measureStatus;
        public MEASURE_STATUS MeasureStatus
        {
            get { return measureStatus; }
            set
            {
                MEASURE_STATUS was = measureStatus;
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
            this.MeasureType = MEASURE_TYPE.AUTO;
            this.MeasureStatus = MEASURE_STATUS.NOT_STARTED;
        }

        public MeasureBase(MEASURE_TYPE type)
        {
            this.MeasureType = type;
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
            MeasureTimer = new System.Timers.Timer(300);
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
                switch(MeasureType)
                {
                    case MEASURE_TYPE.AUTO:
                        measureThread = new Thread(autoMeasureThreadFunction);
                        break;
                    case MEASURE_TYPE.HAND:
                        this.Number++;
                        measureThread = new Thread(handMeasureThreadFunction);
                        break;
                    case MEASURE_TYPE.VERIFICATION:
                        measureThread = new Thread(verificationMeasureThreadFunction);
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
            if (this.stop()) this.MeasureStatus = status;
        }

        public static void measureError(string m)
        {
            MessageBox.Show("Ошибка испытания", m, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
