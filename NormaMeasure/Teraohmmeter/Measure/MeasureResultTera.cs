using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.BaseClasses;
using NormaMeasure.DBClasses;


namespace NormaMeasure.Teraohmmeter
{
    public class MeasureResultTera : MeasureResult
    {
        public TeraDevice TeraDevice;
        public TeraMeasure Measure;
        public IsolationMaterial Material;

        public int Voltage;
        public bool IsCompleted; // статус результата принято или нет
        public int Range; //Диапазон
        public int Status; //Статус
        public int MeasureTime; //Время измерения
        public int FirstMeasure; //Начальное состояние интегратора
        private int lastMeasure;
        public int BringingLength;
        public int Temperature;
        public string BringingTypeId;
        public int InternalCamDiam;
        public int ExternalCamDiam;
        public string BringingLengthMeasure;
        public int MaterialHeight;
        public string ResultMeasure;


        public int LastMeasure //Конечное состояние интегратора
        {
            get
            {
                return this.lastMeasure;
            }set
            {
                this.lastMeasure = value;
                convertAdcResult();
                this.IsCompleted = true;
            }
        }

        private MeasureResultTera()
        {
            this.IsCompleted = false;
        }

        public MeasureResultTera(TeraMeasure m, TeraDevice d) : base()
        {
            this.Measure = m;
            this.TeraDevice = d;
            this.CycleNumber = m.CycleNumber;
            this.MeasureNumber = m.Number;
            this.StatCycleNumber = m.StatCycleNumber;
            this.Voltage = m.Voltage;
            this.Material = m.Material;
            this.Temperature = m.Temperature;
            this.BringingTypeId = m.BringingTypeId;
            this.BringingLengthMeasure = m.BringingLengthMeasure;
        }



        private void convertAdcResult(float rangeCoeff, float voltCoeff, float additionalRangeCoeff)
        {
            double _R = 0;
            double integratorDifference = (this.LastMeasure > this.FirstMeasure) ? ((double)this.LastMeasure - (double)this.FirstMeasure) : 0;
            double capacity = this.TeraDevice.capacitiesList[this.Range];
            double v = (double)Measure.Voltage / 100;
            double[] limit = new double[] { 20000, 200000, 1000000, 2000000 };
            double mTime = Convert.ToDouble(this.MeasureTime);
            //string s = String.Format("Статус: {0}; Диапазон: {1}; Длительность: {2}; Начальное состояние: {3}; Конечное состояние: {4};", this.measStatus, this.rangeId, this.measTime, this.firstMeasure, this.lastMeasure);
            //this.mForm.updateServiceField(s);
            if (integratorDifference > 0)
            {
                _R = (2048.0 * v * (double)(this.MeasureTime)) / (this.TeraDevice.refVoltage * integratorDifference * capacity);
            }
            else
            {
                _R = 0;
            }
            if ((mTime == Convert.ToDouble(1499)) && (_R < (limit[Measure.VoltageId])) && (_R > 0))
                if (integratorDifference > 0) _R = (2048.0 * v * (double)(this.MeasureTime)) / (this.TeraDevice.refVoltage * (integratorDifference - (double)additionalRangeCoeff) * capacity);

            _R *= (double)rangeCoeff;      			                    // Умножаем на коэф. коррекции от диапазона.
            if (Measure.Voltage > 10) _R *= (double)voltCoeff; // Умножаем на коэф. коррекции от напряжения если оно не 10В.
            if (_R > 0.0004) _R -= this.TeraDevice.zeroResistance;                                     // Вычитаем 300кОм последовательно включён
            if (_R < 0.005 && _R > 0) _R += 0.00013; // Если меньше 5МОм то добавляем 170 кОм
            this.AbsoluteResult = _R; // * this.bringingCoeff;

            this.BringingResult = _R * this.calculateMaterialCoeff();
        }
        /// <summary>
        /// Срабатывает, когда устанавливается свойство LastMeasure
        /// </summary>
        private void convertAdcResult()
        {
            float rangeCoeff, voltCoeff, additionalRangeCoeff;
            if (this.Measure.MeasureType != MEASURE_TYPE.CALIBRATION)
            {
                rangeCoeff = this.TeraDevice.rangeCoeffs[this.Range];
                voltCoeff = this.Voltage > 10 ? this.TeraDevice.voltageCoeffs[this.Measure.VoltageId - 2] : 1;
                additionalRangeCoeff = this.TeraDevice.rangeCoeffs[4];
                
            }else
            {
                rangeCoeff = this.Measure.RangeCoeff;
                voltCoeff = this.Measure.VoltageCoeff;
                additionalRangeCoeff = this.Measure.AdditionalCoeff;
            }
            this.convertAdcResult(rangeCoeff, voltCoeff, additionalRangeCoeff);
        }


        /// <summary>
        /// Абсолютный вид результата
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string AbsResultViewWithMeasure(double r)
        {
            string[] quntMeas = new string[] { "МОм", "ГОм", "ТОм" };
            int qIdx = 0;
            double mult = 0;
            int rnd = 0;
            if (r >= 1)
            {
                if ((r / 1000) >= 1) { qIdx = 2; mult = 0.001; }
                else { qIdx = 1; mult = 1; }
            }
            else
            {
                qIdx = 0;
                mult = 1000;
            }
            r *= mult;
            if (r > 99) rnd = 2;
            else rnd = 3;
            return String.Format("{0} {1}", Math.Round(r, rnd), quntMeas[qIdx]);
        }

        /// <summary>
        /// Степенной вид результата
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string DegResultViewWithMeasure(double r)
        {
            double dg = 0;
            double maxDg = 9; //максимальный порядок
            r *= 1000.0;
            for (dg = 0; dg <= maxDg; dg++)
            {
                double curVal = Math.Pow(10, dg);
                double nextVal = curVal * 10;
                bool cResult = (dg == 0) ? (r >= 0) : r >= curVal;
                cResult &= r < nextVal;
                if (cResult) break;
            }
            r /= Math.Pow(10, dg);
            return String.Format("{0}Е{1} МОм", Math.Round(r, 2), dg);
        }


        private double calculateMaterialCoeff()
        {
            double coeff = (double)this.Material.GetCoefficient(this.Temperature);
            switch (this.BringingTypeId)
            {
                case "1": //без приведения
                    break;
                case "2": //к длине
                    double length = this.BringingLength;
                    switch (this.BringingLengthMeasure)
                    {
                        case "см":
                            length *= 100;
                            break;
                        case "дм":
                            length *= 10;
                            break;
                        case "м":
                            break;
                        case "км":
                            length /= 1000;
                            break;
                    }
                    coeff *= length;
                    break;
                case "3": //объёмное
                    double sumDiamHalf = (this.ExternalCamDiam + this.InternalCamDiam) / 2;
                    coeff *= (Math.PI * Math.Pow(sumDiamHalf, 2)) / (this.MaterialHeight * 4);
                    break;
                case "4": //поверхностное
                    coeff *= Math.PI * (this.ExternalCamDiam + this.InternalCamDiam) / (this.ExternalCamDiam - this.InternalCamDiam);
                    break;
            }
            return coeff;
        }

    }
}
