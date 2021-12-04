using System;
using System.Threading.Tasks;
using MassTransit;

namespace Application.Services
{
    public class MassTransitRabbitMqService
    {
        private readonly IBus _bus;

        public MassTransitRabbitMqService(IBus bus)
        {
            this._bus = bus;
        }

        public async Task<bool> SendAsync<T>(T entity, string channel) where T:class
        {
            var endpointAddr = new Uri("rabbitmq://localhost/"+channel);
            var endpoint = await _bus.GetSendEndpoint(endpointAddr);
            await endpoint.Send(entity);
            return true;
        }

    }
}