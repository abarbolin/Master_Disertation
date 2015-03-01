using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParse.Methods
{
    public class Helpers
    {
        /// <summary>
        /// Сравнение двух массивов
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }


        /// <summary>
        /// Метод поиска частоты слова
        /// </summary>
        /// <param name="lemmaList"></param>
        public static IOrderedEnumerable<KeyValuePair<string, int>> TermFrequencyMethod(IEnumerable<string> lemmaList)
        {
            var withCountDict = lemmaList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            IOrderedEnumerable<KeyValuePair<string, int>> sortedDict = from entry in withCountDict orderby entry.Key ascending select entry;
            return sortedDict;
        }

    }
}
