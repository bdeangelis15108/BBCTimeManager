using System.ComponentModel.DataAnnotations;

namespace Nucleus.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
