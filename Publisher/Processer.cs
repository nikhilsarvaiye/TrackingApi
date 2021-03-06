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
                var tracker = new Tracker(serviceBusConfiguration.TrackerApi, activator);
                var request = await tracker.CreateAsync(new CreateTrackerRequest
                {
                    Type = TrackerRequestType.DeleteUser,
                    MetaDeta1 = "clientId",
                    UserId = 0,
                    Content = null // if you have any
                });
                */
                Log.Information($"Adding New Tracker");
                var tracker = new Tracker(serviceBusConfiguration.TrackerApi, activator);
                await tracker.CreateAsync(new CreateTrackerRequest
                {
                    Type = (int)TrackerRequestType.DeleteUser,
                    UserId = 0,
                });
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error while processing");
            }
        }

        public static async Task SDKSampleOperations(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            var tracker = new Tracker(serviceBusConfiguration.TrackerApi, activator);

            // all trackers
            var trackers = await tracker.PaginateAsync();

            // create new
            var request = await tracker.CreateAsync(new CreateTrackerRequest
            {
                Type = (int)TrackerRequestType.DeleteUser
            });

            if (request != null)
            {
                // update state if required only
                var update = await tracker.UpdateStepAsync(request.Id, new UpdateTrackerStepRequest
                {
                    Step = 2
                });

                // mark as complete
                var complete = await tracker.CompleteAsync(request.Id, new CompleteTrackerRequest
                {
                    ResultType = TrackerRequestResultType.Success
                });

                // delete
                var result = await tracker.RemoveAsync(request.Id);
            }

            // sample for getting notifications using tracker id
            var notifications = await new SDK.Notification(serviceBusConfiguration.TrackerApi, activator).GetByTrackerIdAsync(0);

            // sample for getting user notifications using notification id
            var userNotifications = await new SDK.UserNotification(serviceBusConfiguration.TrackerApi, activator).GetByNotificationIdAsync(1);
        }
    }
}
