using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class FAQ
    {
        public int FaqID { get; set; }
        public string CustomerTypeName { get; set; }
        public string Questions { get; set; }
        public string Answer { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerType { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public List<FAQ> faqls { get; set; }

    }
}