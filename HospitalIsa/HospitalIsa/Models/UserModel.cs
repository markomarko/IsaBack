using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public bool SignedBefore { get; set; }
    }
}
