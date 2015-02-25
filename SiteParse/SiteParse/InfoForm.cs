using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiteParse.Communication.SqlManager;
using SiteParse.Methods;
using SiteParse.Properties;

namespace SiteParse
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            var sites = SqlMethods.GetSites();
            var listSites1 = new List<string>();
            var listSites2 = new List<string>();
            foreach (var site in sites)
            {
                listSites1.Add(site["id"] + " " + site["url"]);
                listSites2.Add(site["id"] + " " + site["url"]);
            }

            siteCompareLb1.DataSource = listSites1;
            siteCompareLb2.DataSource = listSites2;
        }

        

        
        private void distanceBtn_Click(object sender, EventArgs e)
        {
            var siteId1 = Convert.ToInt32(siteCompareLb1.SelectedItem.ToString().Split(' ')[0]);
            var siteId2 = Convert.ToInt32(siteCompareLb2.SelectedItem.ToString().Split(' ')[0]);

            var vect1 = SqlMethods.GetVectorForPage(siteId1, siteId2);
            var vect2 = SqlMethods.GetVectorForPage(siteId2, siteId1);
            if (vect1.Count == vect2.Count)
            {
                var cnt = vect1.Count;
                var vectDouble1 = new double[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    vectDouble1[i] = Convert.ToDouble(vect1[i]["freq"]);
                }
                var vectDouble2 = new double[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    vectDouble2[i] = Convert.ToDouble(vect2[i]["freq"]);
                }

                var distance = DistanceMethods.EuclideanDistance(vectDouble1, vectDouble2);
                distanceLbl.Text = distance.ToString();
            }
            else
            {
            }
        }
    }
}
