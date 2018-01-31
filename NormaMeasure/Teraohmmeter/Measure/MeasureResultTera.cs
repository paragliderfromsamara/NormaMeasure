using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.BaseClasses;


namespace NormaMeasure.Teraohmmeter
{
    public class MeasureResultTera : MeasureResult
    {
        public TeraDevice teraDevice;
        public TeraMeasure measure;
        public bool IsCompleted; // статус результата принято или нет
        public int Range; //Диапазон
        public int Status; //Статус
        public int MeasureTime; //Время измерения
        public int FirstMeasure; //Начальное состояние интегратора
        private int lastMeasure;
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
            this.measure = m;
            this.teraDevice = d;
        }

        /// <summary>
        /// Срабатывает, когда устанавливается свойство LastMeasure
        /// </summary>
        private void convertAdcResult()
        {
            double _R = 0;
            double integratorDifference = (this.LastMeasure > this.FirstMeasure) ? ((double)this.LastMeasure - (double)this.FirstMeasure) : 0;
            double capacity = this.teraDevice.capacitiesList[this.Range];
            double v = (double)measure.Voltage / 100;
            double[] limit = new double[] { 20000, 200000, 1000000, 2000000 };
            double mTime = Convert.ToDouble(this.MeasureTime);
            //string s = String.Format("Статус: {0}; Диапазон: {1}; Длительность: {2}; Начальное состояние: {3}; Конечное состояние: {4};", this.measStatus, this.rangeId, this.measTime, this.firstMeasure, this.lastMeasure);
            //this.mForm.updateServiceField(s);
            if (integratorDifference > 0)
            {
                _R = (2048.0 * v * (double)(this.MeasureTime)) / (this.teraDevice.refVoltage * integratorDifference * capacity);
            }
            else
            {
                _R = 0;
            }
            if ((mTime == Convert.ToDouble(1499)) && (_R < (limit[measure.VoltageId])) && (_R > 0))
                if (integratorDifference > 0) _R = (2048.0 * v * (double)(this.MeasureTime)) / (this.teraDevice.refVoltage * (integratorDifference - (double)this.teraDevice.rangeCoeffs[5]) * capacity);

            _R *= (double)this.teraDevice.rangeCoeffs[this.Range];      			                    // Умножаем на коэф. коррекции от диапазона.
            if (measure.Voltage > 10) _R *= (double)this.teraDevice.voltageCoeffs[this.measure.VoltageId - 2]; // Умножаем на коэф. коррекции от напряжения если оно не 10В.
            if (_R > 0.0004) _R -= this.teraDevice.zeroResistance;                                     // Вычитаем 300кОм последовательно включён
            if (_R < 0.005 && _R > 0) _R += 0.00013; // Если меньше 5МОм то добавляем 170 кОм
            this.AbsoluteResult = _R; // * this.bringingCoeff;

            this.BringingResult = _R*this.measure.BringingCoeff;
        }




    }
}
