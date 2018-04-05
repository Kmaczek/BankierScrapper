using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankierScrapper.Repositories.DbModels
{
    public class CompanyDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public List<RecommendationDb> Recommendations { get; set; }
        public List<TimeSeriesDb> TimeSeries { get; set; }
    }
}
