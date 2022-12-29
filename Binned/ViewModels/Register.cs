using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Binned.ViewModels
{
    public class Register
    {

		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Username { get; set; }

		[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }
	}

}
