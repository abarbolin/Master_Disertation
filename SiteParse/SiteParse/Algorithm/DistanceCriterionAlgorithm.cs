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
        /// <param name="maximumDistance">Максимальное расстояние между кластерами</param>
        /// <param name="minCountClustersToCreate">Минимальное кол-во кластеров</param>
        internal DistanceCriterionAlgorithm(float maximumDistance, int minCountClustersToCreate)
        {
            _maximumDistance = maximumDistance;
            _minCountClustersToCreate = minCountClustersToCreate;
        }
        /// <summary>
        /// Реализация критерия остановки
        /// </summary>
        /// <param name="currentClusters"></param>
        /// <param name="lowestDistancePair"></param>
        /// <returns></returns>
        protected override bool IsFinished(ICollection<Cluster> currentClusters, ClusterPair lowestDistancePair)
        {
            return currentClusters.Count == _minCountClustersToCreate || lowestDistancePair.Distance > _maximumDistance;
        }
    }
}
