using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using SiteParse.Communication.SqlManager;
using SiteParse.Distance;
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
                var vectDouble1 = HelpMethods.GetFrequenceArray(vect1); ;

                var vectDouble2 = HelpMethods.GetFrequenceArray(vect2);
                var distance = new EucledeanDistance().GetDistance(HelpMethods.ConvertFloatArrToObjectArr(vectDouble1),
                    HelpMethods.ConvertFloatArrToObjectArr(vectDouble2));
                distanceLbl.Text = distance.ToString(CultureInfo.InvariantCulture);

            }
            else
            {
                MessageBox.Show(Resources.InfoForm_distanceBtn_Click_Формирование_векторов_произведено_с_ошибкой__Попробуйте_еще_раз);
            }
        }
    }
}
