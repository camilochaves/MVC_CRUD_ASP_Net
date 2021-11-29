using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{
    public partial class UnitTest_DLL_SwissKnife
    {
        [TestMethod]
        [DataRow("variable", "value")]
        public void Secrets_GetFromEnvironment_MustReturnTrue(string variable, string inputValue)
        {
            Environment.SetEnvironmentVariable(variable, inputValue);
            var envValue = SecretsHandlerService.GetFromEnv(variable);
            Assert.IsTrue(envValue == inputValue);
        }

        public class Nested
        {
            public string Key1 { get; set; }
            public string Key2 { get; set; }
        };

        [TestMethod]
        public void Secrets_ConfigurationGetObject_MustReturnTrue()
        {
            //Arrange           
            var myCustomCollection = new Dictionary<string, string>
            {
                {"NestedInMemory:Key1", "NestedValue1"},
                {"NestedInMemory:Key2", "NestedValue2"}
            };

            var directory = AppDomain.CurrentDomain.BaseDirectory;

            IConfiguration configuration = new ConfigurationBuilder()
                                .SetBasePath(directory)
                                .AddInMemoryCollection(myCustomCollection)
                                .AddJsonFile("settingsTest.json", false)
                                .Build();
            var secretsHandler = new SecretsHandlerService(configuration);

            //Act
            var key1InMemory = secretsHandler.GetFromConfig("NestedInMemory:Key1");
            var key1InJson = secretsHandler.GetFromConfig("NestedJson:key1");
            var resultInJson1 = secretsHandler.GetFromConfig("CustomKey");
            var resultInJson2 = secretsHandler.GetObject<Nested>("NestedJson");

            //Assert
            Assert.IsTrue(resultInJson1 == "CustomValue");
            Assert.IsTrue(key1InMemory == "NestedValue1");
            Assert.IsTrue(key1InJson == "NestedValue1");
            Assert.IsTrue(resultInJson2.Key1 == "NestedValue1");
            Assert.IsTrue(resultInJson2.Key2 == "NestedValue2");
        }
    }
}