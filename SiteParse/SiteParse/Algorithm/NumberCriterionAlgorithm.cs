using System.Collections.Generic;

namespace SiteParse.Algorithm
{
    /// <summary>
    /// Implements the cluster algorithm by stopping it as soon as a specified count of clusters is reached.
    /// </summary>
    internal class NumberCriterionAlgorithm : AbstractCriterionAlgorithm
    {
        readonly int _desiredClusterCount;

        /// <summary>
        /// Creates a new instance of the NumberCriterionAlgorithm. 
        /// </summary>
        /// <param name="desiredClusterCount">The count of clusters to build.</param>
        internal NumberCriterionAlgorithm(int desiredClusterCount)
        {
            _desiredClusterCount = desiredClusterCount;
        }
        
        protected override bool IsFinished(ICollection<Cluster> currentClusters, ClusterPair lowestDistancePair)
        {
            return _desiredClusterCount < 1 || _desiredClusterCount >= currentClusters.Count;
        }
    }
}
