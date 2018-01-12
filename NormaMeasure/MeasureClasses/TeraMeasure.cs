using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NormaMeasure.DevicesClasses;
using NormaMeasure.Utils;

namespace NormaMeasure.MeasureClasses
{
    public enum TERA_RESULT_STATUS : byte
    {
        SUCC = 0, //0-испытание окончено успешно
        INTEGRATOR_IS_ON_NEGATIVE = 1,          //1-интегратор находится в отрицательной области
        SHORT_CIRCUIT = 2 //2-короткое замыкание
    }

    public enum TERA_MEASURE_STAGE_STATUS : byte
    {
        POLAR = 0,
        DEPOLAR = 1,
        IN_AVERAGING_CYCLE = 2,
        FINISHED = 3

    }


    public struct TeraMeasureResult
    {
        public bool IsReceived; // статус принято или нет
        public int Range; //Диапазон
        public int Status; //Статус
        public int MeasureTime; //Время измерения
        public int FirstMeasure; //Начальное состояние интегратора
        public int LastMeasure; //Конечное состояние интегратора
        public double AbsoluteResult; //Абсолютные результат
        public double BringingResult; //Приведенный результат
    }
    public class TeraMeasure : MeasureBase
    {
        Teraohmmeter teraDevice;
        private List<TeraMeasureResult> ResultList;
        private int temperature = 20;
        private int voltage = 10;
        private int dischargeDelay = 0;
        private int polarizationDelay = 0;
        private bool isCyclicMeasure = true;
        private int cycleTimes = 100;
        private int averagingTimes = 10;
        private int externalCamDiam = 54;
        private int internalCamDiam = 50;
        private string bringingTypeId = "1";
        private int materialHeight = 1000;
        private bool isDegreeViewMode = false;
        private int minTimeToNorm = 1;
        private int normaValue = 1000;
        private string materialId = "1";
        private int bringingLength = 1000;
        private int bringingLengthMeasureId = 0;
        private byte voltageId = 0;
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
            }set
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

            }
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
        public int BringingLengthMeasureId
        {
            get
            {
                return bringingLengthMeasureId;
            }
            set
            {
                if (bringingLengthMeasureId != value)
                {
                    writeToIni("BringingLengthMeasureId", value.ToString());
                    bringingLengthMeasureId = value;
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
                    this.voltageId = 0x03;
                    break;
            }
        }
        private void writeToIni(string key, string val)
        {
            if (teraDevice == null || MeasureType == MEASURE_TYPE.AUTO) return;
            IniFile file = new IniFile(Properties.Settings.Default.DeviceSettingsFileName);
            string sect = teraDevice.IniSectionName();
            file.Write(key, val, sect);
        }

        public TeraMeasure()
        {
            this.MeasureType = MEASURE_TYPE.AUTO;
        }

        public TeraMeasure(Teraohmmeter tera, MEASURE_TYPE t)
        {
            this.teraDevice = tera;
            this.MeasureType = t;
            initByMeasureType();
        }

        private void initByMeasureType()
        {
           switch(MeasureType)
            {
                case MEASURE_TYPE.HAND:
                    fillDataFromIniFile();
                    return;
                case MEASURE_TYPE.AUTO:
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
            this.normaValue = ServiceFunctions.convertToInt16(file.ReadOrWrite("NormaValue", sect, this.NormaValue.ToString()));
            this.materialId = file.ReadOrWrite("MaterialId", sect, this.materialId.ToString());
            this.bringingLength = ServiceFunctions.convertToInt16(file.ReadOrWrite("BringingLength", sect, this.bringingLength.ToString()));
            this.bringingLengthMeasureId = ServiceFunctions.convertToInt16(file.ReadOrWrite("BringingLengthMeasureId", sect, this.bringingLengthMeasureId.ToString()));
        
        }

        protected override void handMeasureThreadFunction()
        {
            int cycleCount = 0;
            int mCountLimit = this.AveragingTimes;
            int cycleCountLimit = this.CycleTimes;
            this.teraDevice.setVoltage(this.voltageId);
            Thread.Sleep(500);
            TeraMeasureResult result = new TeraMeasureResult();
            List<TeraMeasureResult> resultList = new List<TeraMeasureResult>();
            if (this.PolarizationDelay > 0)
            {
                this.teraDevice.DeviceForm.updateMeasureStatus("Поляризация...");
                int minute = this.PolarizationDelay;
                int sec = 1;
                do
                {
                    this.teraDevice.RetentionVoltage();
                    sec--;
                    if (sec == 0 && minute > 0)
                    {
                        sec = 59;
                        minute--;
                    }
                    this.teraDevice.DeviceForm.updateResultFieldText(ServiceFunctions.TimerTime(minute, sec));
                    Thread.Sleep(1000);
                    this.teraDevice.DeviceForm.updateResultFieldText(ServiceFunctions.TimerTime(minute, sec));
                } while (minute != 0 || sec != 0);
            }
            this.teraDevice.DeviceForm.updateResultFieldText("Измерение...");
            //int[] cmd = uTest.makeCmd;
            //uTest.setDiaps();
            do
            {
                this.teraDevice.DeviceForm.updateCycleNumberField((cycleCount + 1).ToString());
                this.teraDevice.StartIntegrator();
                result = this.getMeasureResult();
                if (!result.IsReceived) { TeraMeasure.measureError("Превышено время ожидания результата"); break; }
                if (result.Status > 0) break;
                if (Properties.Settings.Default.IsTestApp) Thread.Sleep(2000);
                resultList.Add(result);

                if (this.IsStatistic) this.teraDevice.DeviceForm.updateStatMeasInfo(new string[] { String.Format("{0} из {1}", resultList.Count, mCountLimit), this.teraDevice.DeviceForm.absoluteResultView(result.AbsoluteResult) });

                if (resultList.Count < mCountLimit) continue;
                result = calcAverage(resultList);
                this.teraDevice.DeviceForm.updateResultField(result);
                resultList.Clear();
                cycleCount++;
                if (!this.IsCyclicMeasure && cycleCount == this.CycleTimes) break;

                //счетчик циклов
                this.teraDevice.DeviceForm.updateResultField(result);
                //this.pause();
            } while (true);
            this.teraDevice.DeviceForm.updateMeasureStatus("Измерение не начато.");
            this.teraDevice.DeviceForm.switchFieldsMeasureOnOff(true);
            //this.teraMeasure.measTestForm.switchButtons(false);
        }

        private TeraMeasureResult calcAverage(List<TeraMeasureResult> resultList)
        {
            TeraMeasureResult result = resultList.Last();
            double absRes = 0;
            double brRes = 0;
            if (resultList.Count > 1)
            {
                foreach(TeraMeasureResult tmr in resultList)
                {
                    absRes += tmr.AbsoluteResult;
                    brRes += tmr.BringingResult;
                }
                result.AbsoluteResult = absRes / resultList.Count;
                result.BringingResult = brRes / resultList.Count;
            }
            return result;
        }

        private TeraMeasureResult getMeasureResult()
        {
            this.teraDevice.StartIntegrator();
            TeraMeasureResult r;
            int maxMeasTime = 400;
            int time = 0;
            do
            {
                r = this.teraDevice.CheckResult();
                if (r.IsReceived)
                {
                    if (r.Status == 0) r = convertAdcResult(r);
                    break;
                }
                Thread.Sleep(100);
                time++;
            } while (time < maxMeasTime);
           
            return r;
        }

        protected override void autoMeasureThreadFunction()
        {
            throw new NotImplementedException();
        }


        private TeraMeasureResult convertAdcResult(TeraMeasureResult result)
        {
            double _R = 0;
            double integratorDifference = (result.LastMeasure > result.FirstMeasure) ? ((double)result.LastMeasure - (double)result.FirstMeasure) : 0;
            double capacity = this.teraDevice.capacitiesList[result.Range];
            double v = (double)this.Voltage / 100;
            double[] limit = new double[] { 20000, 200000, 1000000, 2000000 };
            double mTime = Convert.ToDouble(result.MeasureTime);
            //string s = String.Format("Статус: {0}; Диапазон: {1}; Длительность: {2}; Начальное состояние: {3}; Конечное состояние: {4};", this.measStatus, this.rangeId, this.measTime, this.firstMeasure, this.lastMeasure);
            //this.mForm.updateServiceField(s);
            if (integratorDifference > 0)
            {
                _R = (2048.0 * v * (double)(result.MeasureTime)) / (this.teraDevice.refVoltage * integratorDifference * this.teraDevice.capacitiesList[result.Range]);
            }
            else
            {
                _R = 0;
            }
            if ((mTime == Convert.ToDouble(1499)) && (_R < (limit[this.voltageId])) && (_R > 0))
                if (integratorDifference > 0) _R = (2048.0 * v * (double)(result.MeasureTime)) / (this.teraDevice.refVoltage * (integratorDifference - (double)this.teraDevice.rangeCoeffs[5]) * this.teraDevice.capacitiesList[result.Range]);

            _R *= (double)this.teraDevice.rangeCoeffs[result.Range];      			                    // Умножаем на коэф. коррекции от диапазона.
            if (this.Voltage > 10) _R *= (double)this.teraDevice.voltageCoeffs[this.voltageId - 2]; // Умножаем на коэф. коррекции от напряжения если оно не 10В.
            if (_R > 0.0004) _R -= this.teraDevice.zeroResistance;                                     // Вычитаем 300кОм последовательно включён
            if (_R < 0.005 && _R > 0) _R += 0.00013; // Если меньше 5МОм то добавляем 170 кОм
            result.AbsoluteResult = _R; // * this.bringingCoeff;
            result.BringingResult = _R; 
            return result;
        }
    }
}
