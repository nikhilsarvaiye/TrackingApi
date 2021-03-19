namespace Models
{
	using System;
    using System.Runtime.Serialization;

    public class UserNotification : BaseModel
	{
		[DataMember]
		public long UserId { get; set; }

		[DataMember]
		public long NotificationId { get; set; }

		[DataMember]
		public bool IsRead { get; set; }

		[DataMember]
		public DateTime? ReadDateTime { get; set; }
	}
}
