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

        public string FirstName { get; set; }

        public string LastName { get; set; } = string.Empty;
        
        public string GenderType { get; set; } = string.Empty;

        public string PersonalNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string? ImagePath { get; set; }
        
        public string Number { get; set; } = string.Empty;
       
        public string PhoneNumberType { get; set; }
      
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public CityModel City { get; set; }

        [JsonIgnore]
        public List<UserRelationModel> UserRelations { get; set; } = new List<UserRelationModel>();

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
