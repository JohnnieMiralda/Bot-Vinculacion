using HoursTracker.Domain.Aggregates.Bot;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.Bot
{
    public class DataBotRepository : EfRepository<DataBot>, IDataBotRepository
    {
        public DataBotRepository(DbContext context) : base(context)
        {

        }
    }
}
