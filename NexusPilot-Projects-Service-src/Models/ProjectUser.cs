using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace NexusPilot_Projects_Service_src.Models
{
    [Table("projectusers")]
    public class ProjectUser: BaseModel
    {
        [PrimaryKey("project_id", true)]
        public Guid ProjectId { get; set; }

        [PrimaryKey("user_unique_key", true)]
        public Guid UserId { get; set; }

        [Column("user_nickname")] 
        public string UserNickName { get; set; }

    }
}
