
#region using
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SiteParse.Communication.SqlManager;
using SiteParse.Distance;
using SiteParse.Interfaces;

#endregion

namespace SiteParse
{
   public static class HelpMethods
    {
       /// <summary>
       /// Метод который возвращает массив с частотами встречаемости слов
       /// </summary>
       /// <param name="siteVect"></param>
       /// <returns></returns>
       public static  float[] GetFrequenceArray(List<Dictionary<string, string>> siteVect)
       {
           var count = siteVect.Count;
           var result = new float[count];
           for (int i = 0; i < count; i++)
           {
               result[i] = Convert.ToSingle(siteVect[i]["freq"]);
           }
           return result;
       }
       /// <summary>
       /// Ищем новый цент тяжести путём нахождения среднего арифметического
       /// </summary>
       /// <param name="points"></param>
       /// <returns></returns>
       public static PointF GetCenterOfGravity(List<PointF> points)
       {
           var count = points.Count;
           var center = new PointF{X = 0,Y=0};
           foreach (var item in points)
           {
               center.X += item.X;
               center.Y += item.Y;
           }
           center.X = center.X/count;
           center.Y = center.Y/count;
           return center;
       }
       /// <summary>
       /// Ищем новый центр тяжести - путём нахождения центра тяжести , до которого будет минимальное расстояние
       /// </summary>
       /// <param name="points"></param>
       /// <returns></returns>
       public static PointF GetCenterOfGravityWithMinimalDistance(List<PointF> points)
       {
           var minSum = float.MaxValue;
           var newCenter = new PointF(-1,-1);
           foreach (var item in points)
           {
               float sums;
               
               GetDistanceFromCenterToPoint(item, points, out sums);
               if (sums < minSum)
               {
                   minSum = sums;
                   newCenter = item;
               }
           }
           return newCenter;
       }
       /// <summary>
       /// Расттояние между двумя точками
       /// </summary>
       /// <returns></returns>
       private static float DistanceBetweenPoint(PointF first , PointF second)
       {
           return Convert.ToSingle(Math.Sqrt(Math.Pow((second.X-first.X),2) + Math.Pow((second.Y - first.Y),2)));
       }
       /// <summary>
       /// Суммарное расстояние от центра до всех точек
       /// </summary>
       /// <param name="center"></param>
       /// <param name="points"></param>
       /// <param name="sum"></param>
       public static void GetDistanceFromCenterToPoint(PointF center, List<PointF> points, out float sum)
       {
           sum = points.Sum(item => DistanceBetweenPoint(item, center));
       }
       /// <summary>
       /// Возвращаем список точек входящих в круг(точка входит в круг, если расстояние от точки до центра меньше радиуса
       /// </summary>
       public static List<PointF> GetAllPointsIncudeInComingInCircle(List<PointF> points,PointF center,float radius)
       {
           return points.Where(item => DistanceBetweenPoint(item, center) <= radius).ToList();
       }

       public static List<PointF> GetAllPoints(IDistanceMetric distance)
       {
           int dimension;
           //Получили матрицу расстояний 
           var distanceMatrix = GetDistanceMatrix(out dimension, distance);
           // Конвертируем матрицу расстояний в массив точек
             return GetPointListFromDoubleArray(distanceMatrix, dimension);
           //Точки для обхода
       }
       /// <summary>
       /// Получаем новый центр
       /// </summary>
       /// <param name="points">Все точки</param>
       /// <param name="center">Центр</param>
       /// <param name="radius">текущий радиус</param>
       /// <returns></returns>
       public static PointF GetFirstPointForNewCenter(ref List<PointF> points, PointF center, float radius)
       {
           var resultPoint = new PointF(-1,-1);

               var i = 0;
               foreach (var item in points)
               {
                   if (DoesLessDistanceRadius(item, center, radius))
                   {
                       resultPoint = points[i];
                       break;
                   }
                   i++;
               }
               if (resultPoint.X == -1 && resultPoint.Y == -1)
               {
                   throw new Exception("Точка не найдена! КОСЯК!");
               }
               return resultPoint;          
       }
       /// <summary>
       /// Возвращает true , если расстояние меньше радиуса
       /// </summary>
       /// <returns></returns>
       public static bool DoesLessDistanceRadius(PointF point, PointF center, float radius)
       {
           return DistanceBetweenPoint(point, center) <= radius;
       }
      
       /// <summary>
       /// Матрицу конвертируем в массив точек
       /// </summary>
       /// <param name="arr"></param>
       /// <param name="dimension"></param>
       /// <returns></returns>
       private static List<PointF> GetPointListFromDoubleArray(float[,] arr,int dimension)
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
       /// <summary>
       /// Конвертируем массив float  в массив object
       /// </summary>
       /// <param name="inputArr"></param>
       /// <returns></returns>
       public static object[] ConvertFloatArrToObjectArr(float[] inputArr)
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
                               distance.GetDistance(ConvertFloatArrToObjectArr(vectDouble1),
                                   ConvertFloatArrToObjectArr(vectDouble2));                      

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
