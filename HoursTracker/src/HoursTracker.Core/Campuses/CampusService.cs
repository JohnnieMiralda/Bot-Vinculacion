using HoursTracker.Domain.Aggregates.Campuses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Core.Campuses
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;

        public CampusService(ICampusRepository campusRepository)
        {
            _campusRepository = campusRepository;
        }

        public async Task<IEnumerable<Campus>> All()
        {
            return await _campusRepository
                .Filter(campus => !campus.Disabled)
                .ToListAsync();
        }

        public async Task<Campus> FindByCode(string code)
        {
            return await _campusRepository.FirstOrDefault(c => c.Code == code);
        }

        public async Task Create(Campus campus)
        {
                await _campusRepository.Add(campus);         
        }

        public async Task<Campus> FindById(int id)
        {
            return await _campusRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var campus = await _campusRepository.FindById(id);
            await _campusRepository.Disable(campus);
        }

        public async Task Update(int id,Campus campus)
        {
            var existingCampus = await _campusRepository.FindById(id);

            existingCampus.Code = campus.Code;
            existingCampus.Name = campus.Name;

            await _campusRepository.Update(existingCampus);
        }
    }
}