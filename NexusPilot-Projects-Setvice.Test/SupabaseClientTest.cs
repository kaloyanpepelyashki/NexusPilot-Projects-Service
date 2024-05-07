using FluentAssertions;
using NexusPilot_Projects_Service_src.DAOs;


namespace NexusPilot_Projects_Setvice.Test
{
    public class SupabaseClientTest
    {
        [Fact]
        public void SupabaseClient_GetInstance_ShouldReturnSupabaseClientInstance()
        {
            //Arrange

            //Act
            var supabaseClient = SupabaseClient.GetInstance();

            //Assert
            supabaseClient.Should().NotBeNull();
            supabaseClient.Should().BeOfType<SupabaseClient>();
        }

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
    }
}
