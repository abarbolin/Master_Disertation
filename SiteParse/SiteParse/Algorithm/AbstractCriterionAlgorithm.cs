using System.Collections.Generic;
using System.Linq;
using SiteParse.Fusions;
using SiteParse.Interfaces;

namespace SiteParse.Algorithm
{
    /// <summary>
    /// В этом абстрактном классе реализуется основная часть алгоритма, в дополнительных классах- различные условия остановки
    /// </summary>
    internal abstract class AbstractCriterionAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elements">Элементы</param>
        /// <param name="fusion"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        internal protected Cluster[] Cluster(List<Element> elements, Fusion fusion, IDistanceMetric metric)
        {
            var clusters = new HashSet<Cluster>();
            var pairs = new ClusterPairs();

            // 1. В начале каждый элемент это кластер
            foreach (var el in elements)
            {
                var cl = new Cluster(fusion);
                cl.AddElement(el);
                clusters.Add(cl);
            }

            // 2. a) Ищем расстояние от одного кластера до всех остальных и составляем ClusterPair
            foreach (var cl1 in clusters)
            {
                foreach (var cl2 in clusters)
                {
                    if (cl1 == cl2)
                        continue;
                    var pair = new ClusterPair(cl1, cl2, cl1.CalculateDistance(cl2));
                    pairs.AddPair(pair);
                }
            }

            
            // 2. b) Ищем пару кластеров с минимальным расстоянием
            var lowestDistancePair = pairs.LowestDistancePair;
          

            // 3. Merge clusters to new clusters and recalculate distances in a loop until there are only countCluster clusters
            while (!IsFinished(clusters, lowestDistancePair))
            {
                // a) Merge: Create a new cluster and add the elements of the two old clusters    
                // Возвращаем наименьшую пару
                lowestDistancePair = pairs.LowestDistancePair;
                //Создаём новый кластер
                var newCluster = new Cluster(fusion); 
                //Добавляем в него кластеры, между которыми минимальное расстояние
                newCluster.AddElements(lowestDistancePair.Cluster1.GetElements());
                newCluster.AddElements(lowestDistancePair.Cluster2.GetElements());
                // b)Удаляем их из общего списка кластеров
                clusters.Remove(lowestDistancePair.Cluster1);
                clusters.Remove(lowestDistancePair.Cluster2);
                // c) Удаляем эти кластеры из Pairs
                pairs.RemovePairsByOldClusters(lowestDistancePair.Cluster1, lowestDistancePair.Cluster2);

                // d)Считаем расстояние от найденной минимальной пары, до всех оставшихся кластеров
                foreach (Cluster cluster in clusters)
                {
                    var pair = new ClusterPair(cluster, newCluster, cluster.CalculateDistance(newCluster));
                    pairs.AddPair(pair);
                }
                // e) Добавляем новый кластер
                clusters.Add(newCluster);
            }

            return clusters.ToArray<Cluster>();
        }

        /// <summary>
        /// Критерий остановки
        /// </summary>
        /// <param name="currentClusters"></param>
        /// <param name="lowestDistancePair"></param>
        /// <returns></returns>
        protected abstract bool IsFinished(ICollection<Cluster> currentClusters, ClusterPair lowestDistancePair);
    }
}
