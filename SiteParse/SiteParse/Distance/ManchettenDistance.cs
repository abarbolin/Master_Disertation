using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Interfaces;

namespace SiteParse.Distance
{
    class ManchettenDistance:IDistanceMetric
    {
        /// <summary>
        /// . В большинстве случаев эта мера расстояния приводит к таким же результатам, как и для обычного расстояния Евклида. 
        /// Однако для этой меры влияние отдельных больших разностей (выбросов) уменьшается (т.к. они не возводятся в квадрат)
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
                    sumSquaredDiffs += Math.Abs((Convert.ToDouble(set1[i]) - Convert.ToDouble(set2[i])));
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
    {
    }
}
