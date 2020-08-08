using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class MailModel
    {
        public string Subject { get; set; }

        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Body { get; set; }
    }
}
