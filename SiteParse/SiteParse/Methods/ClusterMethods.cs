using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteParse.Model;

namespace SiteParse.Methods
{
    public class ClusterMethods
    {
        /// <summary>
        /// Получаем список рандомных индексов
        /// </summary>
        /// <param name="clusterCount"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static List<int> GetRandomIndexes(int clusterCount, List<PageModel> pages)
        {
            var r = new Random();
            var indexes = new List<int>();

            while (indexes.Count < clusterCount)
            {
                var index = r.Next(0, pages.Count);

                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                }
            }

            return indexes;
        }

        /// <summary>
        /// Инициализация начального состояния кластеров, с рандомным выбором страниц как центроидов кластеров
        /// </summary>
        /// <param name="clusterCount"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static List<ClusterModel> InitializeClusters(int clusterCount, List<PageModel> pages)
        {
            var indexes = GetRandomIndexes(clusterCount, pages);

            var clusters = new List<ClusterModel>();

            foreach (var index in indexes)
            {
                var clusterModel = new ClusterModel() { PageList = new List<PageModel>() };
                clusterModel.AddPage(pages[index]);
                clusterModel.CalculateCentroidVector();
                clusters.Add(clusterModel);
            }

            return clusters;
        }
    }
}
