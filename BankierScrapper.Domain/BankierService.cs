using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using BankierScrapper.Common;
using BankierScrapper.Domain.Model;
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
            var url = ConstructBankerUrl();
            var rand = new Random(DateTime.Now.Millisecond);
            recommendations.Add(new RecommendationModel() { CurrentPrice = new Decimal(rand.NextDouble()) });

            return recommendations;
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
