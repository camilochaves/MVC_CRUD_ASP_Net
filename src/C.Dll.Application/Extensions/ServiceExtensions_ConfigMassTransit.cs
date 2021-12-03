using System;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static partial class ServiceExtentions
    {
        public static IServiceCollection ConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                //x.AddConsumer<TodoConsumer>();
                x.AddBus(provider =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(config =>
                   {
                       config.Host(new Uri("rabbitmq://localhost"), h =>
                       {
                           h.Username("Camilo");
                           h.Password("123");
                       });
                    //    config.ReceiveEndpoint("todoQueue", ep =>
                    //     {
                    //         ep.PrefetchCount = 16;
                    //         ep.UseMessageRetry(r => r.Interval(2, 100));
                    //         ep.ConfigureConsumer<TodoConsumer>(provider);
                    //     });
                   });
                });

            });
            services.AddMassTransitHostedService();

            return services;
        }
    }
}