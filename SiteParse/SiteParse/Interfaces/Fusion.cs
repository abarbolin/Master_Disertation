
namespace SiteParse.Interfaces
{
    public abstract class Fusion
    {
        protected IDistanceMetric _metric;
        public IDistanceMetric Metric
        {
            set { _metric = value; }
        }
        public abstract double CalculateDistance(Cluster cluster1, Cluster cluster2);
    }
}
