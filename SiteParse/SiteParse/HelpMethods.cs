
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using SiteParse.Communication.SqlManager;
using SiteParse.Distance;

namespace SiteParse
{
   public static class HelpMethods
    {
       /// <summary>
       /// Метод который возвращает массив с частотами встречаемости слов
       /// </summary>
       /// <param name="siteVect"></param>
       /// <returns></returns>
       public static  double[] GetFrequenceArray(List<Dictionary<string, string>> siteVect)
       {
           var count = siteVect.Count;
           var result = new double[count];
           for (int i = 0; i < count; i++)
           {
               result[i] = Convert.ToDouble(siteVect[i]["freq"]);
           }
           return result;
       }
       /// <summary>
       /// Расттояние между двумя точками
       /// </summary>
       /// <returns></returns>
       private static double DistanceBetweenPoint(Point first , Point second)
       {
           return Math.Sqrt((first.X - second.X) + (first.Y - second.Y));
       }
       /// <summary>
       /// Матрицу конвертируем в массив точек
       /// </summary>
       /// <param name="arr"></param>
       /// <param name="dimension"></param>
       /// <returns></returns>
       public static List<PointF> GetPointListFromDoubleArray(float[,] arr,int dimension)
       {
           var result = new List<PointF>();
           if (dimension <= 0) throw new Exception("Неправильная размерность");
           var count = dimension;
           for (var i = 0; i < count; i++)
           {
               for (var j = 0; j < count && i != j; j++)
               {
                   result.Add(new PointF(arr[i,j],arr[j,i]));
               }
           }
           return result;
       }

       public static object[] ConvertDoubleToObjectArr(double[] inputArr)
       {
           var objectArray = new object[inputArr.Length];
           inputArr.CopyTo(objectArray, 0);
           return objectArray;
       }
       /// <summary>
       /// Составление матрицы расстояний 
       /// </summary>
       public static float[,]  GetDistanceMatrix(out int dimension,IDistanceMetric distance)
       {
           var sites = SqlMethods.GetSites();
           var idSites = sites.Select(item => Convert.ToInt32(item["id"])).ToList();
           dimension = idSites.Count;
           var distanceMatrix = new float[dimension, dimension];


           for (var i = 0; i < dimension; i++)
           {
               for (var j = i + 1; j < dimension; j++)
               {
                   var vect1 = SqlMethods.GetVectorForPage(idSites[i], idSites[j]);
                   var vect2 = SqlMethods.GetVectorForPage(idSites[j], idSites[i]);
                   if (vect1.Count == vect2.Count)
                   {
                       var vectDouble1 = GetFrequenceArray(vect1); ;

                       var vectDouble2 = GetFrequenceArray(vect2);
                       distanceMatrix[i, j] =
                           (float)
                               distance.GetDistance(ConvertDoubleToObjectArr(vectDouble1),
                                   ConvertDoubleToObjectArr(vectDouble2));                      

                       distanceMatrix[j, i] = distanceMatrix[i, j];
                   }
                   else
                   {
                       throw new Exception("Хранимая процедура GetVectorForPage неправильно посчитала размерности векторов");
                   }
               }

           }
           return distanceMatrix;
       }
    }
}
