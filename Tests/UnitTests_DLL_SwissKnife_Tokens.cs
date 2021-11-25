using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{
    public partial class UnitTest_DLL_SwissKnife
    {

        [TestMethod]
        [DataRow("username", "SomeLongSecretKey", "SomeLongSaltWithAtLeast8Bits", "ip", "useragent",1, 1)]
        [DataRow("username", "SomeLongSecretKey", "SomeLongSaltWithAtLeast8Bits", "ip", "useragent",0, 2)]
        public void SwissKnife_CreateAndValidateAccessToken(
            string username,
            string secretkey,
            string salt,
            string ip,
            string userAgent,
            int validMinutes,
            int pass)
        {
            //Act

            //Arrange
            var access_token = TokenTools.CreateAccessToken(username, secretkey, salt, ip, userAgent, validMinutes);
            var isValidAccessToken = TokenTools.IsValidAccessToken(access_token, ip, userAgent, secretkey, salt);
            //Assert
            if (pass==1) Assert.IsTrue(isValidAccessToken);
            if (pass==2) Assert.IsFalse(isValidAccessToken);            
        }

        [TestMethod]
        [DataRow("username", "SomeLongSecretKey", "Reason Systems")]
        [DataRow("chaves.camilo@gmail.com", "SomeLongSecretKey", "ZUP")]
        public void SwissKnife_CreateAndValidateIDToken(
           string username, string secretkey, string issuer
       )
        {
            //Act
            var userIdentity = new GenericIdentity(username);
            var claimsIdentity = new ClaimsIdentity(userIdentity);
            var privateTokenKey = Encoding.ASCII.GetBytes(secretkey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(privateTokenKey), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow
            };

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateAudience = false,
                IssuerSigningKey =
                         new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey)),
                ClockSkew = TimeSpan.Zero // remove delay of token when expire
            };

            //Arrange
            var jwt_id_token = TokenTools.CreateIdToken(claimsIdentity, secretkey, issuer, tokenDescriptor);
            var isValidIdToken = TokenTools.ValidateIdToken(jwt_id_token, secretkey, tokenValidationParameters);

            //Assert
            Assert.IsTrue(isValidIdToken);
        }

    }
}