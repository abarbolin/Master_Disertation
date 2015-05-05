using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using LEMMATIZERLib;
using SiteParse.Properties;
using Tools;
using Tools.Communication.SqlManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        //        private static StringBuilder _textResult;
        private static List<Dictionary<string, string>> _tags;
        private static List<string> _listOfTags;
        private const int MinLength = 5;
        private static int _userId = 0;


        public ParseForm()
        {
            InitializeComponent();
        }

        //        private void LoadPanelConfig()
        //        {
        //            waitingPanel.Location = new Point(
        //            ClientSize.Width / 2 - waitingPanel.Size.Width / 2,
        //            ClientSize.Height / 2 - waitingPanel.Size.Height / 2
        //            );
        //            waitingPanel.Anchor = AnchorStyles.None;
        //            waitingPanel.BorderStyle = BorderStyle.Fixed3D;
        //        }
        /// <summary>
        /// Событие загрузки формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseForm_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            _listOfTags = new List<string>();
            errorLbl.Text = String.Empty;
            _userId = LoadUserId();
            ParseBox.ScrollBars = ScrollBars.Vertical;
        }
        /// <summary>
        /// Возвращаем идентификатор польщователя
        /// </summary>
        /// <returns></returns>
        private static int LoadUserId()
        {
            var localIPs = Dns.GetHostAddresses(Dns.GetHostName())[2];

            var userId = SqlMethods.GetUserId(localIPs.ToString(), SolutionResources.myHost);

            return userId;

        }
        /// <summary>
        /// Событие начала парсинга страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParseBtn_Click(object sender, EventArgs e)
        {
            string siteInfo;
            List<string> list;
            StringBuilder lemmatext;
            StringBuilder formatText;
            string error;
            clearControls();

            ParseWebSites(urlBox.Text, out siteInfo, out list, out lemmatext, out formatText, out error, true);

            if (error == String.Empty)
            {
                ParseBox.Text = siteInfo;
                findWordTB.Text = formatText.ToString();
                lemmaBox.Text = lemmatext.ToString();
            }
            else
            {
                errorLbl.Text = error;
            }


        }

        public void clearControls()
        {
            ParseBox.Clear();
            findWordTB.Clear();
            lemmaBox.Clear();
            errorLbl.Text = String.Empty;
        }
        /// <summary>
        /// Получаем Html-код страницы
        /// </summary>
        /// <param name="url">Ссылка на ресурс</param>
        /// <param name="error"></param>
        private HtmlDocument GetHtml(string url, out string error)
        {
            if (url == String.Empty)
            {
                error = "Вы передали пустой url";
                return null;
            }
            try
            {
                var http = new HttpClient();
                var response = http.GetByteArrayAsync(url);

                if (response.Result != null)
                {
                    var parseDoc = new HtmlDocument();

                    parseDoc.LoadHtml(GetPageSourceWithNeedEncoding(url, response));

                    error = string.Empty;
                    return parseDoc;

                }
                error = Resources.ParseForm_GetHtml_Page_is_not_available;
                return null;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }


        }
        /// <summary>
        /// Получаем текст страницы в нужной кодировке
        /// </summary>
        /// <param name="url"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public string GetPageSourceWithNeedEncoding(string url, Task<byte[]> response)
        {
            var source = Encoding.GetEncoding(GetEncoding(url))
                        .GetString(response.Result, 0, response.Result.Length - 1);
            source = WebUtility.HtmlDecode(source);

            return source;

        }

        public void ParseWebSites(string url, out string siteInfo, out  List<string> lemmaList, out StringBuilder textForLemmaBox, out StringBuilder formatText, out string error, bool needText)
        {
            //Проверяем, есть ли у нас уже такая страница
            if (!IsExistsPage(url))
            {
                //Получаем html документ нашего сайта
                var htmlDoc = GetHtml(url, out error);
                //Если документ успешно получен
                if (error == String.Empty)
                {
                    //Получаем всю важную информацию с сайта
                    siteInfo = GetHeadTags(htmlDoc);
                    //Получаем список лемм с сайта
                    lemmaList = Lemmatizer(siteInfo, out formatText);
                    //Получаем отсортированный список с применением TF
                    var sortWordlst = TermFrequencyMethod(lemmaList, url);
                    var keyValuePairs = sortWordlst as KeyValuePair<string, int>[] ?? sortWordlst.ToArray();
                    textForLemmaBox = needText ? GetTextForLemmaBox(keyValuePairs, keyValuePairs.Count()) : null;

                }
                else
                {
                    InitiAlizeOutParam(out siteInfo, out lemmaList, out textForLemmaBox, out formatText);
                }

            }
            else
            {
                error = "Такая страница уже есть в БД";
                InitiAlizeOutParam(out siteInfo, out lemmaList, out textForLemmaBox, out formatText);
            }

        }
        /// <summary>
        /// Облегченная версия, если нам не нужны параметры для текстбоксов
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ParseWebSitesWithoutOutParameters(string url)
        {
            string error;
            if (!IsExistsPage(url))
                {
                    //Получаем html документ нашего сайта
                    var htmlDoc = GetHtml(url, out error);
                    if (error == String.Empty)
                    {
                        //Получаем всю важную информацию с сайта
                        var siteInfo = GetHeadTags(htmlDoc);
                        //Получаем список лемм с сайта
                        StringBuilder formatText;
                        var lemmaList = Lemmatizer(siteInfo, out formatText);
                        //Получаем отсортированный список с применением TF
                        TermFrequencyMethod(lemmaList, url);
                    }
                    return error;
                }
            error = "Такая страница уже есть в БД";
            return error;
        }
        public void InitiAlizeOutParam(out string siteInfo, out List<string> lemmaList, out StringBuilder textForLemmaBox, out StringBuilder formatText)
        {
            lemmaList = null;
            textForLemmaBox = null;
            formatText = null;
            siteInfo = null;
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
        /// <param name="formatText"></param>
        public List<string> Lemmatizer(String text, out StringBuilder formatText)
        {
            //  ' ',
            formatText = new StringBuilder();
            var stringArr = text.Split('\n', '\r');
            stringArr = stringArr.Where(item => item != String.Empty).ToArray();

            var lemmaList = new List<string>();
            ILemmatizer lemmatizerRu = new LemmatizerRussian();
            lemmatizerRu.LoadDictionariesRegistry();

            foreach (var block in stringArr)
            {
                var blockSplit = block.Split(' ');
                var stringBlock = new StringBuilder();
                foreach (var piParadigmCollection in blockSplit.Select(str => lemmatizerRu.CreateParadigmCollectionFromForm(str, 0, 0)).Where(piParadigmCollection => piParadigmCollection.Count > 0))
                {
                    for (var j = 0; j < 1; j++)
                    {
                        object[] args = { j };
                        var paradigmCollectionType = piParadigmCollection.GetType();
                        var item = paradigmCollectionType.InvokeMember("Item", BindingFlags.GetProperty, null, piParadigmCollection, args);
                        var itemType = item.GetType();
                        var lemma = itemType.InvokeMember("Norm", BindingFlags.GetProperty, null, item, null);
                        if (lemma.ToString().Length < MinLength) continue;
                        stringBlock.Append(lemma + " ");
                        lemmaList.Add(lemma.ToString());
                    }
                }
                if (!String.IsNullOrEmpty(stringBlock.ToString()))
                {
                    formatText.Append(stringBlock + Environment.NewLine);
                }
            }

            return lemmaList;

        }
        /// <summary>
        /// Загрузка списка тэгов
        /// </summary>
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
        private static string GetHeadTags(HtmlDocument parseDoc)
        {
            var result = new StringBuilder();
            var nodes = parseDoc.DocumentNode.Descendants();
            LoadTagToList();


            foreach (var tag in _tags)
            {
                var tag1 = tag;
                var currentNodes = nodes.Where(t => t.Name == tag1["name"]);
                foreach (var currentNode in currentNodes.Where(t => t.ParentNode.Name == "body"))
                {
                    GetText(currentNode, result);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Достаём текст
        /// </summary>
        /// <param name="curNode">Текущая ветка</param>
        /// <param name="result"></param>
        private static void GetText(HtmlNode curNode, StringBuilder result)
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
                        result.AppendLine(text);
                    }
                }
                else
                {
                    GetText(childNode, result);
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

        public bool IsExistsPage(string url)
        {
            return SqlMethods.ExistsPage(url) != 0;
        }

        /// <summary>
        /// Метод поиска частоты слова
        /// </summary>
        /// <param name="lemmaList"></param>
        /// <param name="url"></param>
        private IEnumerable<KeyValuePair<string, int>> TermFrequencyMethod(IEnumerable<string> lemmaList, string url)
        {

            var withCountDict = lemmaList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var sortedDict = from entry in withCountDict orderby entry.Key ascending select entry;
            var totalCount = sortedDict.Count();


            var pageId = Convert.ToInt32(SqlMethods.AddPage(url, totalCount, _userId, SolutionResources.myHost));

            foreach (var item in sortedDict)
            {
                SqlMethods.AddWord(item.Key, pageId, string.Format("{0:N6}", (double)item.Value / totalCount));
            }

            return sortedDict;
        }
        /// <summary>
        /// Возвращает еткст с лемматизированными словами
        /// </summary>
        /// <param name="sortedDict"></param>
        /// <param name="totalCount"></param>
        public StringBuilder GetTextForLemmaBox(IEnumerable<KeyValuePair<string, int>> sortedDict, int totalCount)
        {
            var result = new StringBuilder();
            foreach (var item in sortedDict)
            {
                result.Append(item.Key + Resources.ParseForm_TermFrequencyMethod_ + string.Format("{0:N6}", (double)item.Value / totalCount) + Environment.NewLine);
            }
            return result;
        }


        /// <summary>
        /// Вызов формы InfoForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoButton_Click(object sender, EventArgs e)
        {
            var form = new InfoForm();
            form.ShowDialog();
        }
        /// <summary>
        /// Вызов формы ClusterVisualization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualBtn_Click(object sender, EventArgs e)
        {
            var form = new ClusterVisualization();
            form.ShowDialog();
        }
        /// <summary>
        /// По нажатию на кнопку начинаем парсить все сайты в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userParseBtn_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            userParseBtn.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.WorkerReportsProgress = true;
        }

        /// <summary>
        /// Достаём  сайты всех пользователей и кластеризуем
        /// </summary>
        public void ParseInfoForAllUsers()
        {
            var i = 0.0d;
            var countPages = SqlMethods.GetCountPages();
            var error = new StringBuilder();
            try
            {
                
                var userList = SqlMethods.GetAllUsers();
                foreach (var item in userList)
                {
                  
                    var getUserPages = SqlMethods.GetUserPagesAndCountLemm(Convert.ToInt32(item["id"]));

                    foreach (var itemPage in getUserPages)
                    {
                        i += (double)1/countPages*100;
                        backgroundWorker1.ReportProgress((int) i);
                            var result = ParseWebSitesWithoutOutParameters(itemPage["URL"]);
                            if (result != String.Empty)
                            {
                                error.AppendLine(result);
                            }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                error.AppendLine(ex.Message);
            }
            
        }

        /// <summary>
        /// ВЫполняем в фоне
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           
            ParseInfoForAllUsers();
            
        }
        /// <summary>
        /// Изменяем значение 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            userParseBtn.Enabled = true;
            progressBar1.Visible = false;
        }



    }
}
