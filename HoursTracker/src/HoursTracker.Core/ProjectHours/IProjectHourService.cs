using HoursTracker.Domain.Aggregates.ProjectHours;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.ProjectHours
{
    public interface IProjectHourService
    {
        Task<ProjectHour> FindById(int id);
        Task Create(CreateProjectHourDto projecthour);
        Task<IEnumerable<SingleProjectHourDto>> All();
        Task Remove(int id);
        Task Update(int id, UpdateProjectHourDto projecthour);
        Task Accept(int id);
    }
}
