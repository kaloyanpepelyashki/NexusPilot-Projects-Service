using NexusPilot_Projects_Service_src.DAOs;
using NexusPilot_Projects_Service_src.Models;
using NexusPilot_Projects_Service_src.Services.Interfaces;
using Supabase;

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
            if(_instance != null)
            {
                _instance = new ProjectService();
            }

            return _instance;
        }


        public async Task<bool> CreateNewProject(string userUUID, string projectTitle, string projectDescription, string projectTumbnailImageUrl, string projectBackgroundImageUrl, DateTime projectStartDate, DateTime projectEndDate)
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
    }
}
