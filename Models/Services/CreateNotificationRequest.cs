using System.Collections.Generic;

namespace Models
{
    public class CreateNotificationRequest : Notification
    {

        public new object Content { get; set; }

        public List<long> Users { get; set; } = new List<long>();
	}
}
