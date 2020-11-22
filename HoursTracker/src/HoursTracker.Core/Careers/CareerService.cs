using HoursTracker.Domain.Aggregates.Careers;
using HoursTracker.Domain.Aggregates.Faculties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursTracker.Core.Careers
{
    public class CareerService : ICareerService
    {
        private readonly ICareerRepository _careerRepository;
        private readonly IFacultyRepository _facultyRepository;

        public CareerService(ICareerRepository careerRepository, IFacultyRepository facultyRepository)
        {
            _careerRepository = careerRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<IEnumerable<CareerDto>> All()
        {
            return await _careerRepository
                .Filter(career => !career.Disabled).Select(career => new CareerDto { Code = career.Code, Name = career.Name, Faculty = career.Faculty.Name, Id = career.Id })
                .ToListAsync();
        }
        public async Task<Career> FindByCode(string code)
        {
            return await _careerRepository.FirstOrDefault(c => c.Code == code);
        }
        public async Task Create(Career career)
        {
            var faculty = await _facultyRepository.FindById(career.FacultyId);
            var newCareer = new Career
            {
                Name = career.Name,
                Code = career.Code,
                Faculty = faculty
            };
            await _careerRepository.Add(newCareer);
        }

        public async Task<Career> FindById(int id)
        {
            return await _careerRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var career = await _careerRepository.FindById(id);
            await _careerRepository.Disable(career);
        }

        public async Task Update(int id, Career career)
        {
            var existingCareer = await _careerRepository.FindById(id);

            existingCareer.Code = career.Code;
            existingCareer.Name = career.Name;
            existingCareer.FacultyId = career.FacultyId;
            await _careerRepository.Update(existingCareer);
        }
    }
}
