using BankierScrapper.Repositories.DbModels;
using BankierScrapper.Repositories.Repository.Base;

namespace BankierScrapper.Repositories.Repository
{
    public class TimeSeriesRepository : Repository<TimeSeriesDb>
    {
        public TimeSeriesRepository(BankierContext context) : base(context)
        {
        }
    }
}
