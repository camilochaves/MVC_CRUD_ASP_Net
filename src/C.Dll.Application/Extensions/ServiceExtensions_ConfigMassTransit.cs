using System;
using Application.Services;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

/*
MassTransit Transports: RabbitMq, Azure Service Bus, ActiveMq, Amazon Sqs, In Memory
Resources: Concurrency, Connection Management, Monitoring, Manage Consumer Lifecycle
          Exception Treatment, Re-Readings, Scheduling of Messages, Ease Testing
Message Types: Events and Commands
Events: Published by IBus or ConsumeContext. Cannot be send directly to an endpoint
Commands: Sent using Send to an Endpoint. Commands can never be published.
Consumers: Standard, Sagas, State Machine, Job Consumers
Producers: Producers can send or publish a message. 
     Sent messages are seen as commands because they are sent to a specific endpoint
     Published messages are events because they are broadcasted to any subscribed consumer

Note: Every type of message must implement IConsumer<T> where T is the Entity to send
Most used interfaces and resources:
IBusControl - Start and Stop the bus
IBus - Publish/Send messages
ISendEndpointProvider - Send messages based on consumer dependencies
IPublishEndpoint - Publish messages based on consumer dependencies
MassTransitHostedService - Start/Stop automatically the Bus
*/

namespace Application.Extensions
{
    public static partial class ServiceExtentions
    {
        public static IServiceCollection ConfigMassTransit(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddBus(provider =>
                {
                    return Bus.Factory.CreateUsingRabbitMq(config =>
                   {
                       config.Host(new Uri("rabbitmq://localhost"), h =>
                       {
                           h.Username("guest");
                           h.Password("guest");
                       });
                   });
                });

            });
            services.AddMassTransitHostedService();
            services.AddScoped<MassTransitRabbitMqService>(); //My Custom Service

            return services;
        }
    }
}