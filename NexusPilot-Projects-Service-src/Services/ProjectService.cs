﻿using Microsoft.AspNetCore.Mvc;
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
        protected SupabaseClient _supabaseClient;
        protected Client supabase;

        //Supabase Client DAO is dependency injected into the service
        public ProjectService(SupabaseClient supabaseClient) 
        {
            _supabaseClient = supabaseClient;
            supabase = _supabaseClient.SupabaseAccessor;

        }


        /*This method is in charge of creating a new project
         The method queries the database and inserts a new record with the values, passed as constructor parameter to the mmethod*/
        public async Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate)
        {
            try
            {
                //Necessary to convert the userUUID string to a Guid
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
                //Necessary to convert the userUUID string to a Guid
                var userGuid = new Guid(userUUID);

                var result = await supabase.From<Project>()
                    .Where(item => item.OwnerId == userGuid)
                    .Get();

                if(result != null)
                {
                    List<Project> returnedProjects = []; 

                     result.Models.ForEach(item =>
                    {
                        returnedProjects.Add(new Project { Id = item.Id, Title = item.Title, Description = item.Description, TumbnailImageUrl = item.TumbnailImageUrl, Closed = item.Closed, BackGroundImageUrl = item.BackGroundImageUrl });
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

        /*This method returns a single project item, based on the projectUUID provided*/
        public async Task<(bool isSuccess, Project projectItem)> GetProjectById(string projectUUID)
        {
            try
            {

                var result = await supabase.From<Project>().Where(project => project.Id == projectUUID).Get();

                if (result != null)
                {
                    if (result.Models.Count > 0)
                    {
                        Project returnedProject = new Project { Id = result.Models[0].Id, Title = result.Models[0].Title, Description = result.Models[0].Description, TumbnailImageUrl = result.Models[0].TumbnailImageUrl, BackGroundImageUrl = result.Models[0].BackGroundImageUrl, StartDate = result.Models[0].StartDate, EndDate = result.Models[0].EndDate, Closed = result.Models[0].Closed, OwnerId = result.Models[0].OwnerId };
                        return (true, returnedProject);
                    }

                }

                return (false, new Project());


            }
            catch (Exception e)
            {
                throw;
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

        /* This method checks if a user is already assigned to a project 
           If the user is already assigned to the project, this method will returnt false;
         */
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

        /*This method is in charge of closing a project
         The method queries the database and switches the closed value to true, of the targeted by uuid record */
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

        public async Task<bool>  DeleteProject(string projectUUID)
        {
            try
            {
                var projectGuid = new Guid(projectUUID);

                await supabase.From<ProjectUser>().Where(item => item.ProjectId == projectGuid).Delete();
                await supabase.From<Project>().Where(project => project.Id == projectUUID).Delete();
                return true;

            } catch(Exception e)
            {
                throw;
            }
        }
    }
}
