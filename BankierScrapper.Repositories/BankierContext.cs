using BankierScrapper.Model;
using BankierScrapper.Repositories.DbModels;
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

        public DbSet<RecommendationDb> Recommendations { get; set; }
        public DbSet<CompanyDb> Companies { get; set; }
        public DbSet<TimeSeriesDb> TimeSeries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecommendationDb>().HasKey(c => new
            {
                c.ReleaseDate,
                c.Institution,
                c.CompanyId
            });
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
        //}
    }
}
