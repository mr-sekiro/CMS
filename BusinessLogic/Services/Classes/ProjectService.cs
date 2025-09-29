using BusinessLogic.Data_Transfer_Object;
using BusinessLogic.Data_Transfer_Object.ProjectDtos;
using BusinessLogic.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Classes
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IEnumerable<ProjectDto> GetAllProjects()
        {
            var projects = _unitOfWork.ProjectRepo.GetAll();

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Deadline = p.Deadline,
                Status = p.Status
            });
        }

        public UpdatedProjectDto GetProjectForUpdate(int id)
        {
            var project = _unitOfWork.ProjectRepo.GetById(id);
            if (project == null) return null;

            return new UpdatedProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                Deadline = project.Deadline,
                Status = project.Status
            };
        }

        // Changed to synchronous
        public void CreateProject(CreatedProjectDto projectDto)
        {
            var project = new Project
            {
                Name = projectDto.Name,
                Description = projectDto.Description,
                StartDate = projectDto.StartDate,
                Deadline = projectDto.Deadline,
                Status = projectDto.Status
            };

            _unitOfWork.ProjectRepo.Add(project);
            _unitOfWork.SaveChanges();
        }

        public void UpdateProject(UpdatedProjectDto projectDto)
        {
            var project = _unitOfWork.ProjectRepo.GetById(projectDto.Id);
            if (project == null) return;

            project.Name = projectDto.Name;
            project.Description = projectDto.Description;
            project.StartDate = projectDto.StartDate;
            project.Deadline = projectDto.Deadline;
            project.Status = projectDto.Status;

            _unitOfWork.ProjectRepo.Update(project);
            _unitOfWork.SaveChanges();
        }

        public void DeleteProject(int id)
        {
            var project = _unitOfWork.ProjectRepo.GetById(id);
            if (project == null) return;

            _unitOfWork.ProjectRepo.Remove(project);
            _unitOfWork.SaveChanges();
        }
    }
}
