using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{

  public partial class IntegrationTestsDLLApiJson
  {
    [TestMethod]
    public async Task AccessController_LoginRegisteredUser_MustReturnIdToken()
    {
      //Arrange
      await AddEmployee(testEmployee);
      //Act        
      var id_token = await LoginAndReturnIdToken(testEmployee.Email, testEmployee.Password);
      var isTokenValid = TokenTools.ValidateIdToken(id_token,
                                                   _secretsHandler?.GetFromConfig("JWT_SecretKey"));
      //Assert
      Assert.IsTrue(isTokenValid);
    }

    [TestMethod]
    public async Task AccessController_RequestAccessToken_MustReturnValidAccessToken()
    {
      //Arrange
      await AddEmployee(testEmployee);
      //Act
      string access_token = await LoginAndReturnAccessToken(testEmployee.Email, testEmployee.Password);
      //Assert
      Assert.IsTrue(access_token!="");
    }

  }
}