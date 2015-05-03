using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HAC;
using SiteParse.Distance;
using SiteParse.Fusions;
using ZedGraph;

namespace SiteParse
{
    public partial class ClusterVisualization : Form
    {
        private readonly MethodRealization _realization = new MethodRealization();
        private int countClusters = 4;

        public Color[] ColorArr =
        {
            Color.Red,
            Color.Green,
            Color.Purple,
            Color.RoyalBlue,
            Color.Peru,
            Color.SkyBlue,
            Color.SandyBrown,
            Color.Blue
        };
       
        public ClusterVisualization()
        {
            InitializeComponent();
        }

        private void forelBtn_Click(object sender, EventArgs e)
        {
            var x = Color.Red;
            var result = _realization.ForelAlgorithm();
            PaintOnZedGraph(result);
        }

        private void aglomerativeBtn_Click(object sender, EventArgs e)
        {
            var points = HelpMethods.GetAllPoints(new JaccardDistance());
            var elements = new Element[points.Count];
            for (var i = 0; i < points.Count; i++)
            {
                elements[i] = new Element(new object[] { points[i] });
            }
            var hacStart = new HacStart(elements, new SingleLinkage(), new JaccardDistance());
            var result = hacStart.Cluster(countClusters).ToList();
            PaintOnZedGraph(result);
        }

        private void ClusterVisualization_Load(object sender, EventArgs e)
        {

        }

        private void PaintOnZedGraph(IEnumerable<Cluster> clusters)
        {
            var clusterIndex = 1;
            const string clusterName = "Cluster";
            zedGraphControl1.GraphPane.CurveList.Clear();
            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;
            foreach (var item in clusters)
            {
                var elements = item.GetElements();
                var count = elements.Count();
                var xCoordinates = new double[count];
                var yCoordinates = new double[count];
                var i = 0;
                foreach (var innerItem in elements)
                {
                    var point = innerItem.GetDataPoints().Cast<PointF>();
                    
                    foreach (var innerInnerItem in point)
                    {
                        if (innerInnerItem.X > maxX)
                        {
                            maxX = innerInnerItem.X;
                        }
                        else if (innerInnerItem.X < minX)
                        {
                            minX = innerInnerItem.X;
                        }
                        if (innerInnerItem.Y > maxY)
                        {
                            maxY = innerInnerItem.Y;
                        }
                        else if (innerInnerItem.Y < minY)
                        {
                            minY = innerInnerItem.Y;
                        }
                        xCoordinates[i] = innerInnerItem.X;
                        yCoordinates[i] = innerInnerItem.Y;
                        i++;                      
                    }
                     
                }
                zedGraphControl1.GraphPane.AddCurve(clusterName + clusterIndex, xCoordinates, yCoordinates, ColorArr[clusterIndex]);
                clusterIndex++;
            }

            // Устанавливаем интересующий нас интервал по оси X
            zedGraphControl1.GraphPane.XAxis.Scale.Min = minX;
            zedGraphControl1.GraphPane.XAxis.Scale.Max = maxX;

            // !!!
            // Устанавливаем интересующий нас интервал по оси Y
            zedGraphControl1.GraphPane.YAxis.Scale.Min = minY;
            zedGraphControl1.GraphPane.YAxis.Scale.Max = maxY;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
            // В противном случае на рисунке будет показана только часть графика, 
            // которая умещается в интервалы по осям, установленные по умолчанию
            zedGraphControl1.AxisChange();

            // Обновляем график
            zedGraphControl1.Invalidate();

        }

    }
}
