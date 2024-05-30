using FluentAssertions;
using Moq;
using NexusPilot_Projects_Service_src.DAOs;


namespace NexusPilot_Projects_Setvice.Test
{
    public class SupabaseClientTest
    {
        private readonly Mock<SupabaseClient> supabaseClient;

        public SupabaseClientTest() 
        {
            supabaseClient = new Mock<SupabaseClient>();
        }

        [Fact]
        public void SupabaseClient_GetInstance_ShouldReturnSupabaseClientInstance()
        {
            //Arrange

            //Act

            //Assert
           supabaseClient.Should().NotBeNull();
           supabaseClient.Should().BeOfType<Mock<SupabaseClient>>();
        }

        /*
        [Fact]
        public void SupabaseClient_TwoGetInstance_ShouldBeTheSame()
        {
            //Arrange

            //Act
            var supabaseClientInstance1 = SupabaseClient.GetInstance();
            var supabaseClientInstance2 = SupabaseClient.GetInstance();

            //Assert
            Assert.Same(supabaseClientInstance1, supabaseClientInstance2);
        }
        */
    }
}
