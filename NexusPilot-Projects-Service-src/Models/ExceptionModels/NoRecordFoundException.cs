namespace NexusPilot_Projects_Service_src.Models.ExceptionModels
{
    public class NoRecordFoundException: Exception
    {
        public NoRecordFoundException(string message) : base(message) { }
    }
}
