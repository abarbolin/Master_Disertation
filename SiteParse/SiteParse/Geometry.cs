using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SiteParse
{
    static class Geometry
    {
        // For debugging.
        public static PointF[] GMinMaxCorners;
        public static RectangleF GMinMaxBox;
        public static PointF[] GNonCulledPoints;

       
        /// <summary>
        ///  Поиск точек около левого,правого верхник углов и аналогично для нижних
        /// </summary>
        /// <param name="points">Список всех точек</param>
        /// <param name="ul">Точка принадлежащая верхнему левому углу</param>
        /// <param name="ur">Точка принадлежащая верхнему правому углу</param>
        /// <param name="ll">Точка принадлежащая нижнему левому углу</param>
        /// <param name="lr">Точка принадлежащая нижнему правому углу</param>
        private static void GetMinMaxCorners(List<PointF> points, ref PointF ul, ref PointF ur, ref PointF ll, ref PointF lr)
        {
            // Start with the first point as the solution.
            ul = points[0];
            ur = ul;
            ll = ul;
            lr = ul;

            // Search the other points.
            foreach (var pt in points)
            {
                if (-pt.X - pt.Y > -ul.X - ul.Y) ul = pt;
                if (pt.X - pt.Y > ur.X - ur.Y) ur = pt;
                if (-pt.X + pt.Y > -ll.X + ll.Y) ll = pt;
                if (pt.X + pt.Y > lr.X + lr.Y) lr = pt;
            }

            GMinMaxCorners = new [] {ul, ur, lr, ll}; // For debugging.
        }

        // Ищем коробку, которая помещается внутри четырёхугольника
        private static RectangleF GetMinMaxBox(List<PointF> points)
        {
            // Find the MinMax quadrilateral.
            PointF ul = new Point(0, 0), ur = ul, ll = ul, lr = ul;
            GetMinMaxCorners(points, ref ul, ref ur, ref ll, ref lr);

            // Get the coordinates of a box that lies inside this quadrilateral.
            var xmin = ul.X;
            var ymin = ul.Y;

            var xmax = ur.X;
            if (ymin < ur.Y) ymin = ur.Y;

            if (xmax > lr.X) xmax = lr.X;
            var ymax = lr.Y;

            if (xmin < ll.X) xmin = ll.X;
            if (ymax > ll.Y) ymax = ll.Y;

            var result = new RectangleF(xmin, ymin, xmax - xmin, ymax - ymin);
            GMinMaxBox = result;    // For debugging.
            return result;
        }

        // Cull points out of the convex hull that lie inside the
        // trapezoid defined by the vertices with smallest and
        // largest X and Y coordinates.
        // Return the points that are not culled.
        /// <summary>
        /// Отбираем точки, которые не принадлежат выпуклой оболочки
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static List<PointF> HullCull(List<PointF> points)
        {
            // Find a culling box.
            RectangleF cullingBox = GetMinMaxBox(points);

            // Cull the points.
            var results = points.Where(pt => pt.X <= cullingBox.Left || pt.X >= cullingBox.Right || pt.Y <= cullingBox.Top || pt.Y >= cullingBox.Bottom).ToList();

            GNonCulledPoints = new PointF[results.Count];   // For debugging.
            results.CopyTo(GNonCulledPoints);              // For debugging.
            return results;
        }

        /// <summary>
        /// Список точек выпуклой поболочки
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<PointF> MakeConvexHull(List<PointF> points)
        {
            // Cull.
            points = HullCull(points);

            // Find the remaining point with the smallest Y value.
            // if (there's a tie, take the one with the smaller X value.
            PointF bestPt = points[0];
            foreach (PointF pt in points)
            {
                if ((pt.Y < bestPt.Y) ||
                   ((pt.Y == bestPt.Y) && (pt.X < bestPt.X)))
                {
                    bestPt = pt;
                }
            }

            // Move this point to the convex hull.
            var hull = new List<PointF> {bestPt};
            points.Remove(bestPt);

            // Start wrapping up the other points.
            float sweepAngle = 0;
            for (;;)
            {
                // If all of the points are on the hull, we're done.
                if (points.Count == 0) break;

                // Find the point with smallest AngleValue
                // from the last point.
                var x = hull[hull.Count - 1].X;
                var y = hull[hull.Count - 1].Y;
                bestPt = points[0];
                float bestAngle = 3600;

                // Search the rest of the points.
                foreach (var pt in points)
                {
                    var testAngle = AngleValue(x, y, pt.X, pt.Y);
                    if ((!(testAngle >= sweepAngle)) || (!(bestAngle > testAngle))) continue;
                    bestAngle = testAngle;
                    bestPt = pt;
                }

                // See if the first point is better.
                // If so, we are done.
                var firstAngle = AngleValue(x, y, hull[0].X, hull[0].Y);
                if ((firstAngle >= sweepAngle) &&
                    (bestAngle >= firstAngle))
                {
                    // The first point is better. We're done.
                    break;
                }

                // Add the best point to the convex hull.
                hull.Add(bestPt);
                points.Remove(bestPt);

                sweepAngle = bestAngle;
            }

            return hull;
        }

        // Return a number that gives the ordering of angles
        // WRST horizontal from the point (x1, y1) to (x2, y2).
        // In other words, AngleValue(x1, y1, x2, y2) is not
        // the angle, but if:
        //   Angle(x1, y1, x2, y2) > Angle(x1, y1, x2, y2)
        // then
        //   AngleValue(x1, y1, x2, y2) > AngleValue(x1, y1, x2, y2)
        // this angle is greater than the angle for another set
        // of points,) this number for
        //
        // This function is dy / (dy + dx).
        private static float AngleValue(float x1, float y1, float x2, float y2)
        {
            float dx, dy, ax, ay, t;

            dx = x2 - x1;
            ax = Math.Abs(dx);
            dy = y2 - y1;
            ay = Math.Abs(dy);
            if (ax + ay == 0)
            {
                // if (the two points are the same, return 360.
                t = 360f / 9f;
            }
            else
            {
                t = dy / (ax + ay);
            }
            if (dx < 0)
            {
                t = 2 - t;
            }
            else if (dy < 0)
            {
                t = 4 + t;
            }
            return t * 90;
        }

        /// <summary>
        /// Поиск минимальной охватывающей окружности
        /// </summary>
        /// <param name="points">Список всех точек</param>
        /// <param name="center">Центр</param>
        /// <param name="radius">Радиус</param>
        public static void FindMinimalBoundingCircle(List<PointF> points, out PointF center, out float radius)
        {
            // Find the convex hull.
            var hull = MakeConvexHull(points);

            // The best solution so far.
            var bestCenter = points[0];
            var bestRadius2 = float.MaxValue;

            // Look at pairs of hull points.
            for (var i = 0; i < hull.Count - 1; i++)
            {
                for (var j = i + 1; j < hull.Count; j++)
                {
                    // Find the circle through these two points.
                    var testCenter = new PointF(
                        (hull[i].X + hull[j].X) / 2f,
                        (hull[i].Y + hull[j].Y) / 2f);
                    var dx = testCenter.X - hull[i].X;
                    var dy = testCenter.Y - hull[i].Y;
                    var testRadius2 = dx * dx + dy * dy;

                    // See if this circle would be an improvement.
                    if (!(testRadius2 < bestRadius2)) continue;
                    // See if this circle encloses all of the points.
                    if (!CircleEnclosesPoints(testCenter, testRadius2, hull, i, j, -1)) continue;
                    // Save this solution.
                    bestCenter = testCenter;
                    bestRadius2 = testRadius2;
                } // for i
            } // for j

            // Look at triples of hull points.
            for (var i = 0; i < hull.Count - 2; i++)
            {
                for (var j = i + 1; j < hull.Count - 1; j++)
                {
                    for (var k = j + 1; k < hull.Count; k++)
                    {
                        // Find the circle through these three points.
                        PointF testCenter;
                        float testRadius2;
                        FindCircle(hull[i], hull[j], hull[k], out testCenter, out testRadius2);

                        // See if this circle would be an improvement.
                        if (!(testRadius2 < bestRadius2)) continue;
                        // See if this circle encloses all of the points.
                        if (!CircleEnclosesPoints(testCenter, testRadius2, hull, i, j, k)) continue;
                        // Save this solution.
                        bestCenter = testCenter;
                        bestRadius2 = testRadius2;
                    } // for k
                } // for i
            } // for j

            center = bestCenter;
            if (bestRadius2 == float.MaxValue)
                radius = 0;
            else
                radius = (float)Math.Sqrt(bestRadius2);
        }


        /// <summary>
        /// Метод для проверки, все ли точки охватывает окружность.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius2"></param>
        /// <param name="points"></param>
        /// <param name="skip1"></param>
        /// <param name="skip2"></param>
        /// <param name="skip3"></param>
        /// <returns></returns>
        private static bool CircleEnclosesPoints(PointF center,
            float radius2, List<PointF> points, int skip1, int skip2, int skip3)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if ((i != skip1) && (i != skip2) && (i != skip3))
                {
                    var point = points[i];
                    var dx = center.X - point.X;
                    var dy = center.Y - point.Y;
                    var testRadius2 = dx * dx + dy * dy;
                    if (testRadius2 > radius2) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Построение окружности по трём точкам
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="center"></param>
        /// <param name="radius2"></param>
        private static void FindCircle(PointF a, PointF b, PointF c, out PointF center, out float radius2)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            var x1 = (b.X + a.X) / 2;
            var y1 = (b.Y + a.Y) / 2;
            var dy1 = b.X - a.X;
            var dx1 = -(b.Y - a.Y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            var x2 = (c.X + b.X) / 2;
            var y2 = (c.Y + b.Y) / 2;
            var dy2 = c.X - b.X;
            var dx2 = -(c.Y - b.Y);

            // See where the lines intersect.
            bool linesIntersect, segmentsIntersect;
            PointF intersection, closeP1, closeP2;
            FindIntersection(
                new PointF(x1, y1),
                new PointF(x1 + dx1, y1 + dy1),
                new PointF(x2, y2),
                new PointF(x2 + dx2, y2 + dy2),
                out linesIntersect,
                out segmentsIntersect,
                out intersection,
                out closeP1,
                out closeP2);

            center = intersection;
            var dx = center.X - a.X;
            var dy = center.Y - a.Y;
            radius2 = dx * dx + dy * dy;
        }

        // Extension method to draw a RectangleF.
        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF rect)
        {
            graphics.DrawRectangle(pen, Rectangle.Round(rect));
        }

        /// <summary>
        /// Поиск точки пересечения между линиями : p1 --> p2 and p3 --> p4.
        /// </summary>
        /// <param name="p1">Начальная точка первой прямой</param>
        /// <param name="p2">Конечная точка первой прямой</param>
        /// <param name="p3">Начальная точка второй прямой</param>
        /// <param name="p4">Конечная точка второй прямой</param>
        /// <param name="linesIntersect">Признак пересечения прямых</param>
        /// <param name="segmentsIntersect"></param>
        /// <param name="intersection"></param>
        /// <param name="closeP1"></param>
        /// <param name="closeP2"></param>
        private static void FindIntersection(PointF p1, PointF p2, PointF p3, PointF p4,
            out bool linesIntersect, out bool segmentsIntersect,
            out PointF intersection, out PointF closeP1, out PointF closeP2)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            var denominator = (dy12 * dx34 - dx12 * dy34);

            float t1;
            try
            {
                t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            }
            catch
            {
                // The lines are parallel (or close enough to it).
                linesIntersect = false;
                segmentsIntersect = false;
                intersection = new PointF(float.NaN, float.NaN);
                closeP1 = new PointF(float.NaN, float.NaN);
                closeP2 = new PointF(float.NaN, float.NaN);
                return;
            }
            linesIntersect = true;

            var t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segmentsIntersect = ((t1 >= 0) && (t1 <= 1) && (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            closeP1 = new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            closeP2 = new PointF(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }
    }
}
