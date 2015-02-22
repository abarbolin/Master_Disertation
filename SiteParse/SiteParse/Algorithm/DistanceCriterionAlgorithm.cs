using System.Collections.Generic;

namespace SiteParse.Algorithm
{
    /// <summary>
    /// Implements the cluster algorithm by stopping it when the lowest distance of two clusters is greater 
    /// than a given maximum distance (distance criterion).
    /// </summary>
    internal class DistanceCriterionAlgorithm : AbstractCriterionAlgorithm
    {
        readonly float _maximumDistance;
        readonly int _minCountClustersToCreate;

        /// <summary>
        /// Creates a new instance of the DistanceCriterionAlgorithm.
        /// </summary>
        /// <param name="maximumDistance">The maximum distance to merge two clusters. 
        /// The algorithm stops if the distance of all clusters is greater than maximumDistance.</param>
        /// <param name="minCountClustersToCreate">The minimum count of clusters to create</param>
        internal DistanceCriterionAlgorithm(float maximumDistance, int minCountClustersToCreate)
        {
            _maximumDistance = maximumDistance;
            _minCountClustersToCreate = minCountClustersToCreate;
        }

        protected override bool IsFinished(ICollection<Cluster> currentClusters, ClusterPair lowestDistancePair)
        {
            return currentClusters.Count == _minCountClustersToCreate || lowestDistancePair.Distance > _maximumDistance;
        }
    }
}
