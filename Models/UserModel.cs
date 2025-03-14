using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserProfileAPI.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "სახელი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "სახელი უნდა შეიცავდეს მხოლოდ ქართულ ან მხოლოდ ლათინურ ასოებს")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "გვარი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "სახელი უნდა შეიცავდეს მხოლოდ ქართულ ან მხოლოდ ლათინურ ასოებს")]
        public string LastName { get; set; }
        [Required]
        [StringLength(4, ErrorMessage = "სქესი უნდა იყოს 'ქალი' ან 'კაცი'")]
        public string GenderType { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს 11 სიმბოლოს")]
        public string PersonalNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255, ErrorMessage = "სურათის მისამართი უნდა შეიცავდეს მაქსიმუმ 255 სიმბოლოს")]
        public string? ImagePath { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "ტელეფონის ნომერი უნდა შეიცავდეს მინიმუმ 4 სიმბოლოს")]
        public string Number { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 5, ErrorMessage = "მობილურის ტიპი უნდა იყოს 'ოფისი','სახლი' ან 'მობილური'")]
        public string? PhoneNumberType { get; set; }

        [Required]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public CityModel City { get; set; }
       

        [JsonIgnore]
        public List<UserRelationModel> UserRelations { get; set; } = new List<UserRelationModel>();

    }
}
