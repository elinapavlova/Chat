using System.ComponentModel.DataAnnotations;

namespace Models.Dtos.Token
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string EmailUser { get; set; }
    }
}