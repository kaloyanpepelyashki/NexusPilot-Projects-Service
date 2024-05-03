namespace NexusPilot_Projects_Service_src.Services.Interfaces
{
    public interface IProjectService
    {
         Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate);
    }
}
