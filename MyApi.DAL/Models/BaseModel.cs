using System.ComponentModel.DataAnnotations.Schema;

namespace MyApi.DAL.Models
{
    public class BaseModel
    {
        public int Id { get; set; } 
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public String? CreatedBy {get;set;}
        public String? UpdatedBy {get;set;}
        public DateTime UpdatedAt {get;set;}
        [ForeignKey("CreatedBy")]
        public ApplicationUser User {get;set;}

        

    }
}
