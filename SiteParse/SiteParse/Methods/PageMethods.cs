using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteParse.Communication.SqlManager;
using SiteParse.Model;

namespace SiteParse.Methods
{
    public class PageMethods
    {
        /// <summary>
        /// Получаем список страниц, их id и вектора
        /// </summary>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<PageModel> GetPageModelList(int pageCount)
        {
            // Получаем словарь со всех страниц
            var wordsFromPages = SqlMethods.GetWordsFromPages(pageCount);
            // Id веб страниц
            var pagesIds = SqlMethods.GetPagesIds(pageCount);
            int pagesReturnCount = pagesIds.Count();

            var listPageModel = new List<PageModel>();
            for (int i = 0; i < pagesReturnCount; i++)
            {
                // Инициализируем вектор текущей страницы, забивая его нулями
                var page = new PageModel { Id = Convert.ToInt32(pagesIds[i]["id"]), Url = pagesIds[i]["url"], Vector = new Dictionary<string, float>() };
                foreach (var wordFromPage in wordsFromPages)
                {
                    page.Vector[wordFromPage["word"]] = 0;
                }
                listPageModel.Add(page);
            }

            // Забиваем вектор страницы значениями частоты
            foreach (var pageModel in listPageModel)
            {
                var wordsFrequency = SqlMethods.GetWordsFrequency(pageModel.Id);
                foreach (var wordFrequency in wordsFrequency)
                {
                    pageModel.Vector[wordFrequency["word"]] = Convert.ToSingle(wordFrequency["frequency"]);
                }
            }

            return listPageModel;
        }
    }
}
