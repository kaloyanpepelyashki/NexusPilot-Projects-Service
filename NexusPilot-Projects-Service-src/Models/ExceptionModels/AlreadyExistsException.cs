namespace NexusPilot_Projects_Service_src.Models.ExceptionModels
{
    public class AlreadyExistsException: Exception
    {
        public AlreadyExistsException(string message) : base(message) { }
    }
}
