using System.ComponentModel.DataAnnotations;

namespace Models.Dtos.User
{
    public class UserCredentialsDto
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}