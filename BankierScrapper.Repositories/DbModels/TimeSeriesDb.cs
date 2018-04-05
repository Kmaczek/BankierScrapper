using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankierScrapper.Repositories.DbModels
{
    public class TimeSeriesDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public decimal Value { get; set; }

        public DateTime CaptureDate { get; set; }

        public int CompanyId { get; set; }
        public CompanyDb Company { get; set; }
    }
}
