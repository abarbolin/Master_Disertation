using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Interfaces;

namespace SiteParse.Distance
{
    class ChebushevDistance:IDistanceMetric
    {
        /// <summary>
        /// Это расстояние может оказаться полезным, 
        /// когда нужно определить два объекта как «различные», если они различаются по какой-либо одной координате.
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
                    var difference = Math.Abs((Convert.ToDouble(set1[i]) - Convert.ToDouble(set2[i])));
                    if (difference > sumSquaredDiffs)
                    {
                        sumSquaredDiffs = difference;
                    }
                   
                }

                return sumSquaredDiffs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
    }
}
