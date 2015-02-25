using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using LEMMATIZERLib;
using SiteParse.Communication.SqlManager;
using SiteParse.Model;
using SiteParse.Properties;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        private static StringBuilder _textResult;
        private static List<Dictionary<string, string>> _tags;
        private static List<string> _listOfTags;
        private const int MinLength = 5;

        public ParseForm()
        {
            InitializeComponent();
        }

        private void LoadPanelConfig()
        {
            waitingPanel.Location = new Point(
            ClientSize.Width / 2 - waitingPanel.Size.Width / 2,
            ClientSize.Height / 2 - waitingPanel.Size.Height / 2
            );
            waitingPanel.Anchor = AnchorStyles.None;
            waitingPanel.BorderStyle = BorderStyle.Fixed3D;
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
            errorLbl.Text = String.Empty;
            LoadPanelConfig();
            ParseBox.ScrollBars = ScrollBars.Vertical;

            LoadTagToList();
        }
        /// <summary>
        /// Событие начала парсинга страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseBtn_Click(object sender, EventArgs e)
        {
            ParseBox.Clear();
            findWordTB.Clear();
            lemmaBox.Clear();
            _textResult.Clear();

            var url = urlBox.Text;
            if (url != String.Empty)
            {
                waitingPanel.Visible = true;
                var htmlDoc = GetHtml(url);
                if (htmlDoc != null)
                {
                    string pageText = GetTextFromPage(htmlDoc);
                    ParseBox.Text = pageText;
                    var lemmaList = Lemmatizer(pageText);
                    var frequencyDict = TermFrequencyMethod(lemmaList);

                    var totalCount = frequencyDict.Count();

                    if (SqlMethods.ExistsPage(urlBox.Text) == 0)
                    {
                        var pageId = Convert.ToInt32(SqlMethods.AddPage(urlBox.Text, totalCount));

                        foreach (var item in frequencyDict)
                        {
                            SqlMethods.AddWord(item.Key, pageId, string.Format("{0:N6}", (double)item.Value / totalCount));
                        }
                    }
                    else
                    {
                        errorLbl.Text = Resources.ParseForm_TermFrequencyMethod_Данная_страница_уже_добавлена_в_БД;
                    }

                    foreach (var item in frequencyDict)
                    {
                        lemmaBox.Text += item.Key + Resources.ParseForm_TermFrequencyMethod_ + string.Format("{0:N6}", (double)item.Value / totalCount) + Environment.NewLine;
                    }
                }
                waitingPanel.Visible = false;
            }
        }
        /// <summary>
        /// Получаем Html-код страницы
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        private HtmlDocument GetHtml(string url)
        {
            try
            {
                var http = new HttpClient();
                var response = http.GetByteArrayAsync(url);
               /* var response = await http.GetByteArrayAsync(url);*/
                if (response.Result != null)
                {
                    var source = Encoding.GetEncoding(GetEncoding(url))
                        .GetString(response.Result, 0, response.Result.Length - 1);
                    source = WebUtility.HtmlDecode(source);

                    var parseDoc = new HtmlDocument();
                    parseDoc.LoadHtml(source);
                    return parseDoc;
                }
                errorLbl.Text=Resources.ParseForm_GetHtml_Page_is_not_available;
                return null;
            }
            catch (Exception ex)
            {
                errorLbl.Text = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// Получаем кодировку страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetEncoding(string url)
        {
            var req = WebRequest.Create(url);
            var contentHeader = req.GetResponse().ContentType;
            var contentArr = contentHeader.Split(';');
            return contentArr.Length > 1 ? contentArr[1].Replace("charset=", "").Trim() : "utf-8";
        }
        /// <summary>
        /// Процедура лемматизации слова
        /// </summary>
        /// <param name="text"></param>
        public List<string> Lemmatizer(String text)
        {
            //  ' ',
            var stringArr = text.Split('\n', '\r');
            stringArr = stringArr.Where(item => item != String.Empty).ToArray();
            var lemmaList = new List<string>();
            ILemmatizer lemmatizerRu = new LemmatizerRussian();
            lemmatizerRu.LoadDictionariesRegistry();
            foreach (var block in stringArr)
            {
                var blockSplit = block.Split(' ');
                var stringBlock = new StringBuilder();
                foreach (
                    var piParadigmCollection in
                        blockSplit.Select(str => lemmatizerRu.CreateParadigmCollectionFromForm(str, 0, 0))
                            .Where(piParadigmCollection => piParadigmCollection.Count > 0))
                {
                    for (var j = 0; j < 1; j++)
                    {
                        object[] args = {j};
                        var paradigmCollectionType = piParadigmCollection.GetType();
                        var item = paradigmCollectionType.InvokeMember("Item", BindingFlags.GetProperty, null,
                            piParadigmCollection, args);
                        var itemType = item.GetType();
                        var lemma = itemType.InvokeMember("Norm", BindingFlags.GetProperty, null, item, null);
                        if (lemma.ToString().Length < MinLength) continue;
                        stringBlock.Append(lemma + " ");
                        lemmaList.Add(lemma.ToString());
                    }
                }
                if (!String.IsNullOrEmpty(stringBlock.ToString()))
                {
                    //findWordTB.AppendText(stringBlock + Environment.NewLine);
                }
            }
            return lemmaList;
        }

        private static void LoadTagToList()
        {
            _listOfTags.Clear();
            _tags = SqlMethods.GetTags();
            foreach (var tag in _tags)
            {
                _listOfTags.Add(tag["name"]);
            }
        }
        /// <summary>
        /// Получаем верхние тэги страницы
        /// </summary>
        /// <param name="parseDoc">Html документ подверженный парсингу</param>
        /// <returns></returns>
        private string GetTextFromPage(HtmlDocument parseDoc)
        {
            var nodes = parseDoc.DocumentNode.Descendants();

            foreach (var tag in _tags)
            {
                var currentNodes = nodes.Where(t => t.Name == tag["name"]);
                foreach (var currentNode in currentNodes.Where(t => t.ParentNode.Name == "body"))
                {
                    GetText(currentNode);
                }
            }
            
            return _textResult.ToString();
        }
        /// <summary>
        /// Достаём текст
        /// </summary>
        /// <param name="curNode">Текущая ветка</param>
        private static void GetText(HtmlNode curNode)
        {
            foreach (var childNode in curNode.ChildNodes)
            {
                if (childNode.Name == "#text" && _listOfTags.Contains(childNode.ParentNode.Name))
                {
                    var text = childNode.InnerHtml;
                    if (text.Contains("<!--") || text.Contains("</")) continue;
                    text = ClearSpecialChars(text);
                    text = text.Replace("\n", String.Empty).Trim();
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
            var specialChars = text.Where(ch => !Char.IsLetterOrDigit(ch) && ch != ' ').ToList();

            foreach (var specialChar in specialChars)
            {
                return text.Replace(specialChar.ToString(CultureInfo.InvariantCulture), String.Empty);
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
        private IOrderedEnumerable<KeyValuePair<string, int>> TermFrequencyMethod(IEnumerable<string> lemmaList)
        {
            var withCountDict = lemmaList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            IOrderedEnumerable<KeyValuePair<string, int>> sortedDict = from entry in withCountDict orderby entry.Key ascending select entry;
            return sortedDict;
        }
        

        private void infoButton_Click(object sender, EventArgs e)
        {
            var form = new InfoForm();
            form.ShowDialog();
        }

        private void parseHistoryBtn_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 1
            };

            // Set filter options and filter index.


            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOk = openFileDialog1.ShowDialog();

            var listUrl = new List<string>();

            // Process input if the user clicked OK.
            if (userClickedOk.ToString() == "OK")
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();

                using (var reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    string textHistory = reader.ReadToEnd();
                    var stringArr = textHistory.Split('\n', '\r');
                    var stringArrCount = stringArr.Count();
                    for (int i = 1; i < stringArrCount; i++)
                    {
                        if (stringArr[i] != "")
                        {
                            listUrl.Add(stringArr[i].Split(',')[1]);
                        }
                    }
                }
                fileStream.Close();
            }

            var urlsCount = listUrl.Count();
            for(int i = 2500; i<urlsCount; i++)
            {
                _textResult = new StringBuilder();
                string url = listUrl[i];
                countOfUrls.Text = i + "\\" + urlsCount;
                Application.DoEvents();
                if (SqlMethods.ExistsPage(url) == 0 && !url.Contains("vk.com") && !url.Contains("yandex.ru") && !url.Contains("google"))
                {
                    var htmlDoc = GetHtml(url);
                    if (htmlDoc != null)
                    {
                        string pageText = GetTextFromPage(htmlDoc);
                        var lemmaList = Lemmatizer(pageText);
                        var frequencyDict = TermFrequencyMethod(lemmaList);

                        var totalCount = frequencyDict.Count();


                        var pageId = Convert.ToInt32(SqlMethods.AddPage(url, totalCount));

                        foreach (var item in frequencyDict)
                        {
                            SqlMethods.AddWord(item.Key, pageId, string.Format("{0:N6}", (double) item.Value/totalCount));
                        }

                    }
                }
            }
        }


        public static List<PageModel> GetPageModelList(int pageCount)
        {
            var wordsFromPages = SqlMethods.GetWordsFromPages(pageCount);
            var pagesIds = SqlMethods.GetPagesIds(pageCount);
            int pagesReturnCount = pagesIds.Count();

            var listPageModel = new List<PageModel>();
            for (int i = 0; i < pagesReturnCount; i++)
            {
                var page = new PageModel { Id = Convert.ToInt32(pagesIds[i]["id"]), Vector = new Dictionary<string, float>() };
                foreach (var wordFromPage in wordsFromPages)
                {
                    page.Vector[wordFromPage["word"]] = 0;
                }
                listPageModel.Add(page);
            }

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


        public static List<int> GetRandomIndexes(int clusterCount, List<PageModel> pages)
        {
            var r = new Random();
            var indexes = new List<int>();

            while (indexes.Count < clusterCount)
            {
                var index = r.Next(0, pages.Count);

                if (!indexes.Contains(index))
                {
                    indexes.Add(index);
                }
            }

            return indexes;
        }

        public static List<ClusterModel> InitializeClusters(int clusterCount, List<PageModel> pages)
        {
            var indexes = GetRandomIndexes(clusterCount, pages);

            var clusters = new List<ClusterModel>();

            foreach (var index in indexes)
            {
                var clusterModel = new ClusterModel() {PageList = new List<PageModel>()};
                clusterModel.AddPage(pages[index]);
                clusters.Add(clusterModel);
            }

            return clusters;
        }

        private void testClusterBtn_Click(object sender, EventArgs e)
        {
            var initClusters = InitializeClusters(3, GetPageModelList(10));
        }


    }
}
