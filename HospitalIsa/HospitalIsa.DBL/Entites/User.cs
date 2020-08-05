
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DBL.Entites
{
    public class User : IdentityUser
    {
        public Guid UserId { get; set; }
    }
}
