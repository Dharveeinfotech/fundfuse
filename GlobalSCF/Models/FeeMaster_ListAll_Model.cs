using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TMP.Models
{
    public class FeeMaster_ListAll_Model
    {
        public int FeeId { get; set; }
        [Required(ErrorMessage = "Fee Name are required")]
        public string FeeName { get; set; }
        public string StatusUserDesc { get; set; }
        public bool IsActive { get; set; }
        public bool IsFix { get; set; }
        public string Status { get; set; }
        public int FeeProcessHistoryID { get; set; }
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
        public string Checked { get; set; }
        public string Amendment { get; set; }
        public string Approved { get; set; }
        public string Reject { get; set; }
        public List<CustomerTypeMaster_ListAll_Result> lstCustomerTypeMaster { get; set; }
        public List<FeeDetail_ListAll_Result> lstFeeDetailMaster { get; set; }
        public List<FeeMaster_ListAll_Result> lstFeeMaster { get; set; }
        public List<FeeMasterProcessHistory_ListAllBind_Result> lstFeeMasterListBind { get; set; }
        public List<FeeDetailProcessHistory_ListAllBind_Result> lstFeeDetPHListBind { get; set; }
        //public List<MenuRights_ListAll_Result> lstMenuRights { get; set; }
    }
}