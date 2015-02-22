using System.Collections.Generic;

namespace SiteParse
{
    internal class ClusterPairs
    {
        readonly HashSet<ClusterPair> _pairs = new HashSet<ClusterPair>();

        internal ClusterPair LowestDistancePair
        {
            get 
            {
                ClusterPair lowestDistancePair = null;
                foreach (ClusterPair pair in _pairs)
                    if (lowestDistancePair == null || lowestDistancePair.Distance > pair.Distance)
                        lowestDistancePair = pair;
                return lowestDistancePair;
            }
        }

        internal int Count
        {
            get { return _pairs.Count; }
        }

        internal void AddPair(ClusterPair pair)
        {
            _pairs.Add(pair);
        }

        internal void RemovePairsByOldClusters(Cluster cluster1, Cluster cluster2)
        {
            var toRemove = new List<ClusterPair>();
            foreach(ClusterPair pair in _pairs)
            {
                if (pair.HasCluster(cluster1) || pair.HasCluster(cluster2))
                {
                    toRemove.Add(pair);
                }
            }
            foreach (ClusterPair pair in toRemove)
            {
                _pairs.Remove(pair);
            }
        }
    }
}
