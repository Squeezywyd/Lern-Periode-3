

using System;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace WebScraper
{
    public class WebScraper
    {
        public string GetHtmlContent(string url)
        {
            using (var client = new WebClient())
            {
                try
                {
                    return client.DownloadString(url);
                }
                catch (WebException ex)
                {
                    throw new WebException("Error retrieving HTML content.", ex);
                }
            }
        }

        public HtmlNodeCollection ParseHtml(string htmlContent, string xpath)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                throw new ArgumentException("HTML content cannot be null or empty.");
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            try
            {
                return htmlDocument.DocumentNode.SelectNodes(xpath);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error parsing HTML content.", ex);
            }
        }


        public string[] ExtractTextFromNodes(HtmlNodeCollection htmlNodes)
        {
            if (htmlNodes == null || htmlNodes.Count == 0)
            {
                throw new ArgumentException("HTML nodes cannot be null or empty.");
            }

            var textContent = new string[htmlNodes.Count];
            for (int i = 0; i < htmlNodes.Count; i++)
            {
                textContent[i] = htmlNodes[i].InnerText;
            }

            return textContent;
        }
        public static void DisplayAllText(String htmlContent)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            string htmlWithoutTags = Regex.Replace(htmlDocument.DocumentNode.OuterHtml, "<.*?>", String.Empty);
            Console.WriteLine(htmlWithoutTags);
        }



    }



    public class Program
    {

        public static void Main()
        {
            var scraper = new WebScraper();


            string url = "https://cartonal.ch/";
            string xpath = "//h2[@class='article-title']";

            try
            {
                string htmlContent = scraper.GetHtmlContent(url);
                HtmlNodeCollection nodes = scraper.ParseHtml(htmlContent, xpath);
                string[] titles = scraper.ExtractTextFromNodes(nodes);

                Console.WriteLine("Titles of news articles:");
                foreach (string title in titles)
                {
                    Console.WriteLine(title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}

