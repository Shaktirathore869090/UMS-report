using System;
using System.Security;

namespace UMSReportsAPI.Security
{
    [SecurityCritical] // Prevents reflection-based access
    public class SecureService
    {
        private void SecretMethod()
        {
            Console.WriteLine("This is a protected method.");
        }
    }
}