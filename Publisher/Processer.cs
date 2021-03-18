namespace Publisher
{
    using Models;
    using Rebus.Activation;
    using SDK;
    using ServiceBus;
    using System.Threading.Tasks;

    public static class Processer
    {
        public static async Task Process(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            // here actual processing code should present

            // get all the user audit details
            // get all the configuration details

        }

        // here actual processing code should present
        public static async Task Sample(ServiceBusConfiguration serviceBusConfiguration, IHandlerActivator activator)
        {
            var tracker = new Tracker(serviceBusConfiguration.TrackingApi, activator);

            // all trackers
            var trackers = await tracker.PaginateAsync();

            // create new
            var request = await tracker.CreateAsync(new CreateTrackingRequest
            {
                Type = TrackingRequestType.DeleteUser
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
