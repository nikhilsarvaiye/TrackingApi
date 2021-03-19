namespace Subscriber.Handler
{
    using Models;
    using Rebus.Activation;
    using Rebus.Handlers;
    using Serilog;
    using ServiceBus;
    using ServiceBus.Messages;
    using System;
    using System.Threading.Tasks;
    
    public class NotifyUserOnDeactivationHandler : IHandleMessages<TrackingRequest>
    {
        private readonly ServiceBusConfiguration _serviceBusConfiguration;

        private readonly IHandlerActivator _activator;

        public NotifyUserOnDeactivationHandler(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            _serviceBusConfiguration = serviceBusConfiguration;
            if (string.IsNullOrEmpty(serviceBusConfiguration?.Api))
            {
                throw new ArgumentNullException(nameof(ServiceBusConfiguration.Api));
            }
            if (string.IsNullOrEmpty(serviceBusConfiguration?.TrackingApi))
            {
                throw new ArgumentNullException(nameof(ServiceBusConfiguration.TrackingApi));
            }
            _activator = activator;
        }

        public async Task Handle(TrackingRequest trackingRequest)
        {
            if (trackingRequest.Type == (int)TrackingRequestType.NotifyUser)
            {
                Log.Information($"Received {nameof(TrackingRequest)} in {nameof(NotifyUserOnDeactivationHandler)}: Id - {trackingRequest.Id}");
                
                try
                {
                    //var apiClient = new RestClient(_serviceBusConfiguration.Api);

                    //var apiUser = apiClient.Get<ApiUser>(new RestRequest($"user?id={notifyUserOnDeactivationMessage.UserId}")).Data;

                    //if (!string.IsNullOrEmpty(apiUser.Email))
                    //{
                    //    var notification = new SDK.Notification(_serviceBusConfiguration.TrackingApi, _activator);

                    //    var createdNotification = notification.CreateAsync(new Models.Notification
                    //    {
                    //        Type = NotificationMessageType.NotifyUser,
                    //        Content = ""
                    //    });
                    //}
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error in " + nameof(DeleteUserHandler));
                    //var tracker = new Tracker(_serviceBusConfiguration.TrackingApi, _activator);
                    //await tracker.CompleteAsync(notifyUserOnDeactivationMessage.TrackerId, new CompleteTrackingRequest
                    //{
                    //    ResultType = TrackingRequestResultType.Failed,
                    //    ResultDetails = new Exception(exception.Message, exception)
                    //});
                }
            }
        }
    }
}
