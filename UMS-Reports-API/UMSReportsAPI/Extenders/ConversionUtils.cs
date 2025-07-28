using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UMSReportsAPI.Extenders
{
    public static class ConversionUtils
    {
        public static string EncodeToBase64(string userId, string password)
        {
            string credentials = userId + ":" + password;
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            return Convert.ToBase64String(textBytes);
        }

        public static (string userId, string password) DecodeBase64Credentials(string base64Credentials)
        {
            string credentials = base64Credentials.Base64Decode();
            string[] parts = credentials.Split(':');

            if (parts.Length == 2)
            {
                string userId = parts[0];
                string password = parts[1];
                return (userId, password);
            }
            else
            {
                throw new ArgumentException("Invalid credentials format.");
            }
        }
    }
}