using System.Collections.Generic;

namespace SiteParse.Algorithm
{
    /// <summary>
    /// Критерий остановки , по желаемому количеству кластеров
    /// </summary>
    internal class NumberCriterionAlgorithm : AbstractCriterionAlgorithm
    {
        readonly int _desiredClusterCount;

        /// <summary>
        /// Инициализация, сколько должно быть кластеров
        /// </summary>
        /// <param name="desiredClusterCount">Желаемое кол-во кластеров.</param>
        internal NumberCriterionAlgorithm(int desiredClusterCount)
        {
            _desiredClusterCount = desiredClusterCount;
        }
        /// <summary>
        /// Реализация критерия остановки
        /// </summary>
        /// <param name="currentClusters">Текущее кол-во кластеров</param>
        /// <param name="lowestDistancePair">Минимальная дистанция</param>
        /// <returns></returns>
        protected override bool IsFinished(ICollection<Cluster> currentClusters, ClusterPair lowestDistancePair)
        {
            return _desiredClusterCount < 1 || _desiredClusterCount >= currentClusters.Count;
        }
    }
}
