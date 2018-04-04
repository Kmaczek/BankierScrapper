using BankierScrapper.Model;
using System;

namespace BankierScrapper.Domain
{
    public interface IRecommandationFactory
    {
        bool WillValidate { get; set; }

        RecommendationModel CreateNew(DateTime releaseDate, string company, string character, decimal currentPrice, decimal targetPrice, decimal changePotential, decimal releasePrice, string institution, string companyUrl = null, string raportUrl = null);
    }
}