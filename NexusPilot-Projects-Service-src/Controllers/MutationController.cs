using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusPilot_Projects_Service_src.Models.ExceptionModels;
using NexusPilot_Projects_Service_src.Services;
using System.ComponentModel.DataAnnotations;

namespace NexusPilot_Projects_Service_src.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MutationController : ControllerBase
    {
        private readonly ProjectService _projectService;
        
        public MutationController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpPatch("addUserToProject")]
        //Expects to receive a json object
        public async Task<ActionResult> AddUserToProject([FromBody] AddUserObject userObject)
        {
            try
            {
                var result = await _projectService.AddUserToProject(userObject.ProjectUUID, userObject.UserUUID, userObject.UserName);

                if(result)
                {
                    return Ok("User added to project");
                }

                return StatusCode(500, "Uncertain Database transaction result");

            } catch(AlreadyExistsException e)
            {
                return StatusCode(400, "User already a member of this project");

            } catch(Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
        [Authorize]
        [HttpPatch("closeProject")]
        //Expects to receive a json object
        public async Task<ActionResult> CloseProject([FromBody] string projectUUID)
        {
            try
            {
                var result = await _projectService.CloseProject(projectUUID);

                if (result)
                {
                    return Ok("Project closed");
                }

                return StatusCode(500, "Internal Server Error");

            }
            catch (NoRecordFoundException e)
            {
                return StatusCode(404, $"Error closing project: {e}");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }


        public class AddUserObject
        {
            [Required]
            public string ProjectUUID { get; set; }

            [Required]
            public string UserUUID { get; set; }

            [Required] 
            public string UserName { get; set; }
        }
    }
}
