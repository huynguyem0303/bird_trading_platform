using BirdTrading.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirdTrading.Repository.DTO
{
    public class AddUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string AvatarURL { get; set; }
        public bool IsTempUser { get; set; }
        public bool IsBlocked { get; set; }
        public int ShippingInforId { get; set; }
    }
}
