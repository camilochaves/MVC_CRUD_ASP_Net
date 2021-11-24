using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{
    public partial class UnitTest_DLL_SwissKnife
    {
        [TestMethod]
        [DataRow("variable","value")]
        public void Secrets_GetFromEnvironment_MustReturnTrue(string variable, string inputValue)
        {
            Environment.SetEnvironmentVariable(variable, inputValue);
            var envValue = SecretsHandlerService.GetFromEnv(variable);
            Assert.IsTrue(envValue == inputValue);
        }
    }
}