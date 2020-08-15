using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class UserPOCO 
    {
        public Guid UserId { get; set; }
        public bool SignedBefore { get; set; }
    }
}
