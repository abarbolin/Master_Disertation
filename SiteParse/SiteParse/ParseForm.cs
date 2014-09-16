using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using SiteParse.Communication.SqlManager;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SiteParse
{
    public partial class ParseForm : Form
    {
        public ParseForm()
        {
            InitializeComponent();
        }


        private string _url = "http://market.yandex.ru/model-spec.xml?modelid=10719239&hid=90639&track=char";

        private void ParseForm_Load(object sender, EventArgs e)
        {
            ParseBox.ScrollBars = ScrollBars.Vertical;
        }

        private  void ParseBtn_Click(object sender, EventArgs e)
        {
           ParseHtml(_url);
        }

        private async void ParseHtml(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);
            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var parseDoc = new HtmlDocument();
            parseDoc.LoadHtml(source);

            var nodes = parseDoc.DocumentNode.Descendants();
            var tags = SqlMethods.GetTags();
            var textResult = new StringBuilder();

            foreach (var tag in tags)
            {
                var currentNodes = nodes.Where(t => t.Name == tag["name"]);
                foreach (var currentNode in currentNodes)
                {
                    textResult.AppendLine(currentNode.InnerText);
                }
            }

            ParseBox.Text = textResult.ToString();
        }
    }
}
