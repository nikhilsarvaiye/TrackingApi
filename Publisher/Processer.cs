namespace Publisher
{
    using Models;
    using Rebus.Activation;
    using SDK;
    using Serilog;
    using Serilog.Core;
    using ServiceBus;
    using ServiceBus.Messages;
    using System;
    using System.Threading.Tasks;

    public static class Processer
    {
        public static async Task Process(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            try
            {
                // here actual processing code should present

                // get all the user audit details
                // get all the configuration details

                // once you have got the details and if you find you need to invoke any job just the put the entry in tracker like below
                /*
                var tracker = new Tracker(serviceBusConfiguration.TrackingApi, activator);
                var request = await tracker.CreateAsync(new CreateTrackingRequest
                {
                    Type = TrackingRequestType.DeleteUser,
                    MetaDeta1 = "clientId",
                    UserId = 0,
                    Content = null // if you have any
                });
                */
                Log.Information($"Adding New Tracking Entery");
                var tracker = new Tracker(serviceBusConfiguration.TrackingApi, activator);
                await tracker.CreateAsync(new CreateTrackingRequest
                {
                    Type = (int)TrackingRequestType.DeleteUser
                });
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error while processing");
            }
        }

        public static async Task SDKSampleOperations(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            var tracker = new Tracker(serviceBusConfiguration.TrackingApi, activator);

            // all trackers
            var trackers = await tracker.PaginateAsync();

            // create new
            var request = await tracker.CreateAsync(new CreateTrackingRequest
            {
                Type = (int)TrackingRequestType.DeleteUser
            });

            if (request != null)
            {
                // update state if required only
                var update = await tracker.UpdateStepAsync(request.Id, new UpdateTrackingStepRequest
                {
                    Step = 2
                });

                // mark as complete
                var complete = await tracker.CompleteAsync(request.Id, new CompleteTrackingRequest
                {
                    ResultType = TrackingRequestResultType.Success
                });

                // delete
                var result = await tracker.RemoveAsync(request.Id);
            }

            // sample for getting notifications using tracker id
            var notifications = await new SDK.Notification(serviceBusConfiguration.TrackingApi, activator).GetByTrackerIdAsync(0);

            // sample for getting user notifications using notification id
            var userNotifications = await new SDK.UserNotification(serviceBusConfiguration.TrackingApi, activator).GetByNotificationIdAsync(1);
        }
    }
}
