using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class ResendVerificationEmailRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}