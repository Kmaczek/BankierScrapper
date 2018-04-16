using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankierScrapper.Domain;
using BankierScrapper.Model;
using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Company;
using BankierScrapper.Repositories.Repository.Recommendation;
using Microsoft.AspNetCore.Mvc;

namespace BankierScrapper.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IBankierService _bankierService;
        private readonly IRecommendationRepository _recommendationRepository;
        private readonly ICompanyRepository _companyRepository;

        public ValuesController(IBankierService bankierService, IRecommendationRepository recommendationRepository, ICompanyRepository companyRepository)
        {
            this._bankierService = bankierService;
            this._recommendationRepository = recommendationRepository;
            this._companyRepository = companyRepository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<RecommendationModel> Get()
        {
            var recommendations = _bankierService.GetRecomendations();

            var companies = recommendations.Select(x => new CompanyDb()
            {
                Name = x.Company,
                Url = x.CompanyUrl
            }).Distinct();
            var updatedCompanies = _companyRepository.AddRange(companies).Distinct();

            var mappedRecommendations = recommendations.Select(r => new RecommendationDb()
            {
                Company = updatedCompanies.FirstOrDefault(c => c.Name == r.Company),
                ChangePotential = r.ChangePotential,
                Character = r.Character,
                Institution = r.Institution,
                ReleaseDate = r.ReleaseDate,
                ReleasePrice = r.ReleasePrice,
                TargetPrice = r.ReleasePrice
            });

            var recs = _recommendationRepository.AddRange(mappedRecommendations);
            return recommendations;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
