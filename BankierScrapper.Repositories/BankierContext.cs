using BankierScrapper.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankierScrapper.Repositories
{
    public class BankierContext : DbContext
    {
        public BankierContext(DbContextOptions<BankierContext> options)
            : base(options)
        {

        }

        public DbSet<RecommendationModel> Recommendations { get; set; }
    }
}
