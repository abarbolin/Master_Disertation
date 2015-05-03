using System.Collections.Generic;
using System.Drawing;

namespace SiteParse.Interfaces
{
    interface IMethods
    {
        /// <summary>
        /// Метод фореля
        /// </summary>
        List<Cluster> ForelAlgorithm();
        /// <summary>
        /// Агломеративный метод
        /// </summary>
        List<Cluster> AglomerativeAlgorithm();
    }
}
