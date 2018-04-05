using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankierScrapper.Repositories.DbModels
{
    public class RecommendationDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Character { get; set; }

        public decimal TargetPrice { get; set; }

        public decimal ChangePotential { get; set; }

        public decimal ReleasePrice { get; set; }

        public string Institution { get; set; }

        public int CompanyId { get; set; }
        public CompanyDb Company { get; set; }
    }
}
