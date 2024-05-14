using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusPilot_Projects_Service_src.Models.ExceptionModels;
using NexusPilot_Projects_Service_src.Services;

namespace NexusPilot_Projects_Service_src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetrievalController : ControllerBase
    {
        protected ProjectService _projectService;

        public RetrievalController()
        {
            _projectService = ProjectService.GetInstance();
        }

        [Authorize]
        [HttpGet("allProjectsForUser/{userUUID}")]
        public async Task<ActionResult> GetAllProjectsForUser(string userUUID)
        {
            try
            {
              
                var result = await _projectService.GetProjectsForAccount(userUUID);


                if (result.isSuccess)
                {
                    return Ok(result.projects);

                } else
                {
                    return StatusCode(400, "No record found");
                }

            }
            catch (EmptyResultException e) 
            {
                Console.WriteLine($"Error getting all projects for user: {e}");
                return StatusCode(400, $"{e}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting projects: {e}");
                return StatusCode(500, "Error getting projects");
            }
        }
        [Authorize]
        [HttpGet("project/{projectUUID}")] 
        public async Task<ActionResult> GetProjectById(string projectUUID)
        {
            try
            {
                var result = await _projectService.GetProjectById(projectUUID);

                if(result.isSuccess)
                {
                    return Ok(result.projectItem);
                }

                return StatusCode(404, "No record found");

            } catch(Exception e)
            {
                Console.WriteLine($"Error getting project: {e}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Authorize]
        [HttpGet("allUsersForProject/{projectUUID}")]
        public async Task<ActionResult> GetAllUsersForProject(string projectUUID)
        {
            try
            {
                var result = await _projectService.GetAllProjectUsers(projectUUID);

                if(result.isSuccess)
                {
                    if (result.usersList.Count > 0)
                    {
                        return Ok(result.usersList);
                    }

                    return StatusCode(404, "No records found");
                }

                return StatusCode(500, "Error getting records");

            } catch(Exception e)
            {
                Console.WriteLine($"Error getting users for project: {e}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        
    }
}
