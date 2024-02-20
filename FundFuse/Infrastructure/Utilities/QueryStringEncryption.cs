using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using SIW = TMP.Infrastructure.Web;

namespace TMP.Infrastructure.Utilities
{
    public class QueryStringEncryption : IHttpModule
    {

        #region Variables & properties

        public const string PARAMETER_NAME = "qs=";
        private const string ENCRYPTION_KEY = "DaTScF";
        private static bool _IsEnabled = false;
        public static bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
        }

        #endregion

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            _IsEnabled = true;
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        #endregion

        #region Local methods

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (SIW.Common.IsAjaxRequest)
            {
                return;
            }
            if (context.Request.RawUrl.Contains("?"))
            {
                string path = GetVirtualPath();
                if (!string.IsNullOrEmpty(path))
                {
                    string query = ExtractQuery(context.Request.RawUrl);
                    if (query.StartsWith(PARAMETER_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        // Decrypts the query string and rewrites the path.
                        string rawQuery = query.Replace(PARAMETER_NAME, string.Empty);
                        string decryptedQuery = Decrypt(HttpUtility.UrlDecode(rawQuery));
                        context.RewritePath(path, string.Empty, decryptedQuery);
                    }
                    else if(context.Request.HttpMethod == "GET")
                    {
                        //Add some random param into encrypted value looks different for every request
                        query = RandomKey() + "&" + query;
                        // Encrypt the query string and redirects to the encrypted URL.
                        string encryptedQuery = Encrypt(query);
                        context.Response.Redirect(path + "?" + encryptedQuery);
                    }
                }
            }
        }

        private static string RandomKey()
        {
            Random _rndmGenerator = new Random();
            return "rm=" + _rndmGenerator.Next(0, 1000000).ToString("D6");
        }

        private static string GetVirtualPath(string url = "")
        {
            string path = string.IsNullOrEmpty(url) ? HttpContext.Current.Request.RawUrl : url;
            int _qsIndex = path.IndexOf("?");
            if (_qsIndex > -1)
            {
                path = path.Substring(0, _qsIndex);
            }
            return path;
        }

        private static string ExtractQuery(string url)
        {
            int index = url.IndexOf("?") + 1;
            return url.Substring(index);
        }

        public static string EncryptUrl(string url)
        {
            if (!IsEnabled)
            {
                return url;
            }
            string _encryptedUrl = url;
            if (!string.IsNullOrEmpty(_encryptedUrl) && _encryptedUrl.Contains("?"))
            {
                string _path = GetVirtualPath(url);
                if (!string.IsNullOrEmpty(_path))
                {
                    string _query = RandomKey() + "&" + ExtractQuery(url);
                    _encryptedUrl = _path + Encrypt(_query);
                }
            }
            return _encryptedUrl;
        }

        public static string EncryptQuery(string query)
        {
            if (!IsEnabled)
            {
                return query;
            }
            string _encryptedQuery = query;
            if (!string.IsNullOrEmpty(_encryptedQuery))
            {
                _encryptedQuery = Encrypt(RandomKey() + "&" + _encryptedQuery);
            }
            return _encryptedQuery;
        }

        #endregion 

        #region Encryption/Decryption

        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());

        private static string EncryptionKey
        {
            get
            {
                return ENCRYPTION_KEY;
            }
        }

        private static byte[] NewSalt
        {
            get
            {
                return Encoding.ASCII.GetBytes(EncryptionKey.Length.ToString());
            }
        }

        public static string Encrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(EncryptionKey, NewSalt);

            using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        return PARAMETER_NAME + Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            string _dataToDecrypt = inputText.Replace(" ", "+");
            if (_dataToDecrypt.Length % 4 > 0)
            {
                _dataToDecrypt = _dataToDecrypt.PadRight(_dataToDecrypt.Length + 4 - _dataToDecrypt.Length % 4, '=');
            }
            byte[] encryptedData = Convert.FromBase64String(_dataToDecrypt);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(EncryptionKey, NewSalt);

            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        }

        #endregion

    }
}