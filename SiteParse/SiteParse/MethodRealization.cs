using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteParse.Distance;

namespace SiteParse
{
  public class MethodRealization:IMethods
    {
        /// <summary>
        /// Метод формального элемента
        /// </summary>
        public void ForelAlgorithm(out List<PointF> points)
        {
            //В начале нужно сформировать матрицу расстояний
            int dimension;
            float radius;
            float newRadius;
            PointF centerPoint;
            var distanceMatrix = HelpMethods.GetDistanceMatrix(out dimension, new EucledeanDistance());
            points = HelpMethods.GetPointListFromDoubleArray(distanceMatrix, dimension);
            //Находим окружность минимального радиуса, которая охватывает все точки
            Geometry.FindMinimalBoundingCircle(points, out centerPoint, out radius);
        }

        public void AglomerativeAlgorithm()
        {

        }

    }
}
