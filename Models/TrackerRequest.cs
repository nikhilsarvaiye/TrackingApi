namespace Models
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    public class TrackerRequest : BaseModel
    {
        [DataMember]
        public long? UserId { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public string MetaDeta1 { get; set; }

        [DataMember]
        public string MetaDeta2 { get; set; }

        [DataMember]
        public string MetaDeta3 { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public TrackerRequestStatus Status { get; set; }

        [DataMember]
        public TrackerRequestResultType? Result { get; set; }

        [DataMember]
        public string ResultDetails { get; set; }

        [DataMember]
        public int TotalSteps { get; set; } = 1;

        [DataMember]
        public int CurrentStep { get; set; } = 1;

        [DataMember]
        public string CurrentStepDescription { get; set; }

        public TrackerRequest()
        {
            Status = TrackerRequestStatus.Pending;
        }

        public void PartiallySucceeded(object resultDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            Result = TrackerRequestResultType.Partial;
            ResultDetails = resultDetails != null ?  JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Succeeded(object resultDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            CurrentStep = TotalSteps;
            Result = TrackerRequestResultType.Success;
            ResultDetails = resultDetails != null ? JsonConvert.SerializeObject(resultDetails) : null;
        }

        public void Failed(object errorDetails = null)
        {
            Status = TrackerRequestStatus.Completed;
            Result = TrackerRequestResultType.Failed;
            ResultDetails = errorDetails != null ? JsonConvert.SerializeObject(errorDetails) : null;
        }
    }
}
