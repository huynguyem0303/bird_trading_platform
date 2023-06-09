using System.ComponentModel;

#nullable disable warnings
namespace BirdTrading.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("Email or phone number")]
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
