namespace Models
{
    using System;
    using System.Runtime.Serialization;
    
    public class BaseModel
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        [DataMember]
        public DateTime UpdatedDateTime { get; set; }
    }
}
