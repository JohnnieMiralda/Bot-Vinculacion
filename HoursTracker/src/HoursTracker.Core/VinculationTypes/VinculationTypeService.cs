using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Core.VinculationTypes
{
    public class VinculationTypeService : IVinculationTypeService
    {
        private readonly IVinculationTypeRepository _vinculationTypeRepository;

        public VinculationTypeService(IVinculationTypeRepository vinculationTypeRepository)
        {
            _vinculationTypeRepository = vinculationTypeRepository;
        }

        public async Task<IEnumerable<VinculationType>> All()
        {
            return await _vinculationTypeRepository
                .Filter(vType => !vType.Disabled)
                .ToListAsync();
        }

        public async Task Create(VinculationType vType)
        {
            await _vinculationTypeRepository.Add(vType);
        }

        public async Task<VinculationType> FindByCode(string code)
        {
            return await _vinculationTypeRepository.FirstOrDefault(vT => vT.Code == code);
        }

        public async Task<VinculationType> FindById(int id)
        {
            return await _vinculationTypeRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var vType = await _vinculationTypeRepository.FindById(id);
            await _vinculationTypeRepository.Disable(vType);
        }

        public async Task Update(int id, VinculationType vType)
        {
            var existingVType = await _vinculationTypeRepository.FindById(id);

            existingVType.Code = vType.Code;
            existingVType.Type = vType.Type;

            await _vinculationTypeRepository.Update(existingVType);
        }
    }
}
