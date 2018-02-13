using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using System.Windows.Forms;
using NormaMeasure.Utils;
using NormaMeasure.BaseClasses;
using NormaMeasure.DBClasses;
using NormaMeasure.Teraohmmeter.DBClasses;

namespace NormaMeasure.Teraohmmeter
{
    public enum TERA_RESULT_STATUS : byte
    {
        SUCC = 0, //0-испытание окончено успешно
        INTEGRATOR_IS_ON_NEGATIVE = 1,          //1-интегратор находится в отрицательной области
        SHORT_CIRCUIT = 2 //2-короткое замыкание
    }


    public class TeraMeasure : MeasureBase
    {
        TeraDevice teraDevice;

        IsolationMaterial material;
        private TeraEtalonMap etalonMap;
        private int etalonId;
        private int rangeId;
        private int temperature = 20;
        private int voltage = 10;
        private int dischargeDelay = 0;
        private int polarizationDelay = 0;
        private bool isCyclicMeasure = true;
        private int cycleTimes = 100;
        private int averagingTimes = 1;
        private int externalCamDiam = 54;
        private int internalCamDiam = 50;
        private string bringingTypeId = "1";
        private int materialHeight = 1000;
        private bool isDegreeViewMode = false;
        private int minTimeToNorm = 1;
        private int normaValue = 0;
        private string materialId = "1";
        private int bringingLength = 1000;
        private string bringingLengthMeasure = "м";
        public byte voltageId = 0;
        public double EtalonVal = 10000;
        public double MaxDeviationPercent = 5;
        private string materialName = String.Empty;
        private string testedMaterialId = String.Empty;
        //------для режима коррекции-----------------
        public float VoltageCoeff = 1;
        public float RangeCoeff = 1;
        public float AdditionalCoeff = 0;
        public MEASURE_TYPE CorrectionMode = MEASURE_TYPE.AUTO; //режим коррекции, автоматический или ручной
        //-------------------------------------------
        
        /// <summary>
        /// Индекс эталона в массиве значений карты эталонов
        /// </summary>
        public int EtalonId
        {
            get
            {
                return this.etalonId;
            }
            set
            {
                this.etalonId = (this.Type == MEASURE_TYPE.CALIBRATION) ? this.EtalonMap.CalibrationEtalonNumbers[value] : value;
                this.rangeId = value;
            }
        }

        public TeraEtalonMap EtalonMap
        {
            get { return this.etalonMap; }
            set
            {
                this.etalonMap = value;
                float[][] eList = this.etalonMap.ResistanceList;
                for (int res = 0; res < eList.Length; res++)
                {
                    for (int volt = 0; volt < eList[res].Length; volt++)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Температура испытаний
        /// </summary>
        public int Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (temperature != value)
                {
                    writeToIni("Temperature", value.ToString());
                    temperature = value;
                }
            }
        }


        /// <summary>
        /// Испытательное напряжение, Вольт
        /// </summary>
        public int Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                if (voltage != value)
                {
                    writeToIni("Voltage", value.ToString());
                    voltage = value;
                }
                setVoltageId();
            }
        }

        public byte VoltageId { get { return this.voltageId; } }
        /// <summary>
        /// Время разряда, сек
        /// </summary>
        public int DischargeDelay
        {
            get
            {
                return dischargeDelay;
            }
            set
            {
                if (dischargeDelay != value)
                {
                    writeToIni("DischargeDelay", value.ToString());
                    dischargeDelay = value;
                }
            }
        }
        /// <summary>
        /// Время поляризации, сек
        /// </summary>
        public int PolarizationDelay
        {
            get
            {
                return polarizationDelay;
            }
            set
            {
                if (polarizationDelay != value)
                {
                    writeToIni("PolarizationDelay", value.ToString());
                    polarizationDelay = value;
                }
            }
        }
        /// <summary>
        /// Циклично или не циклично
        /// </summary>
        public bool IsCyclicMeasure
        {
            get
            {
                return isCyclicMeasure;
            }
            set
            {
                if (isCyclicMeasure != value)
                {
                    writeToIni("IsCyclicMeasure", value.ToString());
                    isCyclicMeasure = value;
                }
            }
        }
        /// <summary>
        /// Количество циклов
        /// </summary>
        public int CycleTimes
        {
            get
            {
                return cycleTimes;
            }
            set
            {
                if (cycleTimes != value)
                {
                    writeToIni("CycleTimes", value.ToString());
                    cycleTimes = value;
                }
            }
        }
        /// <summary>
        /// Количество циклов усреднения
        /// </summary>
        public int AveragingTimes
        {
            get
            {
                return averagingTimes;
            }
            set
            {
                if (averagingTimes != value)
                {
                    writeToIni("AveragingTimes", value.ToString());
                    averagingTimes = value;
                }

            }
        }
        /// <summary>
        /// Внешний диаметр электрода камеры, мм
        /// </summary>
        public int ExternalCamDiam
        {
            get
            {
                return externalCamDiam;
            }
            set
            {
                if (externalCamDiam != value)
                {
                    writeToIni("ExternalCamDiam", value.ToString());
                    externalCamDiam = value;
                }
            }
        }

        /// <summary>
        /// Внутренний диаметр электрода камеры, мм
        /// </summary>
        public int InternalCamDiam
        {
            get
            {
                return internalCamDiam;
            }
            set
            {
                if (internalCamDiam != value)
                {
                    writeToIni("InternalCamDiam", value.ToString());
                    internalCamDiam = value;
                }
            }
        }
        /// <summary>
        /// Индекс типа приведения
        /// </summary>
        public string BringingTypeId
        {
            get
            {
                return bringingTypeId;
            }
            set
            {
                if (bringingTypeId != value)
                {
                    writeToIni("BringingTypeId", value.ToString());
                    bringingTypeId = value;
                }
            }
        }
        /// <summary>
        /// Толщина материала, мкм
        /// </summary>
        public int MaterialHeight
        {
            get
            {
                return materialHeight;
            }
            set
            {
                if (materialHeight != value)
                {
                    writeToIni("MaterialHeight", value.ToString());
                    materialHeight = value;
                }
            }
        }

        /// <summary>
        /// Название материала
        /// </summary>
        public string MaterialName
        {
            get
            {
                return materialName;
            }
            set
            {
                if (materialName != value)
                {
                    writeToIni("MaterialName", value.ToString());
                    materialName = value;
                }
            }
        }

        /// <summary>
        /// Идентификатор испытуемого образца
        /// </summary>
        public string TestedMaterialId
        {
            get
            {
                return testedMaterialId;
            }
            set
            {
                if (testedMaterialId != value)
                {
                    writeToIni("TestedMaterialId", value.ToString());
                    testedMaterialId = value;
                }
            }
        }

        /// <summary>
        /// Степенной вид результата
        /// </summary>
        public bool IsDegreeViewMode
        {
            get
            {
                return isDegreeViewMode;
            }
            set
            {
                if (isDegreeViewMode != value)
                {
                    writeToIni("IsDegreeViewMode", value.ToString());
                    isDegreeViewMode = value;
                }
            }
        }
        /// <summary>
        /// Минимальное время достижения нормы, минут
        /// </summary>
        public int MinTimeToNorm
        {
            get
            {
                return minTimeToNorm;
            }
            set
            {
                if (minTimeToNorm != value)
                {
                    writeToIni("MinTimeToNorm", value.ToString());
                    minTimeToNorm = value;
                }

            }
        }
        /// <summary>
        /// Значение нормы, МОм
        /// </summary>
        public int NormaValue
        {
            get
            {
                return normaValue;
            }
            set
            {
                if (normaValue != value)
                {
                    writeToIni("NormaValue", value.ToString());
                    normaValue = value;
                }
            }
        }
        /// <summary>
        /// Индекс материала в БД
        /// </summary>
        public string MaterialId
        {
            get
            {
                return materialId;
            }
            set
            {
                if (materialId != value)
                {
                    writeToIni("MaterialId", value.ToString());
                    materialId = value;

                }
                this.material = new IsolationMaterial(value);
            }
        }

        public IsolationMaterial Material
        {
            get { return this.material; }
        }
        /// <summary>
        /// Длина приведения
        /// </summary>
        public int BringingLength
        {
            get
            {
                return bringingLength;
            }
            set
            {
                if (bringingLength != value)
                {
                    writeToIni("BringingLength", value.ToString());
                    bringingLength = value;
                }
            }
        }
        /// <summary>
        /// Мера длины приведения
        /// </summary>
        public string BringingLengthMeasure
        {
            get
            {
                return bringingLengthMeasure;
            }
            set
            {
                if (bringingLengthMeasure != value)
                {
                    writeToIni("BringingLengthMeasure", value.ToString());
                    bringingLengthMeasure = value;
                }
            }
        }
        public bool IsStatistic
        {
            get
            {
                return this.AveragingTimes > 1;
            }
        }

        private void setVoltageId()
        {
            switch (voltage)
            {
                case 10:
                    this.voltageId = 0x01;
                    break;
                case 100:
                    this.voltageId = 0x02;
                    break;
                case 500:
                    this.voltageId = 0x03;
                    break;
                case 1000:
                    this.voltageId = 0x04;
                    break;
                default:
                    this.voltageId = 0x00;
                    break;
            }
        }
        private void writeToIni(string key, string val)
        {
            if (teraDevice == null || Type == MEASURE_TYPE.AUTO) return;
            IniFile file = new IniFile(Properties.Settings.Default.DeviceSettingsFileName);
            string sect = teraDevice.IniSectionName();
            file.Write(key, val, sect);
        }

        public TeraMeasure() : base()
        {
            
        }

        public TeraMeasure(TeraDevice tera, MEASURE_TYPE t) : base(t)
        {
            this.teraDevice = tera;
        }

        protected override void initByMeasureType()
        {
            base.initByMeasureType();
            switch (Type)
            {
                case MEASURE_TYPE.HAND:
                    fillDataFromIniFile();
                    return;
                case MEASURE_TYPE.AUTO:
                    return;
                case MEASURE_TYPE.CALIBRATION:
                    return;
            }
        }



        private void fillDataFromIniFile()
        {
            if (teraDevice == null) return;
            string sect = teraDevice.IniSectionName();
            IniFile file = new IniFile(Properties.Settings.Default.DeviceSettingsFileName);
            this.temperature = ServiceFunctions.convertToInt16(file.ReadOrWrite("Temperature", sect, this.Temperature.ToString()));
            this.voltage = ServiceFunctions.convertToInt16(file.ReadOrWrite("Voltage", sect, this.Voltage.ToString()));
            setVoltageId();
            this.dischargeDelay = ServiceFunctions.convertToInt16(file.ReadOrWrite("DischargeDelay", sect, this.DischargeDelay.ToString()));
            this.polarizationDelay = ServiceFunctions.convertToInt16(file.ReadOrWrite("PolarizationDelay", sect, this.PolarizationDelay.ToString()));
            this.isCyclicMeasure = file.ReadOrWrite("IsCyclicMeasure", sect, this.IsCyclicMeasure.ToString()).ToLower() == "true";
            this.cycleTimes = ServiceFunctions.convertToInt16(file.ReadOrWrite("CycleTimes", sect, this.CycleTimes.ToString()));
            this.averagingTimes = ServiceFunctions.convertToInt16(file.ReadOrWrite("AveragingTimes", sect, this.AveragingTimes.ToString()));
            this.externalCamDiam = ServiceFunctions.convertToInt16(file.ReadOrWrite("ExternalCamDiam", sect, this.ExternalCamDiam.ToString()));
            this.internalCamDiam = ServiceFunctions.convertToInt16(file.ReadOrWrite("InternalCamDiam", sect, this.InternalCamDiam.ToString()));
            this.bringingTypeId = file.ReadOrWrite("BringingTypeId", sect, this.BringingTypeId.ToString());
            this.materialHeight = ServiceFunctions.convertToInt16(file.ReadOrWrite("MaterialHeight", sect, this.MaterialHeight.ToString()));
            this.isDegreeViewMode = file.ReadOrWrite("IsDegreeViewMode", sect, this.IsDegreeViewMode.ToString()).ToLower() == "true";
            this.minTimeToNorm = ServiceFunctions.convertToInt16(file.ReadOrWrite("MinTimeToNorm", sect, this.MinTimeToNorm.ToString()));
            this.normaValue = ServiceFunctions.convertToInt32(file.ReadOrWrite("NormaValue", sect, this.NormaValue.ToString()));
            this.materialId = file.ReadOrWrite("MaterialId", sect, this.materialId.ToString());
            this.bringingLength = ServiceFunctions.convertToInt16(file.ReadOrWrite("BringingLength", sect, this.bringingLength.ToString()));
            this.bringingLengthMeasure = file.ReadOrWrite("BringingLengthMeasure", sect, this.bringingLengthMeasure);
        }

        private void polarisation()
        {
            this.MeasureStatus = MEASURE_STATUS.POLAR;
            int delay = this.PolarizationDelay * 60;
            resetTime();
            do
            {
                this.teraDevice.RetentionVoltage();
                Thread.Sleep(750);
            } while (delay > measSeconds);
        }

        private void discharge()
        {
            resetTime();
            MEASURE_STATUS was = MeasureStatus;
            this.MeasureStatus = MEASURE_STATUS.DISCHARGE;
            do { } while (DischargeDelay > measSeconds);
            resetTime();
            if (was == MEASURE_STATUS.STOPED) this.MeasureStatus = MEASURE_STATUS.STOPED;
            else if (was == MEASURE_STATUS.DISCHARGE) this.MeasureStatus = MEASURE_STATUS.FINISHED;

        }

        private void measure()
        {
            resetTime();
            this.MeasureStatus = MEASURE_STATUS.IS_GOING;
            do
            {
                this.teraDevice.DeviceForm.updateResultField();
                MeasureResultTera result = new MeasureResultTera(this, this.teraDevice);
                this.teraDevice.DoMeasure(ref result);
                if (!result.IsCompleted) { TeraMeasure.measureError("Превышено время ожидания результата"); break; }
                result.SecondsFromStart = measSeconds;
                if (result.Status > 0) break;
                if (Properties.Settings.Default.IsTestApp) Thread.Sleep(2000);
                ResultCollectionsList[this.curResultListId].Add(result);
                StatCycleNumber++;
                if (StatCycleNumber <= this.AveragingTimes) continue; //Если статистическое испытание, то уходим на следующий подцикл
                this.teraDevice.DeviceForm.updateResultField();
                this.CycleNumber++;
                if (!this.IsCyclicMeasure && this.CycleNumber > this.CycleTimes) break;
            } while (true);
        }

        protected override void handMeasureThreadFunction()
        {
            this.teraDevice.setVoltage(this.VoltageId);
            Thread.Sleep(500);
            if (this.PolarizationDelay > 0 && NormaValue == 0) polarisation();
            measure();
            if (this.DischargeDelay > 0) discharge();
            this.MeasureStatus = MEASURE_STATUS.FINISHED;
        }

        protected override void calibrationMeasureThreadFunction()
        {
            this.normaValue = Convert.ToInt32(this.EtalonMap.ResistanceList[this.EtalonId][0]);
            this.teraDevice.setVoltage(this.VoltageId);
            if (this.CorrectionMode == MEASURE_TYPE.AUTO) this.RangeCoeff = 1;
            Thread.Sleep(500);
            if (this.PolarizationDelay > 0) polarisation();
            measure();
            if (this.CorrectionMode == MEASURE_TYPE.AUTO)
            {
                double coeff = 0;
                foreach(MeasureResult r in CurrentCollection.ResultsList)
                {
                    coeff += this.normaValue / (r.BringingResult * 1000) ; 
                }
                coeff = Math.Round(coeff / CurrentCollection.Count, 2);
                this.teraDevice.DeviceForm.updateCoeffField(coeff);
            }
            this.MeasureStatus = MEASURE_STATUS.FINISHED;
        }

        /// <summary>
        /// Обновляет поле статуса испытания
        /// </summary>
        /// <param name="stat"></param>
        protected override void updatedMeasureStatusHandle()
        {
            base.updatedMeasureStatusHandle();
            if (this.MeasureStatus != MEASURE_STATUS.NOT_STARTED) this.teraDevice.DeviceForm.updateMeasureStatus(this.MeasureStatus);
        }

        /// <summary>
        /// Обновляет поле результата в текстовом формате
        /// </summary>
        /// <param name="txt"></param>
        private void updateResultFieldText(string txt)
        {
            if (this.Type == MEASURE_TYPE.HAND) this.teraDevice.DeviceForm.updateResultFieldText(txt);
        }

        /// <summary>
        /// Обновляет поля статистических испытаний
        /// </summary>
        /// <param name="txt"></param>
        private void updateStatMeasInfo(string[] txt)
        {
            if (this.Type == MEASURE_TYPE.HAND) this.teraDevice.DeviceForm.updateStatMeasInfo(txt);
        }

        protected override void autoMeasureThreadFunction()
        {
            throw new NotImplementedException();
        }

        protected override void verificationMeasureThreadFunction()
        {
            if (ResultCollectionsList.Count > 0) return;
        }


        public static string StatusString(MEASURE_STATUS sts)
        {
            switch (sts)
            {
                case MEASURE_STATUS.POLAR:
                    return "Поляризация";
                case MEASURE_STATUS.DISCHARGE:
                    return "Деполяризация";
                case MEASURE_STATUS.FINISHED:
                    return "Измерение завершено";
                case MEASURE_STATUS.IS_GOING:
                    return "Идёт измерение";
                case MEASURE_STATUS.STOPED:
                    return "Остановлено";
                case MEASURE_STATUS.NOT_STARTED:
                default:
                    return "Не начато";

            }
        }

        protected override void measureTimerHandler()
        {
            int sec;
            switch (measureStatus)
            {
                case MEASURE_STATUS.POLAR:
                    sec = (this.PolarizationDelay * 60) - measSeconds;
                    break;
                case MEASURE_STATUS.DISCHARGE:
                    sec = this.DischargeDelay - measSeconds;
                    break;
                case MEASURE_STATUS.IS_GOING:
                    if (this.PolarizationDelay > 0 && this.NormaValue > 0)
                    {
                        sec = (this.PolarizationDelay * 60) - measSeconds;
                        if (sec == 0)
                        {
                            this.StopWithStatus(this.DischargeDelay > 0 ? MEASURE_STATUS.DISCHARGE : MEASURE_STATUS.FINISHED);
                        }
                    }
                    else sec = measSeconds;
                    break;
                default:
                    sec = measSeconds;
                    break;
            }
            this.teraDevice.DeviceForm.RefreshMeasureTimer(sec);
        }

        public override void StopWithStatus(MEASURE_STATUS status)
        {
            base.StopWithStatus(status);
            if (DischargeDelay > 0)
            {
                resetTime();
                this.measureThread = new Thread(discharge);
                this.measureThread.Start();
                MeasureTimer.Start();
            }
        }


        protected override string getName()
        {
            switch(this.Type)
            {
                case MEASURE_TYPE.CALIBRATION:
                    return String.Format("Калибровка - Диапазон {0}", this.rangeId+1);
                default:
                    return base.getName();
            }
        }

    }

}
