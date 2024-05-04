using NexusPilot_Projects_Service_src.Models;

namespace NexusPilot_Projects_Service_src.Services.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateNewProject(Guid userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate);
        Task<(bool, List<Project>?)> GetProjectsForAccount(Guid userUUID);


    }
}
