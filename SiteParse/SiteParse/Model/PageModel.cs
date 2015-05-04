using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse.Model
{
    public class PageModel
    {
        public int Id { get; set; }
        public Dictionary<string, float> Vector { get; set; }
        public int ArrayMessureId { get; set; }
        public string Url { get; set; }
    }
}
