using BankierScrapper.Repositories.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankierScrapper.Repositories.Repository.Company
{
    public interface ICompanyRepository
    {
        IEnumerable<CompanyDb> AddRange(IEnumerable<CompanyDb> companies);
    }
}
