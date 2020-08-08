using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class MailPOCO
    {
        public string Subject { get; set; }

        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Body { get; set; }
    }
}
