namespace Subscriber.Handler
{
    using Rebus.Handlers;
    using ServiceBus.Messages;
    using System;
    using System.Threading.Tasks;

    public class DeleteUserHandler : IHandleMessages<DeleteUserMessage>
    {
        public async Task Handle(DeleteUserMessage deleteUserMessage)
        {
            Console.WriteLine("Got DeleteUserMessage: {0}", deleteUserMessage.UserId);
        }
    }
}
