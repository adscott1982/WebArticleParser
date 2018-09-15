using System;
using HtmlAgilityPack;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebArticleParser
{
    public class WebPageText
    {
        private HtmlDocument htmlDoc;
        public string Text { get; private set; }

        public WebPageText(string url)
        {
            LoadWebPage(url);
            LoadText();
        }

        private void LoadText()
        {
            var title = this.htmlDoc.DocumentNode.Descendants("title");
            if (title.Count() > 0) this.Text += title.First().InnerText + Environment.NewLine + Environment.NewLine;

            // Add different node names here that you want to include
            //  || n.Name == "li"
            var paragraphs =
                this.htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.Name == "p")
                    .Select(n => n.InnerText)
                    .Where(t => !string.IsNullOrWhiteSpace(t) && t.Contains(" "));
                                                   
            foreach (var paragraph in paragraphs)
            {
                var text = HtmlEntity.DeEntitize(paragraph);
                text = this.TrimExcessNewLines(text);
                text += Environment.NewLine + Environment.NewLine;
                this.Text += text;
            }
        }

        private string TrimExcessNewLines(string s)
        {
            while (s.StartsWith("\n") || s.EndsWith("\n") || s.Contains("\n\n"))
            {
                s = s.Trim('\n');
                s = s.Replace("\n\n", "\n");
            }

            return s;
        }

        private void LoadWebPage(string url)
        {
            var web = new HtmlWeb();
            this.htmlDoc = web.Load(url);
        }
    }
}
