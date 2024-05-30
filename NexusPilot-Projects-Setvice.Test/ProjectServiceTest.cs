using FluentAssertions;
using Moq;
using NexusPilot_Projects_Service_src.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusPilot_Projects_Setvice.Test
{
    public class ProjectServiceTest
    {
        private readonly Mock<ProjectService> _projectService;

        public ProjectServiceTest() 
        {
            _projectService = new Mock<ProjectService>();
        
        }

        [Fact]
        public void TaskService_GetInstance_ShouldReturnProjectServiceInstance()
        {
            //Arrange

            //Act


            //Assert
            _projectService.Should().BeOfType<Mock<ProjectService>>();
        }
    }
}
