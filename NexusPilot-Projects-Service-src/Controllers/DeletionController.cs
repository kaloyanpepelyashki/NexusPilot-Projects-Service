using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusPilot_Projects_Service_src.Services;

namespace NexusPilot_Projects_Service_src.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DeletionController : ControllerBase
    {
        private readonly ProjectService _projectService;


        public DeletionController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpDelete("project/{projectUUID}")]
        public async Task<ActionResult> DeleteProject(string projectUUID)
        {
            try
            {
                var result = await _projectService.DeleteProject(projectUUID);

                return Ok("Project deleted");

            } catch (Exception e)
            {
                Console.WriteLine($"Error deleting project: {e}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
