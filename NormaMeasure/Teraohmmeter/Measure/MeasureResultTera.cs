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
        public bool IsReceived; // статус результата принято или нет
        public int Range; //Диапазон
        public int Status; //Статус
        public int MeasureTime; //Время измерения
        public int FirstMeasure; //Начальное состояние интегратора
        public int LastMeasure; //Конечное состояние интегратора

    }
}
