﻿using NexusPilot_Projects_Service_src.Models;

namespace NexusPilot_Projects_Service_src.Services.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate);
        Task<(bool isSuccess, List<Project>? projects)> GetProjectsForAccount(string userUUID);
        Task<(bool isSuccess, List<ProjectUser> usersList)> GetAllProjectUsers(string projectUUID);
        Task<bool> AddUserToProject(string projectUUID, string userUUID, string userNickName);
        Task<bool> CloseProject(string projectId);


    }
}
