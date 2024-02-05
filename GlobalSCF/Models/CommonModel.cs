using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    using System;
    using System.Collections.Generic;
    public partial class CommonModel
    {
        public int EduID { get; set; }
        public bool IsActive { get; set; }
        public int EmpTypeID { get; set; }
        public string EmpTypeName { get; set; }
        public string EduName { get; set; }
    }
}