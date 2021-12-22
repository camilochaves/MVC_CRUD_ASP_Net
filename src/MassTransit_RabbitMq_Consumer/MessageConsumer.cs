using System;
using System.Threading.Tasks;
using Domain.Entities;
using MassTransit;

namespace Consumer
{
    public class MessageConsumer : IConsumer<Message<string>>
    {
        public async Task Consume(ConsumeContext<Message<string>> context)
        {
            await Console.Out.WriteLineAsync("Message received at " + context.Message.Time.ToLongDateString());
            await Console.Out.WriteLineAsync($"Data: {context.Message.Data}");
        }
    }
}
