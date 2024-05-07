using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace NexusPilot_Projects_Service_src.Models
{
    [Table("projects")]
    public class Project: BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("tumbnailimageurl")]
        public string TumbnailImageUrl { get; set; }

        [Column("backgrountimageurl")]
        public string BackGroundImageUrl { get; set; }

        [Column("startdate")]
        public DateTime StartDate {  get; set; }

        [Column("enddate")]
        public DateTime EndDate { get; set; }

        [Column("closed")]
        public bool Closed { get; set; }

        [Column("project_owneruuid")]
        public Guid OwnerId { get; set; }

    }
}
