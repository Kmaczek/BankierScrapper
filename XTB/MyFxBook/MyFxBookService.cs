using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MyFxBook
{
    public class MyFxBookService 
    {
        private readonly IMyFxBookLogger _logger;

        public MyFxBookService(IMyFxBookLogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<MoodModel> GetRecomendations()
        {
            var recommendations = new List<MoodModel>();
            var numberOfPages = 2;// GetNoOfPages();
            var sources = GetAllPageSources(numberOfPages);

            if (sources.Count != numberOfPages)
                _logger.LogWarning($"Number of pages [{numberOfPages}] and fetched sources [{sources.Count}] differ");

            if (sources.Any())
            {
                foreach (var source in sources)
                {
                    recommendations.AddRange(ParseSource(source));
                }
            }

            return recommendations;
        }

        private List<MoodModel> ParseSource(string source)
        {
            var parsedRecommendations = new List<MoodModel>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(source);

                var htmlRecommendations = doc.DocumentNode.SelectNodes("//table[@id='recommendationTable']/tbody/tr[not(contains(@class, 'adv staticRow'))]");
                if (htmlRecommendations != null)
                {
                    foreach (var recommendation in htmlRecommendations)
                    {
                        var releasedDate = recommendation.SelectSingleNode(".//td[1]")?.InnerText;
                        var company = recommendation.SelectSingleNode(".//td[2]/a")?.InnerText;
                        var companyLink = recommendation.SelectSingleNode(".//td[2]/a")?.GetAttributeValue("href", string.Empty);
                        var character = recommendation.SelectSingleNode(".//td[3]")?.InnerText;
                        var currentPrice = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[4]")?.InnerText);
                        var targetPrice = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[5]")?.InnerText);
                        var potential = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[6]")?.InnerText.Trim().Trim('%'));
                        var priceOnRelease = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[7]")?.InnerText);
                        var institution = recommendation.SelectSingleNode(".//td[8]")?.InnerText;
                        var raport = recommendation.SelectSingleNode(".//td[9]/a")?.GetAttributeValue("href", string.Empty);

                        parsedRecommendations.Add(new MoodModel()
                        {
                            //BuyPercent
                        });
                    }
                }
                else
                {
                    _logger.LogError("Couldnt find recommendations.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cant parse page source");
            }

            return parsedRecommendations;
        }

        private List<string> GetAllPageSources(int numberOfPages)
        {
            var sources = new List<string>();

            for (int i = 1; i < numberOfPages; i++)
            {
                sources.Add(GetPageSource(i));
            }

            return sources;
        }

        private string GetPageSource(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                _logger.LogError("Page number cannot be lower than 1");
                return string.Empty;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstructUrl());
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        string data = string.Empty;

                        // default utf-8 is good
                        using (readStream = new StreamReader(receiveStream)) { data = readStream.ReadToEnd(); }
                        // charset sent in response is not correct for html
                        //    using (readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet))) { data = readStream.ReadToEnd(); }

                        return data;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cant fetch page source.");
            }


            return string.Empty;
        }

        private string ConstructUrl()
        {
            return String.Format(@"https://www.myfxbook.com/community/outlook");
        }
    }
}
