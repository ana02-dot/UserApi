using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserProfileAPI.Filters;


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
        public string LastName { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(ქალი|კაცი)$", ErrorMessage = "სქესი უნდა იყოს 'ქალი' ან 'კაცი'")]
        public string GenderType { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს 11 სიმბოლოს")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255, ErrorMessage = "სურათის მისამართი უნდა შეიცავდეს მაქსიმუმ 255 სიმბოლოს")]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "ტელეფონის ნომრის ველი სავალდებულოა")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "ტელეფონის ნომერი უნდა შეიცავდეს მინიმუმ 4 სიმბოლოს")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "ტელეფონის ტიპის ველი სავალდებულოა")]
        [RegularExpression(@"^(მობილური|ოფისი|სახლი)$", ErrorMessage = "ტელეფონის ტიპი უნდა იყოს 'მობილური', 'ოფისი' ან 'სახლი'")]
        public string PhoneNumberType { get; set; }

        [Required]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public CityModel City { get; set; }

        [JsonIgnore]
        public List<UserRelationModel> UserRelations { get; set; } = new List<UserRelationModel>();

        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        [StringLength(50, MinimumLength =4, ErrorMessage = "მომხმარებლის სახელი უნდა იყოს მაქსიმუმ 50 სიმბოლო და მინიმუმ 4")]
        public string Username { get; set; }

        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "პაროლი უნდა შეიცავდეს მინიმუმ 8 სიმბოლოს, მაქსიმუმ 50ს")]
        public string Password { get; set; }

    }
}
