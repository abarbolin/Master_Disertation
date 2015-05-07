using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteParse.Model;

namespace SiteParse.Methods
{
    public class ClusterizationMethods
    {
        /// <summary>
        /// Алгоритм kmeans
        /// </summary>
        /// <param name="initClusters">Первоначальное состояние кластеров, с рандомно выбранными центроидами</param>
        /// <param name="pages">Страницы, которые нужно распихать по кластерам</param>
        /// <returns></returns>
        public static List<ClusterModel> KMeans(List<ClusterModel> initClusters, List<PageModel> pages)
        {
            // Флаг сходимости
            bool stopingCriteria = false;
            // Получаем количество кластеров
            var clustersCount = initClusters.Count;

            // В цикле, пока метод не сойдется
            while (stopingCriteria == false)
            {
                // Сохраняем первоначальное состояние векторов центроидов кластера
                var prevStateCentroidVectors = new Dictionary<int,float[]>();
                foreach (var clusterModel in initClusters)
                {
                    prevStateCentroidVectors[clusterModel.Id] = clusterModel.CentroidVector.Values.ToArray();
                    clusterModel.PageList = new List<PageModel>();
                }

                // Бежим по всем страницами
                foreach (var page in pages)
                {
                    // Вектор текущей страницы
                    float[] pageVector = page.Vector.Values.ToArray();
                    // Находим максимальное значение, которое будем соотвествовать самому близкому центроиду
                    float maxValue = 0;
                    // Индекс центроида
                    int index = 0;
                    for (int i = 0; i < clustersCount; i++)
                    {
                        // Вектор текущего центроида
                        float[] centroidVector = initClusters[i].CentroidVector.Values.ToArray();
                        // Рассчитываем Cosine Similarity, чем оно больше, тем конкретная страница ближе к центроиду
                        var distance = DistanceMethods.FindCosineSimilarity(pageVector, centroidVector);
                        if (distance > maxValue)
                        {
                            index = i;
                            maxValue = distance;
                        }
                    }
                    // Запихиваем страницу в кластер, к центроиду которого, она оказалась ближе
                    initClusters[index].AddPage(page);
                }

                // Рассчитываем новые центроиды кластеров
                var emptyClustersIds = new List<int>();
                foreach (var clusterModel in initClusters)
                {
                    if (clusterModel.PageList.Count == 0)
                    {
                        emptyClustersIds.Add(clusterModel.Id);
                    }
                    else
                    {
                        clusterModel.CalculateCentroidVector();    
                    }
                }

                if (emptyClustersIds.Count > 0)
                {
                    foreach (var emptyClustersId in emptyClustersIds)
                    {
                        initClusters.Remove(initClusters.FirstOrDefault(c => c.Id == emptyClustersId));
                        clustersCount--;
                    }
                    continue;
                }



                // Если какой либо из векторов центроидов не совпадает с предыдущим состоянием, то
                // Проходим алгоритм еще раз
                stopingCriteria = true;
                for (int i = 0; i < initClusters.Count; i++)
                {
                    if (!Helpers.ArraysEqual(prevStateCentroidVectors[initClusters[i].Id], initClusters[i].CentroidVector.Values.ToArray()))
                    {
                        stopingCriteria = false;
                        break;
                    }
                }
            }

            return initClusters;
        }
    }
}
