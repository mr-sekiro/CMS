using BusinessLogic.Data_Transfer_Object;
using BusinessLogic.Data_Transfer_Object.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<ProjectDto> GetAllProjects();
        UpdatedProjectDto GetProjectForUpdate(int id);
        void CreateProject(CreatedProjectDto projectDto);
        void UpdateProject(UpdatedProjectDto projectDto);
        void DeleteProject(int id);
    }
}
