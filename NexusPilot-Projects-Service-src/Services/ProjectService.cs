using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NexusPilot_Projects_Service_src.DAOs;
using NexusPilot_Projects_Service_src.Models;
using NexusPilot_Projects_Service_src.Models.ExceptionModels;
using NexusPilot_Projects_Service_src.Services.Interfaces;
using Supabase;
using System.Text.Json;

namespace NexusPilot_Projects_Service_src.Services
{
    /*This class is in charge of handling all operations related to the project entity */ 
    public class ProjectService: IProjectService
    {
        private static ProjectService _instance;
        protected SupabaseClient supabaseClient;
        protected Client supabase;

        private ProjectService() 
        {
            supabaseClient = SupabaseClient.GetInstance();
            supabase = supabaseClient.SupabaseAccessor;

        }

        public static ProjectService GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ProjectService();
            }

            return _instance;
        }


        public async Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate)
        {
            try
            {
                Guid userGuid = new Guid(userUUID);

                Project newProject = new Project { OwnerId = userGuid, Title = projectTitle, Description = projectDescription, TumbnailImageUrl = projectTumbnailImageUrl, BackGroundImageUrl = projectBackgroundImageUrl, StartDate = projectStartDate, EndDate = projectEndDate };
                var result = await supabase.From<Project>().Insert(newProject);

                if(result != null)
                {
                    return true;
                } else
                {
                    return false;
                }


            } catch(Exception e)
            {
                throw new Exception($"Error creating project: {e}");
            }
        }

        public async Task<(bool isSuccess, List<Project>? projects)> GetProjectsForAccount(string userUUID)
        {
            try
            {
                var userGuid = new Guid(userUUID);

                var result = await supabase.From<Project>()
                    .Where(item => item.OwnerId == userGuid)
                    .Get();

                if(result != null)
                {
                    List<Project> returnedProjects = []; 

                     result.Models.ForEach(item =>
                    {
                        returnedProjects.Add(new Project { Id = item.Id, Title = item.Title, Description = item.Description, TumbnailImageUrl = item.TumbnailImageUrl, BackGroundImageUrl = item.BackGroundImageUrl });
                    });

    
                    if(returnedProjects.Count > 0)
                    {
                        return (true, returnedProjects);

                    } 
                        return (false, returnedProjects);
                    
                    
                }

                throw new EmptyResultException("Error retrieving records, no records");

            } catch(Exception e)
            {
                throw new Exception($"{e}");
            }
        }

        /* This method queries the projectusers table and gets all users, assigned to a project
            The method will return {true, List<ProjectUser>} if the query was successful and {false, List<ProjectUser>}
         */
        public async Task<(bool isSuccess, List<ProjectUser> usersList)> GetAllProjectUsers(string projectUUID)
        {
            try
            {
                var projectGuid = new Guid(projectUUID);

                var result = await supabase.From<ProjectUser>().Where(project => project.ProjectId == projectGuid).Get();

                if(result != null)
                {
                    List<ProjectUser> usersList = new List<ProjectUser>();

                    result.Models.ForEach(user =>
                    {
                        usersList.Add(new ProjectUser { ProjectId = user.ProjectId, UserId = user.UserId, UserNickName = user.UserNickName });
                    });

                    return (true, usersList);
                }

                return (false, new List<ProjectUser>());

            } catch( Exception e )
            {
                throw;
            }
        }

        protected async Task<bool> CheckProjectToUserIsValid(Guid projectGuid, Guid userGuid)
        {
            try
            {
                var result = await supabase.From<ProjectUser>().Where(item => item.ProjectId == projectGuid && item.UserId == userGuid).Get();

                if(result != null)
                {
                    if(result.Models.Count > 0)
                    {
                        return false;
                    }

                }

                return true;
            } catch(Exception e)
            {
                throw;
            }
        }

        //Test this method
        public async Task<bool> AddUserToProject(string projectUUID, string userUUID, string userNickName)
        {
            try
            {
                Guid projectGuid = new Guid(projectUUID);
                Guid userGuid = new Guid(userUUID);

               bool operationIsValid = await CheckProjectToUserIsValid(projectGuid, userGuid);

                if(operationIsValid)
                {
                    ProjectUser newProjectUser = new ProjectUser { ProjectId = projectGuid, UserId = userGuid, UserNickName = userNickName };

                    var result = await supabase.From<ProjectUser>().Insert(newProjectUser);

                    if(result.ResponseMessage.IsSuccessStatusCode)
                    {
                        return true;
                    }

                    return false;

                } else
                {
                    throw new AlreadyExistsException("User already a member of this project");
                }



            } catch( Exception e )
            {
                throw;
            }
        }

        public async Task<bool> CloseProject(string projectId)
        {
            try
            {

                var result = await supabase.From<Project>().Where(project => project.Id == projectId).Set(project => project.Closed, true).Update();

                if(result != null )
                {
                    if(result.ResponseMessage.IsSuccessStatusCode)
                    {
                        if(result.Models.Count > 0)
                        {
                            return true;
                        }

                        throw new NoRecordFoundException("Project was not found");
                    }

                    return false;
                }

                return false;

            } catch(Exception e)
            {
                throw;
            }
        }
    }
}
