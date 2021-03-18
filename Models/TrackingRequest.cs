namespace Models
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    public class TrackingRequest : BaseModel
    {
        [DataMember]
        public long? UserId { get; set; }

        [DataMember]
        public TrackingRequestType Type { get; set; }

        [DataMember]
        public string MetaDeta1 { get; set; }

        [DataMember]
        public string MetaDeta2 { get; set; }

        [DataMember]
        public string MetaDeta3 { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public TrackingRequestStatus Status { get; set; }

        [DataMember]
        public TrackingRequestResultType? Result { get; set; }

        [DataMember]
        public string ResultDetails { get; set; }

        [DataMember]
        public int TotalSteps { get; set; } = 1;

        [DataMember]
        public int CurrentStep { get; set; } = 1;

        [DataMember]
        public string CurrentStepDescription { get; set; }

        public TrackingRequest()
        {
            Status = TrackingRequestStatus.Pending;
        }

        public void PartiallySucceeded(object resultDetails = null)
        {
            Status = TrackingRequestStatus.Completed;
            Result = TrackingRequestResultType.Partial;
            ResultDetails = resultDetails != null ?  JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Succeeded(object resultDetails = null)
        {
            Status = TrackingRequestStatus.Completed;
            CurrentStep = TotalSteps;
            Result = TrackingRequestResultType.Success;
            ResultDetails = resultDetails != null ? JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Failed(object errorDetails = null)
        {
            Status = TrackingRequestStatus.Completed;
            Result = TrackingRequestResultType.Failed;
            ResultDetails = errorDetails != null ? JsonConvert.SerializeObject(errorDetails) : null;
        }
    }
}
