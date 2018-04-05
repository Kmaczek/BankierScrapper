using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BankierScrapper.Repositories.Repository
{
    public class RecommendationRepository : Repository<RecommendationDb>
    {
        public RecommendationRepository(DbContext context) : base(context)
        {
        }
    }
}
