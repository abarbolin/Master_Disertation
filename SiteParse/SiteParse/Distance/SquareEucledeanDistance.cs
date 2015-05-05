using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Interfaces;

namespace SiteParse.Distance
{
    class SquareEucledeanDistance:IDistanceMetric
    {
        /// <summary>
        /// Применяется для придания большего веса более отдаленным друг от друга объектам. Это расстояние вычисляется следующим образом:
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
                    sumSquaredDiffs += Math.Pow((Convert.ToDouble(set1[i]) - Convert.ToDouble(set2[i])), 2);
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
