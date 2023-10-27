using System.ComponentModel.DataAnnotations;

namespace SuperHeroApi.Models
{
    public class UserInfoDto
    {
        [Required]
        public string UserName { get; set; } 

        [Required]
        public string UserPassword { get; set; } 

    }
}
