namespace SDK
{
    using Models;
    using Rebus.Activation;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NotificationModel = Models.Notification;

    public class Notification : Base<NotificationModel>
    {
        public Notification(string apiUrl, IHandlerActivator activator)
            : base(apiUrl, "notification", activator)
        {
        }

        public virtual async Task<List<NotificationModel>> GetByTrackerIdAsync(long trackerId)
        {
            return await Task.FromResult(RestClient.Get<List<NotificationModel>>(new RestRequest($"{Resource}/request?id={trackerId}", (Method)DataFormat.Json)).Data);
        }
    }
}
