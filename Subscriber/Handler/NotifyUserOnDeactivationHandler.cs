namespace Subscriber.Handler
{
    using Rebus.Handlers;
    using ServiceBus.Messages;
    using System;
    using System.Threading.Tasks;
    
    public class NotifyUserOnDeactivationHandler : IHandleMessages<NotifyUserOnDeactivationMessage>
    {
        public async Task Handle(NotifyUserOnDeactivationMessage notifyUserOnDeactivationMessage)
        {
            Console.WriteLine("Got NotifyUserOnDeactivationMessage: {0}", notifyUserOnDeactivationMessage.UserId);
        }
    }
}
