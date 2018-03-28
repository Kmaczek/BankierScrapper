using BankierScrapper.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Domain
{
    public interface IBankierService
    {
        IEnumerable<RecommendationModel> GetRecomendations();
    }
}
