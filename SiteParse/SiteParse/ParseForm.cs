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
using SiteParse.Methods;
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
                    var frequencyDict = Helpers.TermFrequencyMethod(lemmaList);

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
                        var frequencyDict = Helpers.TermFrequencyMethod(lemmaList);

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


        /// <summary>
        /// Получаем матрицу расстояний между страницами
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public static float[][] GetArrayOfMeasure(List<PageModel> pages)
        {
            int pagesCount = pages.Count;
            var array = new float[pagesCount][];

            for (int i = 0; i < pagesCount; i++)
            {
                // Для каждой страницы заводим свой отдельный айди
                // Который будет идентифицировать страницу внутри матрицы расстояний
                pages[i].ArrayMessureId = i;

                array[i] = new float[pagesCount];

                for (int k = 0; k < pagesCount; k++)
                {
                    if (i == k)
                    {
                        // Для диагонали выставляем расстояние 0
                        // Страница сама с собой
                        array[i][k] = 0;
                    }
                    else
                    {
                        // Добавляем расстояние
                        array[i][k] = DistanceMethods.FindCosineSimilarity(pages[i].Vector.Values.ToArray(),
                            pages[k].Vector.Values.ToArray());
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// Получаем максимальное расстояние
        /// При cosine similarity максимальное расстояние достигается при меньшем значении
        /// 1 - полностью совпадают, 0 - вообще не совпали (находятся далеко друг от друга)
        /// </summary>
        /// <param name="arrayOfMeasure"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static MaxArrayOfMeasureModel GetMaxFromArrayMeasure(float[][] arrayOfMeasure, List<int> ids)
        {
            var measureModel = new MaxArrayOfMeasureModel();
            // Наша мера расстояния находится в диапазоне от 0 до 1
            // Ищем минимальное значение, которое будет давать нам в итоге
            // Самое большое расстояние между страницами
            float max = 2;
            foreach (var id in ids)
            {
                foreach (var i in ids)
                {
                    // Если значение меньше, значит расстояние больше и учитываем, что страница не сама на себя id==i (диагональ матрицы расстояний)
                    if (arrayOfMeasure[id][i] < max && id!=i)
                    {
                        measureModel.FirstPageId = id;
                        measureModel.SecondPageId = i;
                        max = arrayOfMeasure[id][i];
                        measureModel.Max = max;
                    }
                }
            }
            return measureModel;
        }

        /// <summary>
        /// Получаем максимальный id в списке кластеров
        /// </summary>
        /// <param name="clusters"></param>
        /// <returns></returns>
        public static int GetMaxClusterId(List<ClusterModel> clusters)
        {
            if (clusters.Count == 0)
            {
                return 0;
            }
            return clusters.Max(c => c.Id);
        }

        /// <summary>
        /// Делим один из кластеров, в котором нашли максимально удаленные друг от друга страницы
        /// на пополам и распихиваем страницы в зависимости от того к какому кластеру они ближе оказались
        /// </summary>
        /// <param name="clusters"></param>
        /// <param name="pages"></param>
        /// <param name="arrayOfMeasure"></param>
        /// <returns></returns>
        public static List<ClusterModel> SplitClusters(List<ClusterModel> clusters, List<PageModel> pages, float[][] arrayOfMeasure)
        {
            // Бежим по всем кластерам
            foreach (var cluster in clusters)
            {
                // Индексы для поиска в матрице расстояний
                var pagesIdList = new List<int>();
                foreach (var page in cluster.PageList)
                {
                    pagesIdList.Add(page.ArrayMessureId);
                }

                // Если количество страниц в кластере больше 1, то находим максимальное расстояние
                // Между страницами. Если количество 1, то делить нечего уже
                if (cluster.PageList.Count > 1)
                {
                    cluster.MaxMeasure = GetMaxFromArrayMeasure(arrayOfMeasure, pagesIdList);
                }
                else
                {
                    cluster.MaxMeasure = new MaxArrayOfMeasureModel{Max=2};
                }
            }

            // Значение меры от 0 до 1, поэтому при поиске значений вытсавляем 2,
            // Чтобы найти минимальное значение, которое будет соответствовать
            // Наибольшему расстоянию между страницами
            float tempMaxMeasure = 2;
            int splitClusterId = 0;

            foreach (var cluster in clusters)
            {
                if (cluster.MaxMeasure.Max < tempMaxMeasure)
                {
                    tempMaxMeasure = cluster.MaxMeasure.Max;
                    splitClusterId = cluster.Id;
                }
            }

            // Если в итоге мы не нашли тот кластер, который будем делить, то просто возвращаем старый набор кластеров
            if (tempMaxMeasure < 2)
            {
                // Достаем тот кластер, который будем делить
                var splitCluster = clusters.FirstOrDefault(c => c.Id == splitClusterId);
                // Идентификаторы страниц, между которыми наибольшее расстояние внутри кластера
                var firstPageMeasureId = splitCluster.MaxMeasure.FirstPageId;
                var secondPageMeasureId = splitCluster.MaxMeasure.SecondPageId;
                // Сохраняем список страниц, который затем будем раскидывать между кластерами
                var pageList = splitCluster.PageList;
                // Удаляем кластер из списка
                clusters.Remove(splitCluster);

                // Инициализируем два новых кластера
                // Закидываем в качестве центроидов те страницы, между которыми было наибольшее расстояние
                var firstCluster = new ClusterModel {Id = GetMaxClusterId(clusters) + 1};
                var firstClusterCentroid = pages.FirstOrDefault(p => p.ArrayMessureId == firstPageMeasureId);
                firstCluster.AddPage(firstClusterCentroid);
                clusters.Add(firstCluster);

                var secondCluster = new ClusterModel {Id = GetMaxClusterId(clusters) + 1};
                var secondClusterCentroid = pages.FirstOrDefault(p => p.ArrayMessureId == secondPageMeasureId);
                secondCluster.AddPage(secondClusterCentroid);
                clusters.Add(secondCluster);

                // Расскидываем страницы между двумя новыми кластерами
                foreach (var page in pageList)
                {
                    // Если это наши центроиды, то пропускаем
                    if (page.ArrayMessureId == firstPageMeasureId || page.ArrayMessureId == secondPageMeasureId)
                    {
                        continue;
                    }

                    // Чем больше мера, тем ближе страница до кластера
                    if (arrayOfMeasure[firstPageMeasureId][page.ArrayMessureId] >
                        arrayOfMeasure[secondPageMeasureId][page.ArrayMessureId])
                    {
                        firstCluster.AddPage(page);
                    }
                    else
                    {
                        secondCluster.AddPage(page);
                    }
                }
            }

            return clusters;
        }

        /// <summary>
        /// Дивизимный метод кластерного анализа
        /// </summary>
        /// <param name="pages">Список страниц</param>
        /// <returns></returns>
        public static List<ClusterModel> DivisiveMethod(List<PageModel> pages)
        {
            // Инициализируем список кластеров
            var clusters = new List<ClusterModel>();

            // Получаем матрицу расстояний
            var arrayOfMeasure = GetArrayOfMeasure(pages);

            // Создаем первый кластер, в который будут входить все страницы
            var initCluster = new ClusterModel() {Id = 1};
            foreach (var page in pages)
            {
                initCluster.PageList.Add(page);
            }
            clusters.Add(initCluster);

            // ToDo: Добавить критерий остановки
            int i = 0;
            while (i < 30)
            {
                SplitClusters(clusters, pages, arrayOfMeasure);
                i++;
            }
            var s = clusters.Where(c => c.PageList.Count > 2);
            return clusters;
        }

        private void testClusterBtn_Click(object sender, EventArgs e)
        {
            // Получаем первые n страниц
            var pages = PageMethods.GetPageModelList(50);


            var s = DivisiveMethod(pages);

            // Количество кластеров
            const int clusterCount = 3;
            // Инициализируем кластеры, выбирая рандомно clusterCount страниц, как центроиды кластеров
            var initClusters = ClusterMethods.InitializeClusters(clusterCount, pages);
            // Применяем алгоритм kmeans
            var clusters = ClusterizationMethods.KMeans(initClusters, pages);
        }





        #region htmlHelpers

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
                errorLbl.Text = Resources.ParseForm_GetHtml_Page_is_not_available;
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
                        object[] args = { j };
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

        #endregion

    }
}
