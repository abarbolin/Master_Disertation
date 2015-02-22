using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse
{
    public interface IDistanceMetric
    {
        double GetDistance(object[] set1, object[] set2);
    }
}
