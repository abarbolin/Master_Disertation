using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse.Model
{
    public class ClusterModel
    {
        private List<PageModel> _pageList;

        public ClusterModel()
        {
            _pageList = new List<PageModel>();
        }

        public void AddPage(PageModel page)
        {
            _pageList.Add(page);
        }

        public void CalculateCentroidVector()
        {
            CentroidVector = new Dictionary<string, float>();

            if (_pageList.Count == 1)
            {
                CentroidVector = _pageList.First().Vector;
            }
            else
            {
                var vectorKeys = _pageList.First().Vector.Keys.ToList();
                foreach (var vectorKey in vectorKeys)
                {
                    float total = 0;
                    foreach (var pageModel in _pageList)
                    {
                        total += pageModel.Vector[vectorKey];
                    }
                    CentroidVector[vectorKey] = total / _pageList.Count();
                }
            }
        }

        public List<PageModel> PageList
        {
            get { return _pageList; }
            set
            {
                _pageList = value;
            }
        }

        public Dictionary<string, float> CentroidVector { get; set; }

        public MaxArrayOfMeasureModel MaxMeasure { get; set; }

        public int Id { get; set; }
    }
}
