using BankierScrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Domain
{
    public static class RecommandationFactory
    {
        public static RecommendationModel CreateNew(
            DateTime releaseDate, 
            string company, 
            string character, 
            decimal currentPrice, 
            decimal targetPrice, 
            decimal changePotential, 
            decimal releasePrice,
            string institution,
            string companyUrl = null,
            string raportUrl = null)
        {
            return new RecommendationModel
            {
                Company = company,
                ReleaseDate = releaseDate,
                Character = character,
                CurrentPrice = currentPrice,
                TargetPrice = targetPrice,
                ChangePotential = changePotential,
                ReleasePrice = releasePrice,
                Institution = institution,
                CompanyUrl = companyUrl,
                RaportUrl = raportUrl
            };
        }
    }
}
