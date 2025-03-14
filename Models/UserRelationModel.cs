using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static UserProfileAPI.Models.UserModel;

namespace UserProfileAPI.Models
{
    public class UserRelationModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(8,ErrorMessage = "კავშირის ტიპი უნდა იყოს 'კოლეგა','ნაცნობი','ნათესავი' ან 'სხვა'")]
        public string? RelationType { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel User { get; set; }
        [Required]
        public int RelatedUserId { get; set; }

        [ForeignKey("RelatedUserId")]
        public UserModel RelatedUser {  get; set; }
    }
}
