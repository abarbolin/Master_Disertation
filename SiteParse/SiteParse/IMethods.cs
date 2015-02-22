using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse
{
    interface IMethods
    {
        /// <summary>
        /// Метод фореля
        /// </summary>
        void ForelAlgorithm(out List<PointF> points);
        /// <summary>
        /// Агломеративный метод
        /// </summary>
        void AglomerativeAlgorithm();
    }
}
