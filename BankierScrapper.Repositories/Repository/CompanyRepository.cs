using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Base;

namespace BankierScrapper.Repositories.Repository
{
    public class CompanyRepository : Repository<CompanyDb>
    {
        public CompanyRepository(BankierContext context) :base(context)
        {
        }
    }
}
