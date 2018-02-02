using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.BaseClasses
{
    public abstract class MeasureResult
    {
        /// <summary>
        /// Абсолютный результат
        /// </summary>
        public double AbsoluteResult; //Абсолютные результат
        /// <summary>
        /// Приведенный результатат
        /// </summary>
        public double BringingResult; //Приведенный результат
        /// <summary>
        /// Время получение результата с момента начала испытания
        /// </summary>
        public int SecondsFromStart;
        /// <summary>
        /// Номер испытания
        /// </summary>
        public int MeasureNumber = 0;
        /// <summary>
        /// Номер цикла испытаний
        /// </summary>
        public int CycleNumber = 0;
        /// <summary>
        /// Номер подцикла испытаний (для статистических измерений)
        /// </summary>
        public int StatCycleNumber = 0;
    }
    public class MeasureResultCollection
    {
        private int measureNumber;
        public int MeasureNumber { get { return measureNumber; } }
        public int Count
        {
            get
            {
                return resultsList.Count;
            }
        }

        private enum ReturnedValue : byte
        {
            MAX = 1,
            MIN = 2,
            AVERAGE = 3
        }

        private enum ReturnedVariable : byte
        {
            BRINGING_RESULT = 1,
            ABSOLUTE_RESULT = 2
        }

        private List<MeasureResult> resultsList;

        public List<MeasureResult> ResultsList
        {
            get
            {
                return resultsList;
            }
        }

        public MeasureResultCollection()
        {
            resultsList = new List<MeasureResult>();
            this.measureNumber = 1;
        }

        /// <summary>
        /// Создаём коллекцию результатов с номером испытания
        /// </summary>
        /// <param name="measure_number"></param>
        public MeasureResultCollection(int measure_number) : this()
        {
            this.measureNumber = measure_number;
        }

        public void Add(MeasureResult result)
        {
            resultsList.Add(result);
        }

        public void Clear()
        {
            resultsList.Clear();
        }

        public MeasureResult Last()
        {
            return resultsList.Last();
        }

        public MeasureResult First()
        {
            return resultsList.First();
        }





        /// <summary>
        /// Максимальное значение абсолютного результата в коллекции
        /// </summary>
        /// <returns></returns>
        public double MaxAbsolute()
        {
            return getStat(ReturnedValue.MAX, ReturnedVariable.ABSOLUTE_RESULT);
        }

        /// <summary>
        /// Минимальное значение абсолютного результата в коллекции
        /// </summary>
        /// <returns></returns>
        public double MinAbsolute()
        {
            return getStat(ReturnedValue.MIN, ReturnedVariable.ABSOLUTE_RESULT);
        }

        /// <summary>
        /// Среднее значение абсолютного результата в коллекции
        /// </summary>
        /// <returns></returns>
        public double AverageAbsolute()
        {
            return getStat(ReturnedValue.AVERAGE, ReturnedVariable.ABSOLUTE_RESULT);
        }

        /// <summary>
        /// Среднее значение абсолютного результата
        /// </summary>
        /// <param name="measure_number">Номер испытания в данной серии</param>
        /// <param name="cycle_number">Номер цикла в данном испытании</param>
        /// <returns></returns>
        public double AverageAbsolute(int measure_number, int cycle_number)
        {
            MeasureResultCollection col = GetMeasureResultList(measure_number);
            col = col.getStatResultList(cycle_number);

            return col.AverageAbsolute();
        }

        /// <summary>
        /// Максимальное значение приведенного результата в коллекции
        /// </summary>
        /// <returns></returns>
        public double MaxBringing()
        {
            return getStat(ReturnedValue.MAX, ReturnedVariable.BRINGING_RESULT);
        }

        /// <summary>
        /// Минимальное значение приведенного результата
        /// </summary>
        /// <returns></returns>
        public double MinBringing()
        {
            return getStat(ReturnedValue.MIN, ReturnedVariable.BRINGING_RESULT);
        }

        /// <summary>
        /// Среднее значение приведенного результата
        /// </summary>
        /// <returns></returns>
        public double AverageBringing()
        {
            return getStat(ReturnedValue.AVERAGE, ReturnedVariable.BRINGING_RESULT);
        }

        /// <summary>
        /// Среднее значение приведенного результата
        /// </summary>
        /// <param name="measure_number">Номер испытания в данной серии</param>
        /// <param name="cycle_number">Номер цикла в данном испытании</param>
        /// <returns></returns>
        public double AverageBringing(int measure_number, int cycle_number)
        {
            MeasureResultCollection col = GetMeasureResultList(measure_number);
            col = col.getStatResultList(cycle_number);
            return col.AverageBringing();
        }

        public MeasureResultCollection GetMeasureResultList(int measure_number)
        {
            MeasureResultCollection col = new MeasureResultCollection();
            foreach (MeasureResult r in this.ResultsList) if (r.MeasureNumber == measure_number) col.Add(r);
            return col;
        }
        /// <summary>
        /// Ищет промежуточные результаты статистического испытания в рамках одного испытания по номеру цикла
        /// Список испытаний при этом, должен содержать только результаты одного испытания
        /// </summary>
        /// <param name="cycle_number">Номер цикла текущего испытания</param>
        /// <returns></returns>
        private MeasureResultCollection getStatResultList(int cycle_number)
        {
            MeasureResultCollection col = new MeasureResultCollection();
            foreach (MeasureResult r in this.ResultsList) if (r.CycleNumber == cycle_number) col.Add(r);
            return col;
        }

        private double getStat(ReturnedValue type, ReturnedVariable var)
        {
            if (resultsList.Count == 0) return 0;
            double[] arr = new double[resultsList.Count];
            switch(var)
            {
                case ReturnedVariable.ABSOLUTE_RESULT:
                    for (int i = 0; i < resultsList.Count; i++) arr[i] = resultsList[i].AbsoluteResult;
                    break;
                case ReturnedVariable.BRINGING_RESULT:
                    for (int i = 0; i < resultsList.Count; i++) arr[i] = resultsList[i].AbsoluteResult;
                    break;
            }
            switch(type)
            {
                case ReturnedValue.AVERAGE:
                    return arr.Average();
                case ReturnedValue.MIN:
                    return arr.Min();
                case ReturnedValue.MAX:
                    return arr.Max();
                default:
                    return 0;
            }
        }
    }

}
