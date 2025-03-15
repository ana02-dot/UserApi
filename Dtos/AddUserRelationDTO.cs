using System.ComponentModel.DataAnnotations;

namespace UserProfileAPI.Dtos
{
    public class AddUserRelationDTO
    {
        [Required(ErrorMessage = "კავშირის ტიპი სავალდებულოა")]
        [RegularExpression(@"^(კოლეგა|ნაცნობი|ნათესავი|სხვა)$", ErrorMessage = "კავშირის ტიპი უნდა იყოს 'კოლეგა', 'ნაცნობი', 'ნათესავი' ან 'სხვა'")]
        public string RelationType { get; set; }

        [Required(ErrorMessage = "მომხმარებლის ID სავალდებულოა")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "დაკავშირებული მომხმარებლის ID სავალდებულოა")]
        public int RelatedUserId { get; set; }
    }
}