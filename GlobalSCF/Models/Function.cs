
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TMP.DAL;
using TMP.Infrastructure;

namespace TMP.Models
{
    public class Function
    {
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }

        private const string RememberMeCookieName = "MyCookieName";
        public string GetSystemIP()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
        public static int LoggedInUserID
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt16(HttpContext.Current.Session["UserId"].ToString());
                }
                return 0;
            }
        }
        public int LoggedUserID
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt16(HttpContext.Current.Session["UserId"].ToString());
                }
                return 0;
            }
        }
        public int ProcessByID
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt16(HttpContext.Current.Session["UserId"].ToString());
                }
                return 0;
            }
        }
        public int UpdateByID
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt16(HttpContext.Current.Session["UserId"].ToString());
                }
                return 0;
            }
        }
        public int DeleteByID
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                {
                    return Convert.ToInt16(HttpContext.Current.Session["UserId"].ToString());
                }
                return 0;
            }
        }
        public string[] Checkcredentials()
        {
            string[] data = { "", "", "" };
            ClsCountryMaster _clsCon = new ClsCountryMaster();
            CountryMaster _objModel = new CountryMaster();
            ClsUserMaster _clsUser = new ClsUserMaster();
            UserMaster_ListAll_Result _objUserModel = new UserMaster_ListAll_Result();

            _objModel = _clsCon.SystemPerameter_ListAll().FirstOrDefault();
            if (_objModel != null)
            {
                if (_objModel.IsMaintenance == true)
                {
                    Infrastructure.Web.SessionManager.ClearSession();
                }
            }
            //if (HttpContext.Current.Session["LoginKey"] != null && HttpContext.Current.Session["LoginMatch"] != null && HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserId"] != null)
            if (HttpContext.Current.Session["LoginKey"] != null &&  HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserId"] != null)
            {
                string LoginKey = HttpContext.Current.Session["LoginKey"].ToString();
                //string LoginMatch = HttpContext.Current.Session["LoginMatch"].ToString();

                _objUserModel.UserID = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
                _objUserModel = _clsUser.UserSessionHistory_ListAll(_objUserModel).FirstOrDefault();
                //if (LoginKey == LoginMatch)
                //{

                    Guid GU = Guid.NewGuid();

                    HttpContext.Current.Session["LoginKey"] = GU.ToString();

                    //HttpContext.Current.Session["LoginMatch"] = GU.ToString();


                    data[0] = "pass";

                    data[1] = HttpContext.Current.Session["UserId"].ToString();

                    data[2] = HttpContext.Current.Session["UserName"].ToString();

                    if (_objUserModel != null && _objUserModel.CurrentSessionID == Convert.ToString(HttpContext.Current.Session["CurrentSessionID"]))
                    {

                        data[0] = "pass";
                    }
                    //else
                    //{
                    //    data[0] = "fail";
                    //    if (_objUserModel != null)
                    //    {
                    //        HttpContext.Current.Session["LoginDevice"] = _objUserModel.LoginDevice;
                    //    }
                    //}
                //}
                //else
                //{
                //    data[0] = "fail";
                //}

            }
            else
            {
                data[0] = "fail";
            }
            return data;
        }
        public UserRights_Result CheckUserRight(string pMenuID, string bredCrumName, int pParentMenuID)
        {
            List<MenuRoleRights> MenuRole = new List<MenuRoleRights>().ToList();
            UserRights_Result UserRight = new UserRights_Result();
            try
            {
                ClsMenuRoleRights db = new ClsMenuRoleRights();
                if (HttpContext.Current.Session["RoleId"] != null)
                {
                    int MenuID = 0;
                    if (string.IsNullOrEmpty(pMenuID))
                    {
                        int.TryParse(pMenuID.ToString(), out MenuID);
                    }

                    string RoleId = HttpContext.Current.Session["RoleId"].ToString();
                    string PageName = Convert.ToString(HttpContext.Current.Session["PageName"]);
                    int breadcrumval = 0;

                    if (PageName == "Maker" && bredCrumName == "Maker")
                    {
                        bredCrumName = "Maker";
                        breadcrumval = 1;
                        if (pParentMenuID == 0)
                        {
                            UserRight = null;
                        }
                        else
                        {
                            MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), pParentMenuID, 0, UserRoletatus()).ToList();
                        }
                    }
                    else if (bredCrumName == "Update")
                    {
                        if (pParentMenuID == 0)
                        { UserRight = null; }
                        else
                        {
                            int pUpdateMenuID = 0;
                            // pUpdateMenuID = Convert.ToInt32(HttpContext.Current.Session["UpdateMenuID"]);
                            pUpdateMenuID = Convert.ToInt32(HttpContext.Current.Session["AddMenuID"]);
                            if (pUpdateMenuID > 0)
                            {
                                MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), pUpdateMenuID, 0, UserRoletatus()).ToList();
                            }
                            else
                            {
                                MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), pParentMenuID, 0, UserRoletatus()).ToList();
                            }
                        }
                    }
                    else if (bredCrumName == "Add")
                    {
                        if (pParentMenuID == 0)
                        { UserRight = null; }
                        else
                        {
                            pParentMenuID = Convert.ToInt32(HttpContext.Current.Session["AddMenuID"]);
                            MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), pParentMenuID, 0, UserRoletatus()).ToList();
                        }
                    }
                    else if (PageName == "" && bredCrumName == "Maker")
                    {
                        bredCrumName = "Maker";
                        breadcrumval = 1;
                        if (pParentMenuID == 0)
                        {
                            UserRight = null;
                        }
                        else
                        {
                            MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), 0, pParentMenuID, UserRoletatus()).ToList();
                        }
                    }
                    else
                    {
                        if (pParentMenuID == 0)
                        { UserRight = null; }
                        else
                        {
                            MenuRole = db.MenuRoleRights_ListAll(RoleId.ToString(), pParentMenuID, 0, UserRoletatus()).ToList();
                        }
                    }

                    if (MenuRole.Count > 0)
                    {
                        for (int i = 0; i < MenuRole.Count; i++)
                        {
                            UserRight.MenuID = MenuRole[i].MenuID;
                            UserRight.ParentMenuID = MenuRole[i].ParentMenuID;
                            UserRight.MenuName = MenuRole[i].MenuName;
                            UserRight.RoleID = MenuRole[i].RoleID;
                            UserRight.RoleName = MenuRole[i].RoleName;
                            UserRight.IsMaker = MenuRole[i].IsMaker;
                            if (UserRight.IsMaker)
                            {
                                if (UserRight.MenuName == "Add") { UserRight.IsAdd = true; UserRight.AddMenuID = MenuRole[i].MenuID; HttpContext.Current.Session["AddMenuID"] = MenuRole[i].MenuID; if (breadcrumval == 0) { bredCrumName = "Add"; } }
                                if (UserRight.MenuName == "Update") { UserRight.IsUpdate = true; UserRight.UpdateMenuID = MenuRole[i].MenuID; HttpContext.Current.Session["AddMenuID"] = MenuRole[i].MenuID; if (breadcrumval == 0) { bredCrumName = "Update"; } }
                            }
                            UserRight.IsChecker = MenuRole[i].IsChecker;
                            UserRight.IsApprover = MenuRole[i].IsApprover;
                            UserRight.IsView = MenuRole[i].IsView;
                        }

                        if (!string.IsNullOrEmpty(bredCrumName))
                        {
                            string Crum = HttpContext.Current.Session["bredCrumpage"] + "/" + bredCrumName;
                            HttpContext.Current.Session["bredCrum"] = Crum;
                        }
                        else
                        {
                            string Crum = HttpContext.Current.Session["bredCrumpage"] + "/" + UserRight.MenuName;
                            HttpContext.Current.Session["bredCrum"] = Crum;
                        }
                    }
                    else
                    { UserRight = null; }
                }
                else
                { UserRight = null; }
            }
            catch (Exception ex)
            { UserRight = null; }
            return UserRight;
        }
        public string CheckForCookieUserName()
        {
            string returnValue = string.Empty;
            HttpCookie rememberMeUserNameCookie = HttpContext.Current.Request.Cookies.Get(RememberMeCookieName);
            if (null != rememberMeUserNameCookie)
            {
                /* Note, the browser only sends the name/value to the webserver, and not the expiration date */
                returnValue = rememberMeUserNameCookie.Value;
            }

            return returnValue;
        }
        public void CreateRememberMeCookie(string userName)
        {
            HttpCookie rememberMeCookie = new HttpCookie(RememberMeCookieName, userName);
            rememberMeCookie.Expires = DateTime.MaxValue;
            HttpContext.Current.Response.SetCookie(rememberMeCookie);
        }
        public void RemoveRememberMeCookie()
        {
            /* k1ll the cookie ! */
            HttpCookie rememberMeUserNameCookie = HttpContext.Current.Request.Cookies[RememberMeCookieName];
            if (null != rememberMeUserNameCookie)
            {
                HttpContext.Current.Response.Cookies.Remove(RememberMeCookieName);
                rememberMeUserNameCookie.Expires = DateTime.Now.AddYears(-1);
                rememberMeUserNameCookie.Value = null;
                HttpContext.Current.Response.SetCookie(rememberMeUserNameCookie);
            }
        }
        public string GetMasterData(string tempName, DataTable DT, string toMail, string Sub, string DocPath = "", int UserID = 0, int intInvoiceID = 0)
        {
            string MessageText = string.Empty;
            string htmlString = "";
            HTMLTemplate_ListAll_Result _objTemplate = new HTMLTemplate_ListAll_Result();
            ClsHTMLTemplate _clsTemp = new ClsHTMLTemplate();
            _objTemplate.HtmlTemplateID = 0;
            _objTemplate.HtmlName = tempName;
            _objTemplate = _clsTemp.HTMLTemplate_ListAll(_objTemplate.HtmlTemplateID, tempName, 1, "", false, "", 0).FirstOrDefault();
            string ViewSample = "";
            string ViewSub = "";
            int CustomerID = 0;
            if (_objTemplate != null)
            {
                Sub = _objTemplate.Subject;
                MessageText = _objTemplate.HtmlText;

                if (intInvoiceID > 0)
                {
                    htmlString = getHtml(Convert.ToInt32(DT.Rows[0]["InvoiceID"]));
                    DT.Columns.Add("Table", typeof(string));
                    DT.Rows[0]["Table"] = htmlString;
                }

                foreach (DataRow dr in DT.Rows)
                {
                    ViewSample = GetText(dr, MessageText);
                    ViewSub = GetText(dr, Sub);
                }

                if (DT != null)
                {
                    if (intInvoiceID > 0)
                        CustomerID = 0;
                    else
                        CustomerID = Convert.ToInt32(DT.Rows[0]["CustomerID"]);
                }

                SendEmail(toMail, ViewSub, ViewSample, _objTemplate.HtmlTemplateID, CustomerID, 0, "", Infrastructure.Web.StatusManager.OtherDetail.Constant.EmailConfigID, DocPath, UserID);
            }
            return ViewSample;
        }

        public string getHtml(int InvoiceID = 0)
        {
            try
            {
                DataTable DT = new DataTable();
                InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                _objModel.InvoiceID = InvoiceID;
                ClsInvoiceTransaction _clsInvoi = new ClsInvoiceTransaction();
                DT = _clsInvoi.InvoiceDetail_ListAllDT(_objModel);
                string messageBody = "<font></font>";

                if (DT == null)
                    return messageBody;
                string htmlTableStart = "<table class=\"MsoNormalTable\" style=\"border-collapse: collapse; mso-yfti-tbllook: 1184; mso-padding-alt: 0in 0in 0in 0in;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\"width: 131.75pt; border: solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt;\">";
                string htmlTdEnd = "</td>";
                string spnTdStart = "<span style=\"font-weight:bold\">";
                string spnTdEnd = "</span>";

                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + spnTdStart + "Item No." + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Invoice No." + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Invoice Date" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Currency" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Amount" + spnTdEnd + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;

                int i = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    i++;
                    messageBody = messageBody + htmlTrStart;
                    messageBody = messageBody + htmlTdStart + "( " + Convert.ToString(i) + " )" + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["InvoiceNo"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToDateTime(dr["InvoiceDate"]).ToString("dd-MMM-yyyy") + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["CurrencyCode"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(string.Format("{0:0,0.00}", dr["Amt"])) + htmlTdEnd;
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;


                return messageBody;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string getOblHtml(int InvoiceID = 0)
        {
            try
            {
                DataTable DT = new DataTable();
                InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                _objModel.InvoiceID = InvoiceID;
                ClsInvoiceTransaction _clsInvoi = new ClsInvoiceTransaction();
                DT = _clsInvoi.InvoiceDetail_ListAllDT(_objModel);
                string messageBody = "<font></font>";

                if (DT == null)
                    return messageBody;
                string htmlTableStart = "<table class=\"MsoNormalTable\" style=\"border-collapse: collapse; mso-yfti-tbllook: 1184; mso-padding-alt: 0in 0in 0in 0in;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\"width: 131.75pt; border: solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt;\">";
                string htmlTdEnd = "</td>";

                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "Sr No." + htmlTdEnd;
                messageBody += htmlTdStart + "Counterparty" + htmlTdEnd;
                messageBody += htmlTdStart + "Invoice Number" + htmlTdEnd;
                messageBody += htmlTdStart + "Invoice Date" + htmlTdEnd;
                messageBody += htmlTdStart + "Currency" + htmlTdEnd;
                messageBody += htmlTdStart + "Billed Amount" + htmlTdEnd;
                messageBody += htmlTdStart + "Due Date" + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;

                int i = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    i++;
                    messageBody = messageBody + htmlTrStart;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(i) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["ObligorName"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["InvoiceNo"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToDateTime(dr["InvoiceDate"]).ToString("dd-MMM-yyyy") + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["CurrencyCode"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(string.Format("{0:0,0.00}", dr["Amt"])) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToDateTime(dr["DueDate"]).ToString("dd-MMM-yyyy") + htmlTdEnd;
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;


                return messageBody;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string getAcct_DetailsHtml(int InvoiceID = 0)
        {
            try
            {
                DataTable DT = new DataTable();
                InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                _objModel.InvoiceID = InvoiceID;
                ClsInvoiceTransaction _clsInvoi = new ClsInvoiceTransaction();
                DT = _clsInvoi.InvoiceHTMLDetail(InvoiceID, Convert.ToInt32(HttpContext.Current.Session["UserId"]));

                string messageBody = "<font></font>";

                if (DT == null)
                    return messageBody;
                string htmlTableStart = "<table class=\"MsoNormalTable\" style=\"border-collapse: collapse; mso-yfti-tbllook: 1184; mso-padding-alt: 0in 0in 0in 0in;\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"mso-yfti-irow: 1; mso-yfti-lastrow: yes;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\"width: 131.75pt; border: solid windowtext 1.0pt; padding: 0in 5.4pt 0in 5.4pt;\">";
                string htmlTdEnd = "</td>";
                string spnTdStart = "<span style=\"font-weight:bold\">";
                string spnTdEnd = "</span>";

                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                //messageBody += htmlTdStart + "Sr No." + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Account Name" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Bank Name" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Account Number" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "Swift Code" + spnTdEnd + htmlTdEnd;
                messageBody += htmlTdStart + spnTdStart + "IBAN" + spnTdEnd + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;

                int i = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    i++;
                    messageBody = messageBody + htmlTrStart;
                    //messageBody = messageBody + htmlTdStart + Convert.ToString(i) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["AccountName"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["BankName"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["AccountNumber"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["SwiftCode"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTdStart + Convert.ToString(dr["IBAN"]) + htmlTdEnd;
                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;


                return messageBody;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string GetInvoiceMasterData(int InvoiceID, string HTMLText, int UserID = 0)
        {
            DataTable DT = new DataTable();
            string ViewSample = "";
            string htmlString = "";
            string htmlOblString = "";
            string htmlAccString = "";
            ClsHTMLTemplate dbHtml = new ClsHTMLTemplate();
            ClsInvoiceTransaction _Clsinvoice = new ClsInvoiceTransaction();
            DT = _Clsinvoice.InvoiceHTMLDetail(InvoiceID, UserID);
            if (InvoiceID > 0)
            {
                htmlString = getHtml(Convert.ToInt32(DT.Rows[0]["InvoiceID"]));
                htmlOblString = getOblHtml(Convert.ToInt32(DT.Rows[0]["InvoiceID"]));
                htmlAccString = getAcct_DetailsHtml(Convert.ToInt32(DT.Rows[0]["InvoiceID"]));
            }
            DT.Columns.Add("Table", typeof(string));
            DT.Columns.Add("Table-obl", typeof(string));
            DT.Columns.Add("Table-Acct-Details", typeof(string));

            DT.Rows[0]["Table"] = htmlString;
            DT.Rows[0]["Table-obl"] = htmlOblString;
            DT.Rows[0]["Table-Acct-Details"] = htmlAccString;
            foreach (DataRow dr in DT.Rows)
            {
                ViewSample = GetText(dr, HTMLText);
            }
            return ViewSample;
        }
        public int SendEmail(string To, string Subject, string MessageBody, int HtmlTemplateID, int FromCustID, int InvoiceID, string ResponseText, int EmailConfigID, string DocPath = "", int UserID = 0)
        {
            string SenderName = "";
            int retval = 0;
            string from = string.Empty;
            string Password = string.Empty;
            string SmtpHost = string.Empty;
            string Bcc = string.Empty;
            string CC = string.Empty;
            string DisplayName = string.Empty;
            int intLogID = 0;
            ClsEmailConfiguration objEmailconf = new ClsEmailConfiguration();
            //CountryMaster EmailConfiList = new CountryMaster();
            //EmailConfiList = objEmailconf.EmailConfiguration_ListAll(EmailConfigID, "", false, "").FirstOrDefault();


            //from = "scfplatform@gmail.com";
            //Password = "azhar1248";
            //SmtpHost = "smtp.gmail.com";

            //from = "support@scfplatform.com";
            //Password = "scf@776";
            //SmtpHost = "jupiter.dnsindia.net";
            //DisplayName = "SCF Support";

            from = "jayeshkumar.dba@gmail.com";
            Password = "zlus tkzu rssw mzyo";
            SmtpHost = "smtp.gmail.com";
            DisplayName = "SCF Support";



            //from = "tech@trade-hub.ae";
            //Password = "TradeHub@123$";
            //SmtpHost = "d935.lon1.mysecurecloudhost.com";
            //DisplayName = "Trade Hub Support";

            //Bcc = EmailConfiList.BCCEmailID;
            //CC = EmailConfiList.CCEmailID;
            //if (To == "")
            //{
            //    To = EmailConfiList.EmailID;
            //}
            //if (SenderName != "")
            //{
            //    DisplayName = SenderName;
            //}
            //else
            //{
            //    DisplayName = EmailConfiList.DisplayName;
            //}

            objEmailconf.conn = ClsAppDatabase.GetCon();
            if (objEmailconf.conn.State == ConnectionState.Closed)
            { objEmailconf.conn.Open(); }
            else { objEmailconf.conn.Close(); objEmailconf.conn.Open(); }
            objEmailconf.tras = objEmailconf.conn.BeginTransaction();

            try
            {
                ObjectParameter pLogID = new ObjectParameter("pLogID", typeof(int));
                intLogID = objEmailconf.EmailSMSLogDetail_Add(pLogID, HtmlTemplateID, FromCustID, UserID, from, To, MessageBody);
                objEmailconf.tras.Commit();
                objEmailconf.conn.Close();
            }
            catch (Exception ex)
            { objEmailconf.tras.Rollback(); objEmailconf.conn.Close(); }

            using (MailMessage mail = new MailMessage(Convert.ToString(new MailAddress(from, DisplayName)), To))
            {

                mail.Subject = Subject;
                mail.Body = MessageBody;

                string[] BCCMail = Bcc.Split(',');
                if (BCCMail.Length > 0)
                {
                    for (int i = 0; i < BCCMail.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(BCCMail[i]))
                        {
                            mail.Bcc.Add(BCCMail[i]);
                        }
                    }
                }
                string[] CCMail = CC.Split(',');

                for (int k = 0; k < CCMail.Length; k++)
                {
                    if (!string.IsNullOrEmpty(CCMail[k]))
                    {
                        mail.CC.Add(CCMail[k]);
                    }
                }


                if (DocPath != "" && DocPath != null)
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(DocPath);
                    Stream stream = new MemoryStream(bytes);
                    mail.Attachments.Add(new Attachment(stream, "attachment.pdf"));
                }

                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = SmtpHost;
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from, Password);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                //smtp.Port = 465;
                smtp.Send(mail);
                retval = 1;
            }

            if (retval == 1)
            {
                try
                {
                    objEmailconf.conn = ClsAppDatabase.GetCon();
                    if (objEmailconf.conn.State == ConnectionState.Closed)
                    { objEmailconf.conn.Open(); }
                    else { objEmailconf.conn.Close(); objEmailconf.conn.Open(); }
                    objEmailconf.tras = objEmailconf.conn.BeginTransaction();

                    objEmailconf.EmailSMSLogDetail_Update(intLogID, true, ResponseText);
                    objEmailconf.tras.Commit();
                    objEmailconf.conn.Close();
                }
                catch (Exception ex)
                { objEmailconf.tras.Rollback(); objEmailconf.conn.Close(); }
            }
            return retval;
        }
        public string AddHTML(string Input)
        {
            return "<HTML><head></head><Body>" + Input + "</Body></HTML>";
        }
        public string CreateErrorMessage(Exception serviceException)
        {
            StringBuilder messageBuilder = new StringBuilder();

            try
            {
                messageBuilder.Append("\n\n");
                messageBuilder.AppendLine("\n\n");
                messageBuilder.AppendLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
                messageBuilder.Append("The Exception is:-");

                messageBuilder.Append("Exception :: " + serviceException.ToString());
                if (serviceException.InnerException != null)
                {
                    messageBuilder.Append("InnerException :: " + serviceException.InnerException.ToString());
                }
                messageBuilder.AppendLine("\n\n");
                messageBuilder.AppendLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");

                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.Append("Exception:: Unknown Exception.");
                return messageBuilder.ToString();
            }

        }
        public void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string logFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~\\ProgramLog\\"));// "D:\\Project\\LogError\\";
                message = "<Start Issue>" + System.DateTime.Now + "\t" + message + "\n <End Issue>";
                logFilePath = logFilePath + "ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

                if (logFilePath.Equals("")) return;
                #region Create the Log file directory if it does not exists 
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();


                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(message);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }
        public void LogFileWriteNew(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string logFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~\\ProgramLogNew\\"));// "D:\\Project\\LogError\\";
                message = "<Start Issue>" + System.DateTime.Now + "\t" + message + "\n <End Issue>";
                logFilePath = logFilePath + "ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt";

                if (logFilePath.Equals("")) return;
                #region Create the Log file directory if it does not exists 
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();


                #endregion Create the Log file directory if it does not exists

                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(message);
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }

        }
        public string SMSSendingProccess(int? id, string msg, int iFlag)
        {
            //updateby rajni 20150929
            //ClsFactoring _ClsFactoring = new ClsFactoring();
            DataTable DT = new DataTable();
            string viewsample = "";
            //add Rajni 20150929
            ClsHTMLTemplate dbHtml = new ClsHTMLTemplate();
            //updateby rajni 20150929
            //DataSet ds = _ClsFactoring.GetDataSetUser("UserMaster_ListAll", id, 0, "", "", "", -1, "", false, "");
            //add Rajni 20150929
            DataSet ds = dbHtml.GetDataSetUser(id, 0, "", "", "", -1, "", false, "");
            DT = ds.Tables[0];
            foreach (DataRow dr in DT.Rows)
            {
                viewsample = GetSMSText(dr, msg);
            }
            string username = "vnt-test";
            string password = "demo";
            string senderid = "SHAKTI";

            string MobileNo = string.Empty;
            string loginname = Convert.ToString(ds.Tables[0].Rows[0]["LoginName"]);
            string userpassword = Convert.ToString(ds.Tables[0].Rows[0]["Password"]);
            MobileNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
            string message = "Your UserName :" + loginname + " and Password :" + userpassword;
            //string MobileNo = "9833628828";
            string OTPNumber = string.Empty;
            //SendSMSInternational(username, password, senderid, message, MobileNo);
            //SendSMS(username, password, senderid, message, MobileNo);
            OTPNumber = CreateRandomOTPNumber(6);
            string strLive = string.Empty;
            string strSendSMS = string.Empty;
            strLive = System.Configuration.ConfigurationManager.AppSettings["IsLive"];
            strSendSMS = System.Configuration.ConfigurationManager.AppSettings["SendSMS"];
            if (strLive.ToLower() == "true")
            {
                //if (iFlag == 0)
                if (strSendSMS.ToLower() == "true")
                {
                    SendOTPNumberandPassword(userpassword, OTPNumber, MobileNo);
                    //SendOTPNumberandPasswordLocal(userpassword, OTPNumber);
                }
            }


            ClsUserMaster dbh = new ClsUserMaster();
            dbh.conn = ClsAppDatabase.GetCon();
            if (dbh.conn.State == ConnectionState.Closed)
            { dbh.conn.Open(); }
            else { dbh.conn.Close(); dbh.conn.Open(); }
            dbh.tras = dbh.conn.BeginTransaction();

            ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
            dbh.UserLoginHistory_Add(OutUserLoginHistoryID, id, loginname, userpassword, OTPNumber, true, true, "Success Send OTP", GetSystemIP());
            dbh.tras.Commit();
            dbh.conn.Close();

            return viewsample;
        }
        public string GetSMSText(DataRow dr, string msg)
        {
            string strSMS = msg;// Message Text

            while (strSMS.Contains("{"))
            {
                string ColumnName = strSMS.Substring(strSMS.IndexOf("{") + 1, strSMS.IndexOf("}") - strSMS.IndexOf("{") - 1);
                if (ColumnName == Convert.ToString(dr.Table.Columns[ColumnName]))
                {
                    strSMS = strSMS.Replace("{" + ColumnName + "}", GetPlainText(dr[ColumnName], dr.Table.Columns[ColumnName].DataType.ToString()).Trim());
                }
                else if (ColumnName == "Grid")
                {

                    int UserID = Convert.ToInt32(dr.Table.Rows[0]["UserID"]);
                    string RoleId = string.Empty;
                    ClsUserRoleLinkMaster URLM = new ClsUserRoleLinkMaster();
                    var UserRole = URLM.UserRoleLinkMaster_ListAll(0, UserID, 0, 1, "NA1", false, "").ToList();
                    if (UserRole != null)
                    {
                        if (UserRole.Count > 0)
                        {
                            for (int i = 0; i < UserRole.Count; i++)
                            {
                                if (i == 0)
                                {
                                    RoleId = Convert.ToString(UserRole[i].RoleID);
                                }
                                else
                                {
                                    RoleId += "," + Convert.ToString(UserRole[i].RoleID);
                                }
                            }
                        }
                    }

                    string userrolestring = string.Empty;
                    if (!string.IsNullOrEmpty(RoleId))
                    {
                        userrolestring = SendUserRightinMail(RoleId);
                        strSMS = strSMS.Replace("{" + ColumnName + "}", userrolestring);
                    }
                    else
                    {
                        strSMS = strSMS.Replace("{" + ColumnName + "}", "");
                    }

                }
                else
                {
                    strSMS = strSMS.Replace("{" + ColumnName + "}", "");
                }

            }

            return strSMS;
        }
        public string GetOTPText(string msg, string OTPNumber)
        {
            string strSMS = msg;// Message Text
            string OTP = string.Empty;
            OTP = "OTP";
            while (strSMS.Contains("{"))
            {
                string ColumnName = strSMS.Substring(strSMS.IndexOf("{") + 1, strSMS.IndexOf("}") - strSMS.IndexOf("{") - 1);
                if (ColumnName == Convert.ToString(OTP))
                {
                    strSMS = strSMS.Replace("{" + ColumnName + "}", OTPNumber);
                }
                else
                {
                    strSMS = strSMS.Replace("{" + ColumnName + "}", "");
                }

            }

            return strSMS;
        }
        public string GetPlainText(object obj, string Type)
        {
            if (Type.Equals("System.DateTime"))
            {
                return Convert.ToDateTime(obj.ToString()).Date.ToString("dd-MMM-yyyy");
            }

            if (Type.Equals("System.Decimal"))
            {
                return Convert.ToDecimal(obj.ToString()).ToString("#,##0.00");
                //Convert.ToDateTime(obj.ToString()).Date.ToString("dd-MMM-yyyy");
            }
            return obj.ToString();
        }
        public static string SendUserRightinMail(string RoleId)
        {
            return "";
            //string userRight = string.Empty;
            //List<MenuRoleRights_ListAll_Result> MenuRole = new List<MenuRoleRights_ListAll_Result>().ToList();
            //ClsMenuRoleRights db = new ClsMenuRoleRights();
            //if (!string.IsNullOrEmpty(RoleId))
            //{

            //    Function FN = new Function();
            //    MenuRole = db.MenuRoleRights_ListAll(RoleId, 0, 0, FN.UserRoletatus()).ToList();
            //    if (MenuRole.Count > 0)
            //    {
            //        foreach (var item in MenuRole)
            //        {
            //            if (item.MenuName == "User Management")
            //            {

            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;

            //                        if (menuname == "User")
            //                        {

            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;
            //                            //userRight += "<p style=\"margin-left:10%;color:#1e5f9a;\" ><b style=\"color:#286090;\" >" + menuname + " </b>";
            //                            userRight += "<p style=\"margin-left:10%;\">";
            //                            if (subitem.IsMaker)
            //                            {
            //                               // userRight += "'Maker'";
            //                                iCountIsMaker = 1;

            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                               // userRight += "'Checker'";
            //                                iCountIsChecker = 1;
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                               // userRight += "'Approver'";
            //                                iCountIsApprover = 1;
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker,iCountIsChecker,iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString+ " for <b>" + menuname + " </b></p>";
            //                            }

            //                        }
            //                        if (menuname == "Role")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;
            //                            userRight += "<p style =\"margin-left:10%;\">";

            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";

            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";

            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";

            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }

            //                        }

            //                    }
            //                }
            //            }

            //            if (item.MenuName == "Customer Registration")
            //            {
            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Obligor/Buyer/Supplier")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }

            //                        }

            //                        if (menuname == "Funder")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\">";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }

            //                        }

            //                        if (menuname == "Exporter")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }

            //                        }
            //                    }
            //                }
            //            }

            //            if (item.MenuName == "Service Provider Registration")
            //            {

            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Service Provider")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                 iCountIsMaker = 1;

            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                 iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                        }
            //                    }
            //                }
            //            }
            //            if (item.MenuName == "Sytem Parameters")
            //            {


            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Document Management")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                        }


            //                        if (menuname == "Fee Management")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }

            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                        }

            //                        if (menuname == "Template Management")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                        }
            //                    }
            //                }

            //            }
            //            if (item.MenuName == "Program Configuration")
            //            {


            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Program Config")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }

            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                        }
            //                    }
            //                }
            //            }

            //            if (item.MenuName == "Factoring Process")
            //            {


            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Factoring")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }
            //                            //userRight += " for <b>" + menuname + " </b></p>";
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            if (item.MenuName == "Payables Finance Process")
            //            {


            //                foreach (var subitem in MenuRole)
            //                {
            //                    if (item.MenuID == subitem.ParentMenuID)
            //                    {
            //                        string menuname = subitem.MenuName;
            //                        if (menuname == "Payables Finance")
            //                        {
            //                            int iCountIsMaker = 0;
            //                            int iCountIsChecker = 0;
            //                            int iCountIsApprover = 0;

            //                            userRight += "<p style=\"margin-left:10%;\" >";
            //                            if (subitem.IsMaker)
            //                            {
            //                                iCountIsMaker = 1;
            //                                //userRight += "'Maker'";
            //                            }
            //                            if (subitem.IsChecker)
            //                            {
            //                                iCountIsChecker = 1;
            //                                //userRight += "'Checker'";
            //                            }
            //                            if (subitem.IsApprover)
            //                            {
            //                                iCountIsApprover = 1;
            //                                //userRight += "'Approver'";
            //                            }
            //                            string getReturnString = string.Empty;
            //                            getReturnString = Function.GetuserRightComaand(iCountIsMaker, iCountIsChecker, iCountIsApprover);
            //                            if (!string.IsNullOrEmpty(getReturnString))
            //                            {
            //                                userRight += getReturnString + " for <b>" + menuname + " </b></p>";
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            //        }

            //    }
            //}

            //return userRight;
        }
        public static string GetuserRightComaand(int iCountIsMaker, int iCountIsChecker, int iCountIsApprover)
        {
            string sReturn = string.Empty;
            int iCount = 0;
            if (iCountIsMaker == 1)
            {
                iCount++;
            }
            if (iCountIsChecker == 1)
            {
                iCount++;
            }
            if (iCountIsApprover == 1)
            {
                iCount++;
            }

            if (iCount == 1)
            {
                if (iCountIsMaker == 1)
                {
                    sReturn += "'Maker'";
                }
                if (iCountIsChecker == 1)
                {
                    sReturn += "'Checker'";
                }
                if (iCountIsApprover == 1)
                {
                    sReturn += "'Approver'";
                }
            }
            if (iCount == 2)
            {
                if (iCountIsMaker == 1 && iCountIsChecker == 1)
                {
                    sReturn += "'Maker' and 'Checker'";
                }
                if (iCountIsChecker == 1 && iCountIsApprover == 1)
                {
                    sReturn += "'Checker' and 'Approver'";
                }
                if (iCountIsApprover == 1 && iCountIsMaker == 1)
                {
                    sReturn += "'Maker' and 'Approver'";
                }

            }
            if (iCount == 3)
            {
                sReturn += "'Maker' , 'Checker' and 'Approver'";

            }
            return sReturn;
        }
        public void SendOTPNumberandPassword(string password, string OTPNumber, string PhoneNumber)
        {
            string MessageText = string.Empty;
            ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
            var TempText = dbHT.HTMLTemplate_ListAll(0, "OTP_SMS", 1, "", false, "", 0).FirstOrDefault();
            MessageText = GetOTPText(TempText.HtmlText, OTPNumber);

            PhoneNumber = PhoneNumber.Replace("-", "").Trim();

            ClsSMSConfig obj = new ClsSMSConfig();
            var data = obj.SMSConfiguration_ListAll(1, false, "").FirstOrDefault();

            string url = string.Empty;
            string urlUserName = string.Empty;
            string urlPassword = string.Empty;
            string urlsource = string.Empty;
            url = data.URL.Trim();
            urlUserName = data.UserName.Trim();
            urlPassword = data.Pwd.Trim();
            // urlsource = data.urlsource.Trim();

            //13-Jul-2017
            //var urlstring = url + "?username=" + urlUserName + "&password=" + urlPassword + "&type=0&dlr=1&destination=" + PhoneNumber.Trim() + "&source=fghfghfhg&message=" + MessageText;

            //var urlstring = "http://ckjk.ae/app/smsapi/index.php?key=596782b472e5c&campaign=4773&routeid=39&type=text&contacts=" + PhoneNumber.Trim() + "&senderid=C2004066&msg=" + MessageText;

            //var Data = "http://kjlkj/bulksms/bulksms?username=vers-demo&password=india123&type=0&dlr=1&destination=971508185870&source=demo&message=" + OTPMessageString;
            //var urlstring = "http://jhj.hkj.ae/app/smsapi/index.php?key=596782b472e5c&campaign=4773&routeid=39&type=text&contacts=" + PhoneNumber.Trim() + "&senderid=yuytyut&msg=&msg=" + MessageText + "";
            var urlstring="";
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(urlstring);
            HttpWebResponse hr = (HttpWebResponse)wr.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(hr.GetResponseStream());
            String str = sr.ReadToEnd();
            sr.Close();
            hr.Close();

        }
        public string GetText(DataRow dr, string msg)
        {
            string strSMS = msg;// Message Text

            while (strSMS.Contains("{"))
            {
                string ColumnName = strSMS.Substring(strSMS.IndexOf("{") + 1, strSMS.IndexOf("}") - strSMS.IndexOf("{") - 1);
                var a = ColumnName;

                if (dr.Table.Columns.Contains(ColumnName) == true)
                {
                    ColumnName = RemoveBetween(ColumnName, '<', '>');
                    ColumnName = ColumnName.Replace("&nbsp;", " ");
                    strSMS = strSMS.Replace(a, ColumnName);
                    strSMS = strSMS.Replace("{" + ColumnName + "}", GetPlainText(dr[ColumnName], dr.Table.Columns[ColumnName].DataType.ToString()).Trim());
                }
                else
                {
                    break;
                }
            }
            return strSMS;
        }
        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }
        public string CreateRandomOTPNumber(int OTPLength)
        {
            string _allowedChars = "0123456789";
            //string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[OTPLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < OTPLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public string UserRoletatus()
        {

            string Roletatus = string.Empty;
            Roletatus = "NA1,UR1,UM1,UC1,UA1,UJ1,DR1,DC1,DM1,AR1,AC1,AM1,AA1,IR1,IM1,IC1";
            return Roletatus;
        }
        public int GetPasswordExpriyDay(DateTime date)
        {
            int rday = 0;
            DateTime dCurrenDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
            DateTime dEndDate = Convert.ToDateTime(date.ToString("MM/dd/yyyy hh:mm:ss tt"));
            System.TimeSpan dayCount = (dCurrenDate - dEndDate);
            string day = Convert.ToString(dayCount.Days);
            //if (day.Length == 0) { day = "00"; }
            //else if (day.Length == 1) { day = "0" + day; }
            //else if (day.Length == 2) { if (Convert.ToInt32(day) < 0) { day = "00"; } }
            //else if (day.Length == 3) { if (Convert.ToInt32(day) < 0) { day = "00"; }; }
            //else { if (Convert.ToInt32(day) < 0) { day = "00"; } }

            int.TryParse(day, out rday);
            return rday;
        }
        public static string StringLength(string stringchar, int charLen)
        {
            string stringcharlenth = string.Empty;
            if (stringcharlenth == null)
            {
                stringcharlenth = "";
            }
            if (stringchar == null)
            {
                stringchar = "";
            }
            if (stringchar.Length > charLen)
                stringcharlenth = stringchar.Substring(0, charLen) + "...";
            else
                stringcharlenth = stringchar;
            return stringcharlenth;
        }
        public string GetTempMsg(string tempName, DataTable DT)
        {
            string MessageText = string.Empty;
            HTMLTemplate_ListAll_Result _objTemplate = new HTMLTemplate_ListAll_Result();
            ClsHTMLTemplate _clsTemp = new ClsHTMLTemplate();
            _objTemplate.HtmlTemplateID = 0;
            _objTemplate.HtmlName = tempName;
            _objTemplate = _clsTemp.HTMLTemplate_ListAll(_objTemplate.HtmlTemplateID, tempName, 1, "", false, "", 0).FirstOrDefault();
            string ViewSample = "";
            if (DT == null)
            {
                ViewSample = _objTemplate.HtmlText;
            }
            if (_objTemplate != null && DT != null)
            {
                MessageText = _objTemplate.HtmlText;
                foreach (DataRow dr in DT.Rows)
                {
                    ViewSample = GetText(dr, MessageText);
                }
            }
            return ViewSample;
        }
        public string GetSMS(string tempName, DataTable DT, string PhoneNumber)
        {
            string MessageText = string.Empty;
            HTMLTemplate_ListAll_Result _objTemplate = new HTMLTemplate_ListAll_Result();
            ClsHTMLTemplate _clsTemp = new ClsHTMLTemplate();
            _objTemplate.HtmlTemplateID = 0;
            _objTemplate.HtmlName = tempName;
            _objTemplate = _clsTemp.HTMLTemplate_ListAll(_objTemplate.HtmlTemplateID, tempName, 1, "", false, "", 0).FirstOrDefault();
            string ViewSample = "";
            if (_objTemplate != null)
            {
                MessageText = _objTemplate.HtmlText;
                foreach (DataRow dr in DT.Rows)
                {
                    ViewSample = GetText(dr, MessageText);
                }
            }
            SendSMS(PhoneNumber, ViewSample);
            return ViewSample;
        }
        public void SendSMS(string PhoneNumber, string MessageText)
        {
            ClsEmailConfiguration objEmailconf = new ClsEmailConfiguration();
            CountryMaster EmailConfiList = new CountryMaster();
            int intLogID = 0;
            int retval = 0;
            objEmailconf.conn = ClsAppDatabase.GetCon();
            if (objEmailconf.conn.State == ConnectionState.Closed)
            { objEmailconf.conn.Open(); }
            else { objEmailconf.conn.Close(); objEmailconf.conn.Open(); }
            objEmailconf.tras = objEmailconf.conn.BeginTransaction();

            try
            {
                ObjectParameter pLogID = new ObjectParameter("pLogID", typeof(int));
                intLogID = objEmailconf.EmailSMSLogDetail_Add(pLogID, 1, 1, 1, "", PhoneNumber, MessageText);
                objEmailconf.tras.Commit();
                objEmailconf.conn.Close();
            }
            catch (Exception ex)
            { objEmailconf.tras.Rollback(); objEmailconf.conn.Close(); }

            PhoneNumber = PhoneNumber.Replace("+", "").Trim();
            //var urlstring = "http://customers.smsmarketing.ae/app/smsapi/index.php?key=59e3bc4928e1f&campaign=5450&routeid=39&type=text&contacts="+PhoneNumber.Trim()+"&senderid=800sjhfsdj&msg="+MessageText+"";
            var urlstring = "http://customers.smsmarketing.ae/app/smsapi/index.php?key=596782b472e5c&campaign=4773&routeid=39&type=text&contacts=" + PhoneNumber.Trim() + "&senderid=800djsfdsk&msg=&msg=" + MessageText + "";
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(urlstring);
            HttpWebResponse hr = (HttpWebResponse)wr.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(hr.GetResponseStream());
            String str = sr.ReadToEnd();
            sr.Close();
            hr.Close();
            retval = 1;
            if (retval == 1)
            {
                try
                {
                    objEmailconf.conn = ClsAppDatabase.GetCon();
                    if (objEmailconf.conn.State == ConnectionState.Closed)
                    { objEmailconf.conn.Open(); }
                    else { objEmailconf.conn.Close(); objEmailconf.conn.Open(); }
                    objEmailconf.tras = objEmailconf.conn.BeginTransaction();

                    objEmailconf.EmailSMSLogDetail_Update(intLogID, true, "Sucessfull");
                    objEmailconf.tras.Commit();
                    objEmailconf.conn.Close();
                }
                catch (Exception ex)
                { objEmailconf.tras.Rollback(); objEmailconf.conn.Close(); }
            }
        }
        public string ChangeDate(string date)
        {
            string tmpDate = date.Substring(0, 10);
            string temp = "";
            temp = tmpDate.Substring(0, 2);
            switch (Convert.ToInt32(tmpDate.Substring(3, 2)))
            {
                case 1:
                    temp = temp + "-JAN-";
                    break;
                case 2:
                    temp = temp + "-FEB-";
                    break;
                case 3:
                    temp = temp + "-MAR-";
                    break;
                case 4:
                    temp = temp + "-APR-";
                    break;
                case 5:
                    temp = temp + "-MAY-";
                    break;
                case 6:
                    temp = temp + "-JUN-";
                    break;
                case 7:
                    temp = temp + "-JUL-";
                    break;
                case 8:
                    temp = temp + "-AUG-";
                    break;
                case 9:
                    temp = temp + "-SEP-";
                    break;
                case 10:
                    temp = temp + "-OCT-";
                    break;
                case 11:
                    temp = temp + "-NOV-";
                    break;
                case 12:
                    temp = temp + "-DEC-";
                    break;
            }
            temp = temp + tmpDate.Substring(6, 4);  // 01/01/2015
            return temp.ToString();
        }
        public string RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            string strErrorMessage = "";

            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }
            foreach (DataRow dRow in duplicateList)
            {
                strErrorMessage = colName + " value are duplicate.";
            }
            return strErrorMessage;
        }
    }
}
