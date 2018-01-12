using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NormaMeasure.MeasureClasses
{
    public enum MEASURE_TYPE : byte
    {
        HAND = 1,
        AUTO = 2
    }
    public abstract class MeasureBase
    {
        public MEASURE_TYPE MeasureType = MEASURE_TYPE.HAND;
        public bool IsStarted = false;
        protected Thread measureThread = null;

        protected abstract void handMeasureThreadFunction();
        protected abstract void autoMeasureThreadFunction();

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
                        measureThread = new Thread(handMeasureThreadFunction);
                        break;
                }
                if (measureThread == null)
                {
                    this.IsStarted = false;
                }
                else
                {
                    measureThread.Start();
                    this.IsStarted = true;
                }
                
            }
            return IsStarted;
        }

        public void Stop()
        {
            if (measureThread != null)
            {
                measureThread.Abort();
                measureThread = null;
                this.IsStarted = false;
            }
        }

        public static void measureError(string m)
        {
            MessageBox.Show("Ошибка испытания", m, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }


}
