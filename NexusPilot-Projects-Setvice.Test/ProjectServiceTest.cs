using FluentAssertions;
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
        [Fact]
        public void TaskService_GetInstance_ShouldReturnProjectServiceInstance()
        {
            //Arrange

            //Act
            var projectService = ProjectService.GetInstance();

            //Assert
            projectService.Should().BeOfType<ProjectService>();
        }
    }
}
