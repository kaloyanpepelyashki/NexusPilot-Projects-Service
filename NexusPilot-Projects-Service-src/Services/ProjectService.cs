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


        public async Task<bool> CreateNewProject(Guid userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate)
        {
            try
            {
                Project newProject = new Project { OwnerId = userUUID, Title = projectTitle, Description = projectDescription, TumbnailImageUrl = projectTumbnailImageUrl, BackGroundImageUrl = projectBackgroundImageUrl, StartDate = projectStartDate, EndDate = projectEndDate };
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

        public async Task<(bool isSuccess, List<Project>? projects)> GetProjectsForAccount(Guid userUUID)
        {
            try
            {
                var result = await supabase.From<Project>()
                    .Where(item => item.OwnerId == userUUID)
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
    }
}
