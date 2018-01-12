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
       
        public static byte[][] FakeDevList = new byte[][] {
                                                       new byte[]{ 15, 36, 114, 152},
                                                       new byte[]{ 14, 13, 66, 25},
                                                       new byte[]{ 12, 33, 45, 210}
                                                       //new byte[]{ 16, 3, 195, 133},
                                                       //new byte[]{ 13, 23, 251, 23},
                                                       //new byte[]{ 15, 2, 111, 222}
                                                   };
        /// <summary>
        /// Возвращает фэйковые номера портов в виде адресов массива с фэйковыми устройствами
        /// </summary>
        /// <returns></returns>
        public static string[] FakePortNumbers()
        {
            string[] s = new string[FakeDevList.Length];
            for (int i = 0; i < FakeDevList.Length; i++)
            {
                s[i] = i.ToString();
            }
            return s;
        }
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
