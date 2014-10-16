using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using LEMMATIZERLib;
using SiteParse.Communication.SqlManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        private static StringBuilder _textResult;
        private static List<Dictionary<string, string>> _tags;
        private static List<string> _listOfTags;


        public ParseForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseForm_Load(object sender, EventArgs e)
        {
            _textResult = new StringBuilder();
            _listOfTags = new List<string>();
            ParseBox.ScrollBars = ScrollBars.Vertical;
        }
        /// <summary>
        /// Событие начала парсинга страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void ParseBtn_Click(object sender, EventArgs e)
        {
            ParseBox.Clear();
            var url = urlBox.Text;
            if (url != String.Empty)
            {
                GetHtml(url);
            }
        }
        /// <summary>
        /// Получаем Html-код страницы
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        private async void GetHtml(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            var source = Encoding.GetEncoding(GetEncoding(url)).GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var parseDoc = new HtmlDocument();
            parseDoc.LoadHtml(source);
            ParseBox.Text = GetHeadTags(parseDoc);
           
        }
        /// <summary>
        /// Получаем кодировку страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetEncoding(string url)
        {
            var req  = WebRequest.Create(url);
            var contentHeader = req.GetResponse().ContentType;
            var contentArr = contentHeader.Split(';');
            return contentArr.Length > 1 ? contentArr[1].Replace("charset=", "").Trim() : "utf-8";
        }
        /// <summary>
        /// Процедура лемматизации слова
        /// </summary>
        /// <param name="text"></param>
        public void Lemmatizer(String text)
        {
            var stringArr = text.Split(' ', '\n','\r');
            var lemmaList= new List<string>();
            ILemmatizer lemmatizerRu = new LemmatizerRussian();
            lemmatizerRu.LoadDictionariesRegistry();
            foreach (var piParadigmCollection in stringArr.Select(str => lemmatizerRu.CreateParadigmCollectionFromForm(str, 0, 0)).Where(piParadigmCollection => piParadigmCollection.Count > 0))
            {
                for (var j = 0; j < 1; j++)
                {
                    object[] args = {j};           
                    var paradigmCollectionType = piParadigmCollection.GetType();
                    var item = paradigmCollectionType.InvokeMember("Item", BindingFlags.GetProperty, null, piParadigmCollection, args);
                    var itemType = item.GetType();
                    var lemma = itemType.InvokeMember("Norm", BindingFlags.GetProperty, null, item, null);
                    lemmaList.Add(lemma.ToString());
                }
            }
            TermFrequencyMethod(lemmaList);

        }      
        /// <summary>
        /// Получаем верхние тэги страницы
        /// </summary>
        /// <param name="parseDoc">Html документ подверженный парсингу</param>
        /// <returns></returns>
        private string GetHeadTags(HtmlDocument parseDoc)
        {
            var nodes = parseDoc.DocumentNode.Descendants();
            _tags = SqlMethods.GetTags();
            foreach(var tag in _tags)
            {
                _listOfTags.Add(tag["name"]);
            }

            foreach (var tag in _tags)
            {
                var currentNodes = nodes.Where(t => t.Name == tag["name"]);

                foreach (var currentNode in currentNodes.Where(t =>t.ParentNode.Name=="body"))
                {
                    GetText(currentNode);
                }
            }
            Lemmatizer(_textResult.ToString());

            return _textResult.ToString();
        }
        /// <summary>
        /// Достаём текст
        /// </summary>
        /// <param name="curNode">Текущая ветка</param>
        private static void GetText(HtmlNode curNode)
        {
            foreach(var childNode in curNode.ChildNodes)
            {
                if (childNode.Name == "#text" && _listOfTags.Contains(childNode.ParentNode.Name))
                {
                    var text = childNode.InnerHtml;
                    if (text.Contains("<!--") || text.Contains("</")) continue;
                    text = text.Replace("\n", String.Empty).Trim();
                    text = ClearSpecialChars(text);
                    if (text != String.Empty)
                    {
                        _textResult.AppendLine(text);
                    }
                }
                else
                {
                    GetText(childNode);
                }
            }
        }
        /// <summary>
        /// Удаляем спецсимволы из вытащенного текст
        /// </summary>
        /// <param name="text">Текст</param>
        /// <returns>Текст без спецсимволов</returns>
        public static string ClearSpecialChars(string text)
        {
            var specialChars = text.Where(ch => !Char.IsLetterOrDigit(ch)).ToList();

            foreach (var specialChar in specialChars)
            {
                text.Replace(specialChar.ToString(), String.Empty);
            }

            return text;
        }
        /// <summary>
        /// По нажатию Ctrl+A выделяем  текст и копируем в буфер обмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys != Keys.Control || e.KeyCode != Keys.A) return;
            ParseBox.SelectAll();
            Clipboard.SetText(ParseBox.Text);
        }
        /// <summary>
        /// Метод поиска частоты слова
        /// </summary>
        /// <param name="lemmaList"></param>
        private void TermFrequencyMethod(List<string> lemmaList)
        {
            var withCountDict = lemmaList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var totalCount = lemmaList.Count;
            foreach (var item in withCountDict)
            {
                lemmaBox.Text += Environment.NewLine + item.Key + "; Вес = " + (double) item.Value/totalCount ;
            }
            
        }
    }
}
