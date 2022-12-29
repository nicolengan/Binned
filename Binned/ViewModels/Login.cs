using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Binned.ViewModels
{
    public class Login
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }


    }
}
