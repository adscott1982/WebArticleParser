using System;
using HtmlAgilityPack;
using System.Linq;

namespace WebArticleParser
{
    public class WebPageText
    {
        private HtmlDocument htmlDoc;
        public string Text { get; private set; }

        public WebPageText(string url)
        {
            if (LoadWebPage(url))
            {
                LoadText();
            }
        }


        private void LoadText()
        {

            var title = this.htmlDoc.DocumentNode.Descendants("title");
            if (title.Count() > 0) this.Text += title.First().InnerText + Environment.NewLine + Environment.NewLine;

            // Add different node names here that you want to include
            //  || n.Name == "li"
            var paragraphs = this.htmlDoc.DocumentNode.Descendants()
                    .Where(n => n.Name == "p");

            // Exclude paragraphs where the text stripped of links is null or whitespace or special characters only
            var excludingLinkOnlyParagraphs = paragraphs.Where(p =>
            {
                var linksText = p.ChildNodes.Where(n => n.Name == "a").Select(n => n.InnerText).Where(t => t.Length > 0);
                var allText = p.InnerText;

                var textWithoutLinks = allText;

                foreach(var link in linksText)
                {
                    textWithoutLinks = textWithoutLinks.Replace(link, "");
                }

                return !string.IsNullOrWhiteSpace(textWithoutLinks);
            });

            var withoutWhiteSpace = excludingLinkOnlyParagraphs
                .Select(n => n.InnerText)
                .Where(t => !string.IsNullOrWhiteSpace(t) && t.Contains(" ") && t.Contains('.'));

            foreach (var paragraph in withoutWhiteSpace)
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

        private bool LoadWebPage(string url)
        {
            try
            {
                var web = new HtmlWeb();
                this.htmlDoc = web.Load(url);
                return true;
            }
            catch (Exception e)
            {
                this.Text = $"Failed to load page - exception message: {e.Message}";
                return false;
            }
        }
    }
}
