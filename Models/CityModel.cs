using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserProfileAPI.Models
{
    public class CityModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
