using System.Collections.Generic;
using System.Linq;
using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BankierScrapper.Repositories.Repository.Company
{
    public class CompanyRepository : Repository<CompanyDb>, ICompanyRepository
    {
        public CompanyRepository(BankierContext context) : base(context)
        {
        }

        public IQueryable<CompanyDb> GetCompaniesByName(IEnumerable<string> names)
        {
            var companies = Set.Where(x => names.Any(y => y == x.Name));
            return companies;
        }

        public CompanyDb AddOrUpdate(CompanyDb company)
        {
            var entity = Set.FirstOrDefault(x => x.Name == company.Name);
            if (entity == null)
            {
                entity = company;
            }

            Context.Entry(entity).State = entity.Id == 0 ?
                           EntityState.Added :
                           EntityState.Modified;


            Context.SaveChanges();
            return entity;
        }

        public IEnumerable<CompanyDb> AddRange(IEnumerable<CompanyDb> companies)
        {
            var updatedCompanies = new List<CompanyDb>();
            foreach (var company in companies)
            {
                updatedCompanies.Add(AddOrUpdate(company));
            }

            return updatedCompanies;
        }
    }
}
