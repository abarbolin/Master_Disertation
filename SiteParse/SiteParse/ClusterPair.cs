namespace SiteParse
{
    internal class ClusterPair
    {
        readonly Cluster _cluster1;
        readonly Cluster _cluster2;
        readonly double _distance;

        public Cluster Cluster1
        {
            get { return _cluster1; }
        }

        public Cluster Cluster2
        {
            get { return _cluster2; }
        }

        public double Distance
        {
            get { return _distance; }
        }

        public ClusterPair(Cluster cluster1, Cluster cluster2, double distance)
        {
            _cluster1 = cluster1;
            _cluster2 = cluster2;
            _distance = distance;
        }

        internal bool HasCluster(Cluster cluster)
        {
            return _cluster1 == cluster || _cluster2 == cluster;
        }

    }
}
