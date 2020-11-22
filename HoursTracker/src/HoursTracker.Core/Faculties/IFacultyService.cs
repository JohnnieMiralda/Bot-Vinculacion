using HoursTracker.Domain.Aggregates.Faculties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Faculties
{
    public interface IFacultyService
    {
        Task<Faculty> FindByCode(string code);

        Task<Faculty> FindById(int id);

        Task Create(Faculty faculty);

        Task<IEnumerable<Faculty>> All();

        Task Remove(int id);

        Task Update(int id, Faculty faculty);

        Task<IEnumerable<HoursFacultiesReportDto>> HoursFaculty(string code);
    }
}
