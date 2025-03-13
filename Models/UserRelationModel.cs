using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static UserProfileAPI.Models.UserModel;

namespace UserProfileAPI.Models
{
    public class UserRelationModel
    {
        [Key]
        public int Id { get; set; }
        
        public string? RelationType { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        [Required]
        public int RelatedUserId { get; set; }

        [ForeignKey("RelatedUserId")]
        public UserModel RelatedUser {  get; set; }

        public static readonly List<string> RelationTypes = new List<string>
        {
            "კოლეგა",
            "ნაცნობი",
            "ნათესავი",
            "სხვა"
        };

    }
}
