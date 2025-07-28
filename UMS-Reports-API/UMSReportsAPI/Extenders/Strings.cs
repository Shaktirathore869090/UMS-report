using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.IO;

namespace UMSReportsAPI.Extenders
{
    public static class Strings
    {
        #region String Extensions

        public static string Encrypt(this string plainText, string publicKeyFilePath = "")
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            if (publicKeyFilePath.IsNullOrWhiteSpace())
                publicKeyFilePath = WebSettings.ReadKey("PublicKeyPath");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(File.ReadAllBytes(publicKeyFilePath));
                byte[] encryptedBytes = rsa.Encrypt(plainTextBytes, true);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static string Decrypt(this string encryptedText, string privateKeyFilePath = "")
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            if (privateKeyFilePath.IsNullOrWhiteSpace())
                privateKeyFilePath = WebSettings.ReadKey("PrivateKeyPath");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(File.ReadAllBytes(privateKeyFilePath));
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public static RSAParameters GenerateRSAKeyPair()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                return rsa.ExportParameters(true);
            }
        }

        public static string MD5(this string value)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Last4Digits(this string value)
        {
            string _Result = "";
            try
            {
                string _tmp = "";

                if (value.IsEncrypted())
                    _tmp = value.Decrypt();
                else
                    _tmp = value;

                _Result = (_tmp.Length > 4 ? _tmp.Substring(_tmp.Length - 4) : _tmp);
            }
            catch { }
            return _Result;
        }

        public static bool IsEncrypted(this string text)
        {
            // Check for common patterns indicating encryption (e.g., Base64 encoding)
            if (Regex.IsMatch(text, @"^[A-Za-z0-9+/]*={0,2}$"))
            {
                try
                {
                    Convert.FromBase64String(text);
                    return true;
                }
                catch (FormatException)
                {
                    // Invalid Base64 format
                    return false;
                }
            }

            // Add more pattern checks here if desired

            return false;
        }

        public static string PrepareNotes(this string value, string UserName)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > 0)
                return DateTime.Now.ToString() + " (" + UserName + ") : " + value;
            else
                return value;
        }

        public static long ToLong(this string value)
        {
            long _Result = 0;
            try
            {
                _Result = Convert.ToInt64(value);
            }
            catch { }

            return _Result;
        }

        public static string MaskCreditCard(this string value)
        {
            string _Result = "";
            if (value.Length > 4)
                _Result = new string('X', value.Length - 4) + "-" + value.Substring(value.Length - 4);
            return _Result;
        }

        public static string Coalesce(this string value)
        {
            string _Result = "";
            ////if (!Convert.IsDBNull(value) && value != null)
            ////    _Result = value;

            if (!value.IsNullOrWhiteSpace())
                _Result = value;

            return _Result;
        }

        public static string Coalesce(this string value, string _default)
        {
            string _Result = _default;
            ////if (!Convert.IsDBNull(value) && value != null)
            ////    _Result = value;

            if (!value.IsNullOrWhiteSpace())
                _Result = value;

            return _Result;
        }
        public static string Coalesce(this object value)
        {
            return value.Coalesce("");
        }
        public static string Coalesce(this object value, string _default)
        {
            if (value == null)
                return _default;
            string _result = _default;
            try
            {
                _result = value.ToString();
            }
            catch { }
            return _result;
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            else if (Convert.IsDBNull(value))
                return true;
            else if (value.Trim().Length == 0)
                return true;
            else
                return false;
        }

        public static bool IsNumber(this string value)
        {
            double Num;
            return double.TryParse(value, out Num);
        }

        public static string RemoveCurrencySymbols(this string value)
        {
            //value = value.Replace("(", "-");
            //value = value.Replace(")", "");
            //value = value.Replace("$", "");
            //value = value.Replace("-", "");
            //if (value.Length == 0)
            //    value = "0";
            bool isNegative = value.IndexOf('(') >= 0 || value.IndexOf('-') >= 0;
            char theCharacter = '.';
            var getNumbers = (from t in value
                              where char.IsDigit(t) || t.Equals(theCharacter)
                              select t).ToArray();
            var _str = string.Empty;
            foreach (var item in getNumbers)
            {
                _str += item.ToString();
            }
            value = (isNegative ? "-" : "") + _str;
            return value;
        }

        public static string RemoveSpecialCharsFromPhone(this string value)
        {
            value = value.Replace("(", "");
            value = value.Replace(")", "");
            value = value.Replace("$", "");
            value = value.Replace("-", "").Replace("#", "").Replace(".", "");
            if (value.Length == 0)
                value = "";
            return value;
        }

        public static string Concat(this string[] value)
        {
            string _result = "";
            for (int i = 0; i < value.Length; i++)
            {
                _result += value[i];
                if (i < value.Length - 1)
                    _result += Environment.NewLine;
            }
            return _result;
        }

        public static int ToInt32(this string value)
        {
            int _result = 0;
            try
            {
                _result = Convert.ToInt32(value);
            }
            catch { }
            return _result;
        }
        public static decimal ToDecimal(this string value)
        {
            decimal _result = 0;
            try
            {
                _result = Convert.ToDecimal(value);
            }
            catch { }
            return _result;
        }
        public static bool ToBoolean(this string value)
        {
            bool _result = false;
            value = value.Coalesce("").ToUpper();
            _result = value.Equals("1") || value.Equals("TRUE") || value.Equals("YES");
            return _result;
        }
        public static bool ToBoolean(this object value)
        {
            bool _result = false;
            try
            {
                string str = value.ToString();
                return str.ToBoolean();
            }
            catch { }
            return _result;
        }

        public static List<string> ToCSV(this string value)
        {
            List<string> result = value.Split(',').ToList();
            result = result.Select(x => x.Trim()).ToList();
            return result;
        }

        public static List<string> ToCSV(this string value, char delimintingCharactor)
        {
            return value.Split(delimintingCharactor).ToList();
        }

        public static bool In(this string value, List<string> values)
        {
            return values.Contains(value);
        }

        public static string ConvertMMYY(this string value)
        {
            if (value.Length < 4)
                throw new Exception("Invalid CC Expiry date");
            if (!value.Contains("/") && value.Length == 4)
                return value;

            string[] date = value.Split('/');
            string _result = "";
            if (date[0].Length == 1)
                _result = "0" + date[0];
            else
                _result = date[0];

            if (date[1].Length == 2)
                _result += date[1];
            else if (date[1].Length == 4)
                _result += date[1].Substring(2, 2);
            else
                return "";

            return _result;
        }
        public static string SafeGetString(this SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        public static string ToTitleCase(this string str, TitleCase tcase = TitleCase.All)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }

        public enum TitleCase
        {
            First,
            All
        }


        #endregion
        #region Email

        public static bool IsValidEmail(this string emailID)
        {
            return new EmailAddressAttribute().IsValid(emailID);
        }
        #endregion


        public static bool IsCharacter(this string value)
        {

            if (Regex.IsMatch(value, @"^[a-zA-Z\s -]+$"))
            {
                return true;
            }
            else
                return false;
        }

        public static string Base64Encode(this string text)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }


        // Helper method to decode Base64 to plain text (from the previous response)
        public static string Base64Decode(this string base64Text)
        {
            byte[] base64Bytes = Convert.FromBase64String(base64Text);
            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }

        public static string RepeatForLeftPadding(this string chars, int count)
        {
            try
            {
                string tmpStr = "";
                if (count > 0)
                {
                    if (string.IsNullOrWhiteSpace(chars))
                    {
                        tmpStr = AddSpacesToLeft(count);
                    }
                    else
                    {
                        for (int i = 1; i <= count; i++)
                        {
                            tmpStr += chars;
                        }
                    }
                }
                return tmpStr;
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine("Error: " + ex.Message);
                return ""; // Return a default value or handle the error
            }
        }
        private static string AddSpacesToLeft(int count)
        {
            return new string(' ', count);
        }

        public static string TrimRightPadding(this string value, char paddingCharacter = ' ', int maxLength = 0)
        {
            string result = value + new string(paddingCharacter, maxLength);
            return result.Substring(0, maxLength);
        }

        public static string SubstringWithCorrection(this string value, int startIndex = 0, int length = -1, bool trimWhitespace = false)
        {
            if (length == -1)
                length = value.Length;
            if (trimWhitespace)
                return (value + new string(' ', length)).Substring(startIndex, length).Trim();
            else
                if ((value + new string(' ', length)).Length < length + startIndex)
                return (value + new string(' ', length)).Substring(startIndex);
            else
                return (value + new string(' ', length)).Substring(startIndex, length);

        }
    }
}