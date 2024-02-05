using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class MenuMaster
    {
        public int MenuID { get; set; }
        public int ParentMenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuType { get; set; }
        public bool IsChild { get; set; }
        public string MenuDesc { get; set; }
        public bool IsNewWindow { get; set; }
        public bool IsActive { get; set; }
        public bool IsMultiLayer { get; set; }
        public string Status { get; set; }
        public int ProcessBy { get; set; }
        public System.DateTime ProcessDate { get; set; }
        public string ProcessRemark { get; set; }
        public int CommandID { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateIP { get; set; }
        public string StatusDesc { get; set; }
        public string ParentMenuName { get; set; }
        public int MenuProcessHistoryID { get; set; }
        public string ProcessIP { get; set; }
        public bool IsEnable { get; set; }
        public int Level { get; set; }

        public List<MenuMaster> lstMenuMaster { get; set; }
    }
}