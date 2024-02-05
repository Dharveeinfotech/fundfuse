
namespace TMP.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public  class HTMLTemplate_ListAll_Result
    {

        [Required]
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        [Display(Name = "Page Content")]
        public string PageContent { get; set; }
        public string TemplateType { get; set; }
        
        public int HtmlTemplateID { get; set; }
        [Required]
        public string HtmlName { get; set; }
        public string HtmlText { get; set; }
        [Required]
        public int TemplateID { get; set; }
        public bool IsSMS { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string StatusUserDesc { get; set; }
        
        public int CreateBy { get; set; }
        public int CreateBy1 { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateIP { get; set; }
        [AllowHtml]
        public string Editor { get; set; }
        public string RichText1Value { get; set; }
        public string RichText1Copy { get; set; }
        public string issmsvalue { get; set; }
        public string isEmailvalue { get; set; }

        public string SMSText { get; set; }
        [AllowHtml]
        [Display(Name = "Template")]
        public String RichText1 { get; set; }


        [AllowHtml]
        [UIHint("tinymce_jquery_full")]
        public String RichText1FullHtml { get; set; }

        [AllowHtml]
        [Display(Name = "Ritch Text 2")]
        public String RichText2 { get; set; }

        [AllowHtml]
        public String RichText2FullHtml { get; set; }

        // This attributes allows your HTML Content to be sent up

        //[Required]
        //[AllowHtml]
        //[UIHint("tinymce_full_compressed")]

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string HtmlContent { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public bool IsMemo { get; set; }

        public int DynamicTextID { get; set; }
        public string DynamicTextName { get; set; }

        public string ColumnsName { get; set; }
        public string FirstName { get; set; }
        public string ProcessRemark { get; set; }

        public int HtmlTemplateProcessHistoryID { get; set; }
        public string StatusDesc { get; set; }
        
        public string Checked { get; set; }
        public string Approved { get; set; }
        public int ProcessBy { get; set; }
        public System.DateTime ProcessDate { get; set; }

       
    }
}
