using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{
    [TestClass]
    public partial class UnitTest_DLL_SwissKnife
    {
        
        [TestMethod]
        [DataRow("Test String")]
        [DataRow("Any gibberish qoh3rfoh $4259798")]
        public void SwissKnife_Encode64Decode64Tests_MustBeTrue(string str)
        {
            //Act
            var test = str.EncodeTo64();
            var result = test.DecodeFrom64();

            //Arrange
            Assert.IsTrue(str==result);
        }

        [TestMethod]
        [DataRow("Test String")]
        [DataRow("Any gibberish qoh3rfoh $4259798")]
        public void SwissKnife_AESEncryptDecrypt_MustBeTrue(string str)
        {
            //Arrange
            var password = "Some_Long_And_Obscure_Password";
            var salt = "u298348293847983749827359872498efwefwefewfwe";
            //Act
            var test = str.EncryptAES256(password,salt);
            var result = test.DecryptAES256(password, salt);

            //Arrange
            Assert.IsTrue(str==result);
        }

        [TestMethod]
        [DataRow(10)] //length of the password
        [DataRow(20)]
        public void SwissKnife_CreateRandomEncryptedPassword_MustReturnAString(int length)
        {
            //Arrange
           
            //Act
            var randomEncryptedPassword = RandomPassword.Generate(length);

            //Arrange
            Assert.IsTrue(randomEncryptedPassword is not null);
        }

        [TestMethod]
        public void SwissKnife_HashPasswords_MustReturnUniqueHash()
        {
            //Arrange
            var pass1 = "password1";
            var pass2 = "password2";
           
            //Act
            var hashA = pass1.CreateHash("This_is_a_unique_secret_Salt");
            var hashB = pass1.CreateHash("This_is_a_unique_secret_Salt");
            var hashC = pass2.CreateHash("This_is_another_unique_secret_Salt");

            //Arrange
            Assert.IsTrue(hashA==hashB);
            Assert.IsTrue(hashA!=hashC);
            Assert.IsTrue(hashB!=hashC);
        }

    }
}