﻿using System.ComponentModel.DataAnnotations;

namespace UserProfileAPI.Dtos
{
    public class UpdateUserDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "სახელი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "სახელი უნდა შეიცავდეს მხოლოდ ქართულ ან მხოლოდ ლათინურ ასოებს")]
        public string? FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "გვარი უნდა შეიცავდეს 2-50 სიმბოლოს")]
        [RegularExpression(@"^([ა-ჰ]+|[a-zA-Z]+)$", ErrorMessage = "სახელი უნდა შეიცავდეს მხოლოდ ქართულ ან მხოლოდ ლათინურ ასოებს")]
        public string? LastName { get; set; }

        [AllowedValues("ქალი", "კაცი", ErrorMessage = "სქესი უნდა იყოს 'ქალი' ან 'კაცი'")]
        //[RegularExpression(@"^(ქალი|კაცი)$", ErrorMessage = "სქესი უნდა იყოს 'ქალი' ან 'კაცი'")]
        public string? GenderType { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს 11 სიმბოლოს")]
        public string? PersonalNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50, MinimumLength = 4, ErrorMessage = "ტელეფონის ნომერი უნდა შეიცავდეს მინიმუმ 4 სიმბოლოს")]
        public string? Number { get; set; }

        [RegularExpression(@"^(მობილური|ოფისი|სახლი)$", ErrorMessage = "ტელეფონის ტიპი უნდა იყოს 'მობილური', 'ოფისი' ან 'სახლი'")] 
        public string? PhoneNumberType { get; set; }
        public string? CityName { get; set; }

    }
}
