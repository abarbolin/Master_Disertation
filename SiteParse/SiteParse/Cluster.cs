using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SiteParse.Fusions;

namespace SiteParse
{
    public class Cluster : IEnumerable
    {
        HashSet<Element> elements = new HashSet<Element>();
        Fusion fusion;

        public Cluster(Fusion fusion)
        {
            this.fusion = fusion;
        }

        internal void AddElement(Element element)
        {
            elements.Add(element);
        }

        internal void AddElements(Element[] elements)
        {
            foreach (Element e in elements)
                this.elements.Add(e);
        }

        internal Element[] GetElements()
        {
            return elements.ToArray<Element>();
        }

        internal double CalculateDistance(Cluster otherCluster)
        {
            return fusion.CalculateDistance(this, otherCluster);
        }

        #region IEnumerable Member

        public IEnumerator GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        #endregion
    }
}
