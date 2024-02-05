using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ContactUsModel
    {
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Website { get; set; }
        public string ContactDesc { get; set; }
        public int CompanyID { get; set; }
        public int LanguageID { get; set; }

    }
}