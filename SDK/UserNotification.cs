namespace SDK
{
    using Rebus.Activation;
    using RestSharp;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UserNotificationModel = Models.UserNotification;

    public class UserNotification : Base<UserNotificationModel>
    {
        public UserNotification(string apiUrl, IHandlerActivator activator)
            : base(apiUrl, "userNotification", activator)
        {
        }

        public virtual async Task<List<UserNotificationModel>> GetByNotificationIdAsync(long notificationId)
        {
            return await Task.FromResult(RestClient.Get<List<UserNotificationModel>>(new RestRequest($"{Resource}/notification?id={notificationId}", (Method)DataFormat.Json)).Data);
        }
    }
}
