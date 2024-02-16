using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class MenuRoleRights
    {
        public int MenuID { get; set; }
        public int ParentMenuID { get; set; }
        public string MenuName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsMaker { get; set; }
        public bool IsChecker { get; set; }
        public bool IsApprover { get; set; }
        public bool IsView { get; set; }


    }
}