using System.ComponentModel.DataAnnotations;

namespace Application.InputModels
{
    public class LoginInputModel
    {
        public LoginInputModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        public string Email { get; }
        [Required]
        public string Password { get; }


    }
}