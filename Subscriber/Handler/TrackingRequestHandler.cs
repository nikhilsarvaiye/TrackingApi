namespace Subscriber.Handler
{
    using Models;
    using Rebus.Activation;
    using Rebus.Handlers;
    using ServiceBus.Messages;
    using System;
    using System.Threading.Tasks;

    public class TrackingRequestHandler : IHandleMessages<TrackingRequest>
    {
        private readonly BuiltinHandlerActivator _activator;

        public TrackingRequestHandler(BuiltinHandlerActivator builtinHandlerActivator)
        {
            _activator = builtinHandlerActivator;
        }

        public async Task Handle(TrackingRequest trackingRequest)
        {
            Console.WriteLine("Got DeleteUserMessage: {0}", trackingRequest.Id);

            switch (trackingRequest.Type)
            {
                case TrackingRequestType.DeleteUser:
                    {
                        if (!trackingRequest.UserId.HasValue)
                        {
                            // Log using Serilog
                        }
                        _activator.Bus.Advanced.SyncBus.Publish(new DeleteUserMessage
                        {
                            UserId = trackingRequest.UserId.HasValue ? trackingRequest.UserId.Value : 0
                        });
                    }; break;
                case TrackingRequestType.NotifyUser:
                    {
                        if (!trackingRequest.UserId.HasValue)
                        {
                            // Log using Serilog
                        }
                        _activator.Bus.Advanced.SyncBus.Publish(new NotifyUserOnDeactivationMessage
                        {
                            UserId = trackingRequest.UserId.HasValue ? trackingRequest.UserId.Value : 0
                        });
                    }; break;
            }
        }
    }
}
