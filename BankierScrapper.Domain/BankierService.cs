using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BankierScrapper.Common;
using BankierScrapper.Domain.Model;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace BankierScrapper.Domain
{
    public class BankierService : IBankierService
    {
        private readonly ConfigProvider _config;
        private readonly ILogger<BankierService> _logger;
        private readonly IRecommandationFactory _recommandationFactory;
        private string defaultFromDate = DateTime.Now.AddYears(-1).ToBankerString();
        private string defaultToDate = DateTime.Now.ToBankerString();

        public BankierService(
            ConfigProvider configuration, 
            ILogger<BankierService> logger, 
            IRecommandationFactory recommandationFactory)
        {
            this._config = configuration;
            this._logger = logger;
            this._recommandationFactory = recommandationFactory;
            this._recommandationFactory.WillValidate = true;
        }

        public IEnumerable<RecommendationModel> GetRecomendations()
        {
            var recommendations = new List<RecommendationModel>();
            var numberOfPages = GetNoOfPages();
            var sources = GetAllPageSources(numberOfPages);

            if (sources.Count != numberOfPages)
                _logger.LogWarning($"Number of pages [{numberOfPages}] and fetched sources [{sources.Count}] differ");

            if (sources.Any())
            {
                foreach(var source in sources)
                {
                    recommendations.AddRange(ParseSource(source));
                }
            }

            return recommendations;
        }

        private int GetNoOfPages()
        {
            var firstPageSource = GetPageSource();

            if (string.IsNullOrEmpty(firstPageSource))
            {
                _logger.LogError("Source is empty, cant fetch number of pages.");
                return 0;
            }

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(firstPageSource);

                var htmlNavigationItems = doc.DocumentNode.SelectNodes("//a[contains(@class,'numeral btn')]");
                if (htmlNavigationItems == null)
                    _logger.LogError("Cant find navigation items.");

                return Convert.ToInt32(htmlNavigationItems[htmlNavigationItems.Count - 1].InnerText);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception during html parsing.");
            }
                
            return 0;
        }

        private List<RecommendationModel> ParseSource(string source)
        {
            var parsedRecommendations = new List<RecommendationModel>();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(source);

                var htmlRecommendations = doc.DocumentNode.SelectNodes("//table[@id='recommendationTable']/tbody/tr[not(contains(@class, 'adv staticRow'))]");
                if (htmlRecommendations != null)
                {
                    foreach(var recommendation in htmlRecommendations)
                    {
                        var releasedDate = recommendation.SelectSingleNode(".//td[1]")?.InnerText.ToBankerDate();
                        var company = recommendation.SelectSingleNode(".//td[2]/a")?.InnerText;
                        var companyLink = recommendation.SelectSingleNode(".//td[2]/a")?.GetAttributeValue("href", string.Empty);
                        var character = recommendation.SelectSingleNode(".//td[3]")?.InnerText;
                        var currentPrice = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[4]")?.InnerText);
                        var targetPrice = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[5]")?.InnerText);
                        var potential = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[6]")?.InnerText.Trim().Trim('%'));
                        var priceOnRelease = Convert.ToDecimal(recommendation.SelectSingleNode(".//td[7]")?.InnerText);
                        var institution = recommendation.SelectSingleNode(".//td[8]")?.InnerText;
                        var raport = recommendation.SelectSingleNode(".//td[9]/a")?.GetAttributeValue("href", string.Empty);

                        parsedRecommendations.Add(
                            _recommandationFactory.CreateNew(releasedDate.Value, company, character, currentPrice, targetPrice, 
                                potential, priceOnRelease, institution, companyLink, raport));
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

            for(int i = 1; i < numberOfPages; i++)
            {
                sources.Add(GetPageSource(i));
            }

            return sources;
        }

        private string GetPageSource(int pageNumber = 1)
        {
            if(pageNumber < 1)
            {
                _logger.LogError("Page number cannot be lower than 1");
                return string.Empty;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstructBankerUrl(pageNumber * 100));
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream = null;
                        string data = string.Empty;

                        if (response.CharacterSet == null)
                        {
                            using (readStream = new StreamReader(receiveStream)) { data = readStream.ReadToEnd(); }
                        }
                        else
                        {
                            using (readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet))) { data = readStream.ReadToEnd(); }
                        }

                        return data;
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Cant fetch page source.");
            }
            

            return string.Empty;
        }

        private string ConstructBankerUrl(int offset = 0, DateTime? fromDate = null,  DateTime? toDate = null, int limit = 100, int order = 1, int sort = 2)
        {
            return String.Format(
                _config.BankierUrlWithParameters, 
                offset,
                toDate.HasValue ? toDate.Value.ToBankerString() : defaultToDate,
                limit,
                fromDate.HasValue ? fromDate.Value.ToBankerString() : defaultFromDate,
                order, 
                sort);
        }
    }
}
