using BankierScrapper.Repositories.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Repositories.Repository.Recommendation
{
    public interface IRecommendationRepository
    {
        IEnumerable<RecommendationDb> AddRange(IEnumerable<RecommendationDb> recommendations);
    }
}
