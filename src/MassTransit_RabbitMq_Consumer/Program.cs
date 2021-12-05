using Consumer;
using GreenPipes;
using MassTransit;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(x =>
          {
              x.AddConsumer<MessageConsumer>();
              x.AddBus(provider =>
            {
                return Bus.Factory.CreateUsingRabbitMq(config =>
               {
                   config.Host(new Uri("rabbitmq://localhost"), h =>
                   {
                       h.Username("guest");
                       h.Password("guest");
                   });
                   config.ReceiveEndpoint("myChannel", ep =>
                   {
                       ep.PrefetchCount = 10;
                       ep.UseMessageRetry(r => r.Interval(2, 100));
                       ep.ConfigureConsumer<MessageConsumer>(provider);
                   });
               });
            });

          });
        services.AddMassTransitHostedService();

    })
    .Build();

await host.RunAsync();
