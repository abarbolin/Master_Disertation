using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Distance;
using SiteParse.Fusions;
using SiteParse.Interfaces;

namespace SiteParse
{
    public class MethodRealization : IMethods
    {
        /// <summary>
        /// Метод формального элемента
        /// </summary>
        public List<Cluster> ForelAlgorithm()
        {
            //Радиус
            float radius;
            //Новый радиус
            var newRadius = 0f;         
            //Текущий центр тяжести
            PointF currentCenterOfGravity;
            var i = 0;
            //Список кластеров
            var clusters = new List<Cluster>();
            // Конвертируем матрицу расстояний в массив точек
            var points = HelpMethods.GetAllPoints(new EucledeanDistance());
            //Точки для обхода
            var pointForMoving = points;
            //Находим окружность минимального радиуса, которая охватывает все точки
            Geometry.FindMinimalBoundingCircle(points, out currentCenterOfGravity, out radius);
            //Переменная , которая буджет содержать центр тяжести
            var newCenterOfGravity = new PointF();

            try
            {
                while (points.Count > 0)
                {
                    List<PointF> newPoints = null;                  
                    do
                    {
                        if (i != 0)
                        {
                            currentCenterOfGravity = newCenterOfGravity;
                            radius = newRadius;
                        }

                        //Получаем новый радиус и центр
                        newRadius = radius * 0.9f;
                        //Выбираем случайную точку
                        var randomPoint = HelpMethods.GetFirstPointForNewCenter(ref pointForMoving, currentCenterOfGravity, radius);
                        //Получаем новый список точек вход
                        if (points.Count > 0)
                        {
                            newPoints = HelpMethods.GetAllPointsIncudeInComingInCircle(points, randomPoint, newRadius);   
                        }
                        pointForMoving = newPoints;
                        newCenterOfGravity = HelpMethods.GetCenterOfGravityWithMinimalDistance(points);
//                        newCenterOfGravity = HelpMethods.GetCenterOfGravity(points);
                        i++;
                    } while (currentCenterOfGravity != newCenterOfGravity);

                    //Создаём новый кластер
                    var cluster = new Cluster(new SingleLinkage());
                    //Добавляем в него точки
                    if (newPoints != null)
                    {
                        foreach (var item in newPoints)
                        {
                            cluster.AddElement(new Element(new object[] { item }));
                        }
                        //Добавляем кластер в массив кластеров
                        clusters.Add(cluster);
                        //Удаляем точки из рассмотрения
                        foreach (var item in newPoints)
                        {
                            points.Remove(item);
                        }
                        pointForMoving = points;
                    }
                    else
                    {
                        throw new Exception("Сбой алгоритма");
                    }
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
            return clusters;
        }

        public List<Cluster> AglomerativeAlgorithm()
        {
            return  new List<Cluster>();
        }

    }
}
