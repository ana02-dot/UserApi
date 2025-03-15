using System.ComponentModel.DataAnnotations;

namespace UserProfileAPI.Dtos
{
    public class CreateUserDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "სახელი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "სახელი უნდა შეიცავდეს მხოლოდ ქართულ ან ლათინურ ასოებს")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "გვარი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "გვარი უნდა შეიცავდეს მხოლოდ ქართულ ან ლათინურ ასოებს")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(ქალი|კაცი)$", ErrorMessage = "სქესი უნდა იყოს 'ქალი' ან 'კაცი'")]
        public string GenderType { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს 11 სიმბოლოს")]
        public string PersonalNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "ტელეფონის ტიპის ველი სავალდებულოა")]
        [RegularExpression(@"^(მობილური|ოფისი|სახლი)$", ErrorMessage = "ტელეფონის ტიპი უნდა იყოს 'მობილური', 'ოფისი' ან 'სახლი'")]
        public string PhoneNumberType { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "ტელეფონის ნომერი უნდა შეიცავდეს მინიმუმ 4 სიმბოლოს")]
        public string Number { get; set; }
        [Required]
        public string CityName { get; set; }
    }
}
