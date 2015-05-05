using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Interfaces;

namespace SiteParse.Distance
{
    class PowerLowDistance: IDistanceMetric
    {

        public const double P = 2.0d;

        public const double R = 2.0d;
        /// <summary>
        ///  Применяется в случае, когда необходимо увеличить или уменьшить вес, относящийся к размерности, 
        /// для которой соответствующие объекты сильно отличаются. Степенное расстояние вычисляется по следующей формуле:
        /// </summary>
        /// <param name="set1"></param>
        /// <param name="set2"></param>
        /// <returns></returns>

        public double GetDistance(object[] set1, object[] set2)
        {
            try
            {
                var sumSquaredDiffs = 0.0;
                for (var i = 0; i < set1.Length; ++i)
                {
                    sumSquaredDiffs += Math.Pow((Convert.ToDouble(set1[i]) - Convert.ToDouble(set2[i])), P);
                }

                return Math.Pow(sumSquaredDiffs,1d/R);
            }
            catch (Exception ex)
            {  
                MessageBox.Show(ex.Message);
                return -1;
            }       
        }
    }
    {
    }
}
