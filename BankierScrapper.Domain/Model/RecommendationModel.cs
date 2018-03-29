using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Domain.Model
{
    public class RecommendationModel
    {
        public DateTime ReleaseDate { get; set; }

        public string Company { get; set; }

        public string Character { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal TargetPrice { get; set; }

        public decimal ChangePotential { get; set; }

        public decimal ReleasePrice { get; set; }

        public string Institution { get; set; }


        public string CompanyUrl { get; set; }

        public string RaportUrl { get; set; }
    }
}
