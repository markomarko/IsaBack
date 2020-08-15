using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ChangePasswordModel
    {
        public Guid userId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
