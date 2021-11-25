using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ReasonSystems.DLL.SwissKnife
{
    public static partial class TokenTools
    {
        private const string _alg = "HmacSHA256";
        public static string CreateAccessToken(string username, string secretKey, string salt, string ip, string userAgent, int minutesValid = 10)
        {
            string textToEncrypt = string.Join("%", new string[] { username, ip, userAgent });
            using HMAC hmac = HMACSHA256.Create(_alg);
            hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(secretKey, salt));
            hmac.ComputeHash(Encoding.UTF8.GetBytes(textToEncrypt));
            string hashedCode = Convert.ToBase64String(hmac.Hash);
            string hashedParams = string.Join("%", new string[] { username, DateTime.UtcNow.AddMinutes(minutesValid).ToString() });
            return string.Join("%", hashedCode, hashedParams).EncodeTo64();
        }

        public static string GetHashedPassword(string password, string salt)
        {
            string passwordToEncrypt = string.Join("%", new string[] { password, salt });

            using HMAC hmac = HMAC.Create(_alg);
            // Hash the key.
            hmac.Key = Encoding.UTF8.GetBytes(salt);
            hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordToEncrypt));
            return Convert.ToBase64String(hmac.Hash);
        }

        public static string[] DecodeAccessTokenAndSeparateParts(string token)
        {
            string key = token.DecodeFrom64();
            // Split the parts.
            string[] parts = key.Split(new char[] { '%' });
            return parts;
        }

        //Still to be implemented
        //private const int _expirationMinutes = 10;
        public static bool IsValidAccessToken(string access_token, string ip, string userAgent, string secretKey, string salt)
        {
            bool result = false;
            try
            {
                string[] parts = DecodeAccessTokenAndSeparateParts(access_token);
                if (parts.Length != 3) return false;

                // Get the hash message, username, and timestamp.
                string hashedAccessToken = parts[0];
                string user = parts[1];
                string validUntil = parts[2];
                DateTime validUntilTime = DateTime.Parse(validUntil, styles: System.Globalization.DateTimeStyles.AdjustToUniversal);
                             
                //1st test: Hashed content must match when created from userName, IP and userAgent
                var computedToken = CreateAccessToken(user, secretKey, salt, ip, userAgent);
                var hashedComputedToken = DecodeAccessTokenAndSeparateParts(computedToken)[0];
                if(hashedAccessToken != hashedComputedToken) return false;

                //2nd test: Time constraint
                if(validUntilTime.CompareTo(DateTime.UtcNow)<0) return false;
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}