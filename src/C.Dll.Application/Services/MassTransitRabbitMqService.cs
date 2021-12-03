using System;
using System.Threading.Tasks;
using MassTransit;

namespace Application.Services
{
    public class MassTransitRabbitMqService
    {
        private readonly IBus _bus;
        private readonly Uri uri;

        public MassTransitRabbitMqService(IBus bus, Uri uri)
        {
            this._bus = bus;
            this.uri = uri;
        }

        public async Task Publish<T>(T model) where T:class
        {
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(model);
            return;
        }

    }
}