using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirdTrading.ViewModel
{
    public class RegisterViewModel
    {
        [DisplayName("Email or phone number")]
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
