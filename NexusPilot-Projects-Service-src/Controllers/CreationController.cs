using Microsoft.AspNetCore.Mvc;
using NexusPilot_Projects_Service_src.Services;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexusPilot_Projects_Service_src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreationController : ControllerBase
    {
        private ProjectService _projectService;

        public CreationController()
        {
            _projectService = ProjectService.GetInstance();
        }
        
        [HttpPost("project")]
        //Expects to receive a json object
        public async Task<ActionResult> CreateProject([FromBody] ProjectCreationObject projectObj)
        {
            try
            {
               
                var result = await _projectService.CreateNewProject(projectObj.UserUUId, projectObj.Title, projectObj.Description, projectObj.TumbnailImageUrl, projectObj.BackgroundImageUrl, projectObj.StartDate, projectObj.EndDate);

                if(result)
                {
                    return Ok("Project successfully created");
                } else
                {
                    return BadRequest("Error creating project, try again");
                }
            } catch(Exception e)
            {
                Console.WriteLine($"Error creating project: {e}");
                return StatusCode(500, "Internal Server Error");
            }
        }



        public class ProjectCreationObject
        {
            [Required]
            //It's expected the UserUUID will be received as a string and is later converted to Guid again
            public string UserUUId { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string TumbnailImageUrl { get; set; }

            [Required]
            public string BackgroundImageUrl { get; set; }

            [Required]
            public DateTime StartDate { get; set; }

            [Required]
            public DateTime EndDate { get; set; }
        }
    }
}
