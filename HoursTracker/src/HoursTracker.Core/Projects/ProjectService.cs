using System.Collections.Generic;
using System.Threading.Tasks;
using HoursTracker.Domain.Aggregates.Projects;
using HoursTracker.Domain.Aggregates.VinculationTypes;
using Microsoft.EntityFrameworkCore;

namespace HoursTracker.Core.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IVinculationTypeRepository _vinculationTypeRepository;
        public ProjectService(IProjectRepository projectRepository, IVinculationTypeRepository vinculationTypeRepository)
        {
            _projectRepository = projectRepository;
            _vinculationTypeRepository = vinculationTypeRepository;
        }
        public async Task<IEnumerable<Project>> All()
        {

            return await _projectRepository
            .Filter(project => !project.Disabled)
            .Include(x => x.VinculationType)
            .ToListAsync();

        }

        public async Task Create(CreateProjectDto project)
        {
            // await _projectRepository.Add(project);
            var vinculationType = await _vinculationTypeRepository.FindById(project.VinculationTypeId);

            var projectInfo = new Project
            {
                Code = project.Code,
                Name = project.Name,
                Budget = project.Budget,
                VinculationTypeId = vinculationType.Id

            };

            await _projectRepository.Add(projectInfo);
        }

        public async Task<Project> FindById(int id)
        {
            return await _projectRepository.FindById(id);
        }

        public async Task Remove(int id)
        {
            var project = await _projectRepository.FindById(id);
            await _projectRepository.Disable(project);
        }

        public async Task Update(int id, Project project)
        {
            var existingProject = await _projectRepository.FindById(id);

            existingProject.Code = project.Code;
            existingProject.Name = project.Name;
            existingProject.Budget = project.Budget;
            existingProject.VinculationTypeId = project.VinculationTypeId;




            await _projectRepository.Update(existingProject);
        }

    }

}
