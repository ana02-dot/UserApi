using System.ComponentModel.DataAnnotations;

namespace UserProfileAPI.Dtos
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        public string Username { get; set; }

        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        public string Password { get; set; }
    }
}