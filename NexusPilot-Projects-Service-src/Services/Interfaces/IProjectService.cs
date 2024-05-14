using NexusPilot_Projects_Service_src.Models;

namespace NexusPilot_Projects_Service_src.Services.Interfaces
{
    public interface IProjectService
    {
        //Creation methods
        Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate);
        //Retrieval methods
        Task<(bool isSuccess, List<Project>? projects)> GetProjectsForAccount(string userUUID);
        Task<(bool isSuccess, Project projectItem)> GetProjectById(string projectUUID);
        Task<(bool isSuccess, List<ProjectUser> usersList)> GetAllProjectUsers(string projectUUID);
        //Mutation methods
        Task<bool> AddUserToProject(string projectUUID, string userUUID, string userNickName);
        Task<bool> CloseProject(string projectId);
        //Deletion methods
        Task<bool> DeleteProject(string projectUUID);


    }
}
