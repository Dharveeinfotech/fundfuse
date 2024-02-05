
namespace TMP.Models
{
    using System;

    public partial class DesignationMaster_ListAll_Result
    {
        private string _DesignationName = "";

        private string _Status = "";
        private string _Keywordvalue = "";
        private bool _IsKeywordSearch = false;

        private int _DesignationID = 0;
        public int DesignationID { get { return _DesignationID; } set { _DesignationID = value; } }
        public string DesignationName { get { return _DesignationName; } set { _DesignationName = value; } }
        public bool IsActive { get; set; }
        public string Status { get { return _Status; } set { _Status = value; } }
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
        public bool IsKeywordSearch { get { return _IsKeywordSearch; } set { _IsKeywordSearch = value; } }
        public string Keywordvalue { get { return _Keywordvalue; } set { _Keywordvalue = value; } }

        public short Active { get; set; }
    }
}
