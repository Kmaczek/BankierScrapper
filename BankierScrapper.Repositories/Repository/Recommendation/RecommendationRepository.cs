using System.Collections.Generic;
using System.Linq;
using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BankierScrapper.Repositories.Repository.Recommendation
{
    public class RecommendationRepository : Repository<RecommendationDb>, IRecommendationRepository
    {
        public RecommendationRepository(BankierContext context) : base(context)
        {
        }

        public RecommendationDb AddOrUpdate(RecommendationDb recommendation)
        {
            var entity = Set.FirstOrDefault(r => r.ReleaseDate == recommendation.ReleaseDate 
                && r.Company.Name == recommendation.Company.Name 
                && r.Institution == recommendation.Institution);
            if (entity == null)
            {
                entity = recommendation;
            }
                
            Context.Entry(entity).State = entity.Id == 0 ?
                               EntityState.Added :
                               EntityState.Modified;

            Context.SaveChanges();
            return entity;
        }

        public IEnumerable<RecommendationDb> AddRange(IEnumerable<RecommendationDb> recommendations)
        {
            var tracker = Context.ChangeTracker;
            var updatedRecommendation = new List<RecommendationDb>();
            foreach (var recommendation in recommendations)
            {
                updatedRecommendation.Add(AddOrUpdate(recommendation));
            }

            return updatedRecommendation;
        }
    }
}
