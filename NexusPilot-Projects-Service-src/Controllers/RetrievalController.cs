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

        //To be changed to a get method
        [HttpPost("allProjects")]
        public async Task<ActionResult> GetAllProjectsForUser([FromBody] string userUUID)
        {
            try
            {
                //The UserUUID must be accessed through the header 
                var userGuid = new Guid(userUUID);

                var result = await _projectService.GetProjectsForAccount(userGuid);


                if (result.isSuccess)
                {
                    return Ok(result);

                } else
                {
                    return StatusCode(400, "No record found");
                }

            }
            catch (EmptyResultException e) 
            {
                return StatusCode(400, $"{e}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting projects: {e}");
                return StatusCode(500, "Error getting projects");
            }
        }
        
    }
}
