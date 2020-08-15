using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class ChangePasswordPOCO
    {
        public Guid userId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
