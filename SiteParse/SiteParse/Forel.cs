using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiteParse
{
    public partial class Forel : Form
    {
        private MethodRealization _realize;
        private List<PointF> _allPoints;
        private List<PointF> _convexPoints;

        // The bounding circle.
        private PointF _circleCenter;
        private float _circleRadius = -1;

        public Forel()
        {
            InitializeComponent();
        }

        private void Forel_Load(object sender, EventArgs e)
        {    
            _realize = new MethodRealization();
            _realize.ForelAlgorithm(out _allPoints);

   

            // Get the convex hull.
            _convexPoints = Geometry.MakeConvexHull(_allPoints);

            // Get a minimal bounding circle.
            Geometry.FindMinimalBoundingCircle(_convexPoints,
                out _circleCenter, out _circleRadius);

            // Redraw.
            Invalidate();
        }

        private void Forel_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Fill all of the points.
            foreach (PointF pt in _allPoints)
            {
                e.Graphics.FillEllipse(Brushes.Cyan, pt.X - 3, pt.Y - 3, 7, 7);
            }

            // Fill the non-culled points.
            if (Geometry.GNonCulledPoints != null)
            {
                foreach (PointF pt in Geometry.GNonCulledPoints)
                {
                    e.Graphics.FillEllipse(Brushes.White, pt.X - 3, pt.Y - 3, 7, 7);
                }
            }

            // Draw all of the points.
            foreach (PointF pt in _allPoints)
            {
                e.Graphics.DrawEllipse(Pens.Black, pt.X - 3, pt.Y - 3, 7, 7);
            }

            if (_allPoints.Count >= 3)
            {
                // Draw the MinMax quadrilateral.
                e.Graphics.DrawPolygon(Pens.Red, Geometry.GMinMaxCorners);

                // Draw the culling box.
                e.Graphics.DrawRectangle(Pens.Orange, Geometry.GMinMaxBox);

                // Draw the convex hull.
                var hullPoints = new PointF[_convexPoints.Count];
                _convexPoints.CopyTo(hullPoints);
                e.Graphics.DrawPolygon(Pens.Blue, hullPoints);
            }

            // If we have a counding circle, draw it.
            if (!(_circleRadius > 0)) return;
            var rect = new RectangleF(
                _circleCenter.X - _circleRadius,
                _circleCenter.Y - _circleRadius,
                2 * _circleRadius, 2 * _circleRadius);
            e.Graphics.DrawEllipse(Pens.Green, rect);
            e.Graphics.FillEllipse(Brushes.Green,
                _circleCenter.X - 2,
                _circleCenter.Y - 2, 5, 5);
        }
    }
}
