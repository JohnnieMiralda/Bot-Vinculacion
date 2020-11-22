using HoursTracker.Domain.Aggregates.VinculationTypes;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Data.Repositories.VinculationTypes
{
    public class VinculationTypesRepository : EfRepository<VinculationType>, IVinculationTypeRepository
    {
        public VinculationTypesRepository(DbContext context) : base(context)
        {

        }
    }
}
