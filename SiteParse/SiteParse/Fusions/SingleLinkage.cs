
using SiteParse.Interfaces;

namespace SiteParse.Fusions
{
    public class SingleLinkage : Fusion
    {
        private const float INITIATL_LEAST_DISTANCE = float.MaxValue;

        public override double CalculateDistance(Cluster cluster1, Cluster cluster2)
        {
            double leastDistance = INITIATL_LEAST_DISTANCE;
            foreach (Element elementCluster1 in cluster1)
            {
                foreach (Element elementCluster2 in cluster2)
                {
                    var distance = _metric.GetDistance(elementCluster1.GetDataPoints(), elementCluster2.GetDataPoints());
                    if (distance < leastDistance)
                        leastDistance = distance;
                }
            }

            return leastDistance;
        }
    }
}
