namespace NexusPilot_Projects_Service_src.Models.ExceptionModels
{
    public class EmptyResultException: Exception
    {
        public EmptyResultException(string message) : base(message) { }
    }
}
