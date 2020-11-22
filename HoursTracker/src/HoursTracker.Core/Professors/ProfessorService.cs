using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.Professors;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Core.Professors
{
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessorService(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<SingleProfessorDto> FindById(int id)
        {
            var data = _professorRepository.All().First(x => x.Id == id);

            return new SingleProfessorDto
            {
                Code = data.Code,
                Id = data.Id,
                SecondName = data.SecondName,
                FirstLastName = data.FirstLastName,
                FirstName = data.FirstName,
                SecondLastName = data.SecondLastName
            };
        }
        public async Task<Professor> FindByCode(string code)
        {
            return await _professorRepository.FirstOrDefault(c => c.Code == code);
        }
        public async Task Create(Professor professor)
        {
            await _professorRepository.Add(professor);
        }

        public async Task<IEnumerable<SingleProfessorDto>> All()
        {
            return await _professorRepository
                .Filter(professor => !professor.Disabled)
                .Select(x => new SingleProfessorDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    FirstLastName = x.FirstLastName,
                    FirstName = x.FirstName,
                    SecondLastName = x.SecondLastName,
                    SecondName = x.SecondName
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Remove(int id)
        {
            var professor = await _professorRepository.FindById(id);
            await _professorRepository.Disable(professor);
        }

        public async Task Update(int id, Professor professor)
        {
            var existingProfessor = await _professorRepository.FindById(id);

            existingProfessor.Code = professor.Code;
            existingProfessor.FirstName = professor.FirstName;
            existingProfessor.SecondName = professor.SecondName;
            existingProfessor.FirstLastName = professor.FirstLastName;
            existingProfessor.SecondLastName = professor.SecondLastName;

            await _professorRepository.Update(existingProfessor);
        }
    }
}