using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserProfileAPI.Models
{
    public class PhoneNumberModel
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string? PhoneNumberType { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "ტელეფონის ნომერი უნდა შეიცავდეს მინიმუმ 4 სიმბოლოს")]
        public string Number { get; set; }
        public int UserId {  get; set; }
        [JsonIgnore]
        public UserModel User { get; set; }

        public static readonly List<string> PhoneNumberTypes = new List<string>
        {
            "მობილური",
            "ოფისი",
            "სახლი"
        };
    }
}
