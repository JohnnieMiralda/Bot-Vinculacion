using HoursTracker.Domain.Aggregates.Projects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HoursTracker.Core.Projects
{
    public interface IProjectService
    {
        Task<Project> FindById(int id);

        Task Create(CreateProjectDto project);

        Task<IEnumerable<Project>> All();

        Task Remove(int id);

        Task Update(int id, Project project);


    }
}
