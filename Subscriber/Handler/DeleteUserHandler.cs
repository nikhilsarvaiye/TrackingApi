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

    public class DeleteUserHandler : IHandleMessages<TrackingRequest>
    {
        private readonly ServiceBusConfiguration _serviceBusConfiguration;

        private readonly IHandlerActivator _activator;

        public DeleteUserHandler(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
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
            if (trackingRequest.Type == (int)TrackingRequestType.DeleteUser)
            {
                Log.Information($"Received {nameof(TrackingRequest)} in {nameof(DeleteUserHandler)}: Id - {trackingRequest.Id}");

                try
                {
                    //Log.Information("Received DeleteUserMessage: {0}", trackingRequest.UserId);

                    //var apiClient = new RestClient(_serviceBusConfiguration.Api);

                    //var deleted = apiClient.Delete<bool>(new RestRequest($"user?id={deleteUserMessage.UserId}")).Data;

                    //if (deleted)
                    //{
                    //    var tracker = new Tracker(_serviceBusConfiguration.TrackingApi, _activator);
                    //    var trackerRequest = await tracker.CompleteAsync(deleteUserMessage.TrackerId, new CompleteTrackingRequest
                    //    {
                    //        ResultType = TrackingRequestResultType.Success
                    //    });
                    //}
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error in " + nameof(DeleteUserHandler));
                    //var tracker = new Tracker(_serviceBusConfiguration.TrackingApi, _activator);
                    //await tracker.CompleteAsync(deleteUserMessage.TrackerId, new CompleteTrackingRequest
                    //{
                    //    ResultType = TrackingRequestResultType.Failed,
                    //    ResultDetails = new Exception(exception.Message, exception)
                    //});
                }
            }
        }
    }
}
