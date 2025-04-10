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
       
        public string RelationType { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public UserModel User { get; set; }

        public int RelatedUserId { get; set; }

        [JsonIgnore]
        [ForeignKey("RelatedUserId")]
        public UserModel RelatedUser { get; set; }


    }
}
