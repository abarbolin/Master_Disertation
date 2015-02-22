using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse.Distance
{
    public class JaccardDistance : IDistanceMetric
    {
        public double GetDistance(object[] set1, object[] set2)
        {
            var interSect = set1.Intersect(set2);
            var enumerable = interSect as object[] ?? interSect.ToArray();
            if (!enumerable.Any())
                return 1f / float.Epsilon;
            var unionSect = set1.Union(set2);
            return 1f / (((float)enumerable.Count() / unionSect.Count()) + float.Epsilon);
        }
    }
}
