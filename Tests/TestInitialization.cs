using System;
using System.Linq;
using System.Net.Http;
using Application.InputModels;
using Infra.EFCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReasonSystems.DLL.SwissKnife;

namespace Tests
{
  [TestClass]
  public partial class IntegrationTestsDLLApiJson
  {
    private WebApplicationFactory<ApplicationWebMVC.Startup>? _factory;
    private HttpClient? _client;
    private static readonly string _serverAddr = "https://localhost:5001";
    private static int testNumber = 0;

    private SecretsHandlerService? _secretsHandler;
    private static readonly EmployeeInputModel testEmployee = new
    (
        firstName: "Camilo",
        lastName: "Chaves",
        email: "chaves.camilo@gmail.com",
        password: "123",
        confirmPassword: "123",
        employeeNumber: 13456
    );

    //Constructor
    public IntegrationTestsDLLApiJson()
    {
      this._factory = new WebApplicationFactory<ApplicationWebMVC.Startup>()
        .WithWebHostBuilder(builder =>
        {
          builder.UseSetting("https_port", "5001").UseEnvironment("Testing");
           //Tests will be done with InMemory DATABASE
           //Therefore, I have to remove the current DbContext
           //because it points to a MYSQL CONNECTION
           builder.ConfigureServices(services =>
                 {
                   var descriptor = services.SingleOrDefault(
                                    d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));
                   if (descriptor is not null) services.Remove(item: descriptor);
                   services.AddDbContextPool<ApplicationContext>(options =>
                               {
                                 options.UseInMemoryDatabase($"InMemoryDbForTesting{testNumber}");
                                 //In Memory Dbs leak between tests unless they have a different name
                               });
                 }
             );
          builder.UseKestrel();
        });
        testNumber++; 
      this._factory.Server.BaseAddress = new Uri(_serverAddr);
      this._client = _factory?.CreateClient();
      using var scope = _factory?.Services.CreateScope();
      //I will need a secretsHandler service for later therefore will instantiate one from services
      this._secretsHandler = scope?.ServiceProvider.GetRequiredService<SecretsHandlerService>();

    }

    //Destructor
    ~IntegrationTestsDLLApiJson()
    {
      this._factory?.Dispose();
      this._client?.Dispose();
      this._secretsHandler = null;
    }

    [ClassInitialize]
    public static void ClassInit(TestContext context)
    {

    }

    [TestInitialize]
    public void TestInitialization_PreConditionsAsync()
    {
    }

    [TestCleanup]
    public void TestCleanUp_PostAssertions()
    {
    }

    [ClassCleanup]
    public static void CleanClass()
    {

    }

  }
}