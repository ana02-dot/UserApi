using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static UserProfileAPI.Models.UserModel;

namespace UserProfileAPI.Models
{
    public class UserRelationModel
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        [RegularExpression(@"^(კოლეგა|ნაცნობი|ნათესავი|სხვა)$", ErrorMessage = "კავშირის ტიპი უნდა იყოს 'კოლეგა','ნაცნობი','ნათესავი' ან 'სხვა'")]
        public string RelationType { get; set; }

        [Required(ErrorMessage = "ID სავალდებულოა")]
        public int UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        [Required(ErrorMessage = "დაკავშირებული მომხმარებლის ID სავალდებულოა")]
        public int RelatedUserId { get; set; }

        [JsonIgnore]
        [ForeignKey("RelatedUserId")]
        public UserModel RelatedUser { get; set; }


    }
}
