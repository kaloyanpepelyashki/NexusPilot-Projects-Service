using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusPilot_Projects_Service_src.Services;

namespace NexusPilot_Projects_Service_src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeletionController : ControllerBase
    {
        private readonly ProjectService _projectService;


        public DeletionController()
        {
            _projectService = ProjectService.GetInstance();
        }


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
