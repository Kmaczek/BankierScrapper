using System;
using System.Collections.Generic;
using System.IO;
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
        private string defaultFromDate = DateTime.Now.AddYears(-1).ToBankerString();
        private string defaultToDate = DateTime.Now.ToBankerString();

        public BankierService(ConfigProvider configuration, ILogger<BankierService> logger)
        {
            this._config = configuration;
            this._logger = logger;
        }

        IEnumerable<RecommendationModel> IBankierService.GetRecomendations()
        {
            var recommendations = new List<RecommendationModel>();
            var source = GetPageSource();

            if (!string.IsNullOrEmpty(source))
            {
                ParseSource(source);

            }

            return recommendations;
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
                            RecommandationFactory.CreateNew(releasedDate.Value, company, character, currentPrice, targetPrice, 
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

        private string GetPageSource()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConstructBankerUrl());
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
