using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormaMeasure.BaseClasses
{
    public abstract class MeasureResult
    {
        public double AbsoluteResult; //Абсолютные результат
        public double BringingResult; //Приведенный результат
    }
    public class MeasureResultCollection
    {
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
        }

        public void Add(MeasureResult result)
        {
            resultsList.Add(result);
        }

        public void Clear()
        {
            resultsList.Clear();
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
