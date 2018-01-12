using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NormaMeasure.DevicesClasses;
using NormaMeasure.Utils;

namespace NormaMeasure.DemoModeEntities
{
    abstract class DemoTera
    {
        public static float[] voltageCoeffs()
        {
            Teraohmmeter t = new Teraohmmeter();
            float[] a = new float[t.voltageCoeffs.Length];
            for (int i = 0; i < t.voltageCoeffs.Length; i++)
            {
                a[i] = ServiceFunctions.getRandomNearOne();
            }
            return a;
        }
        public static float[] rangeCoeffs()
        {
            Teraohmmeter t = new Teraohmmeter();
            float[] a = new float[t.rangeCoeffs.Length];
            for (int i = 0; i < t.rangeCoeffs.Length; i++)
            {
                a[i] = ServiceFunctions.getRandomNearOne();
            }
            return a;
        }
    }
}
