namespace SDK
{
    using Models;
    using Rebus.Activation;
    using RestSharp;
    using System;
    using System.Threading.Tasks;

    public class Tracker : Base<TrackingRequest>
    {
        public Tracker(string apiUrl, IHandlerActivator activator)
            : base(apiUrl, "trackrequest", activator)
        {
        }

        public override Task<TrackingRequest> CreateAsync(TrackingRequest t)
        {
            throw new NotImplementedException($"Kindly use another method CreateAsync method with {nameof(CreateTrackingRequest)} as input");
        }

        public async Task<TrackingRequest> CreateAsync(CreateTrackingRequest createTrackingRequest)
        {
            var response = RestClient.Post<TrackingRequest>(new RestRequest($"{Resource}/create").AddJsonBody(createTrackingRequest));
            if (response.Data != null)
            {
                ServiceBus.Bus.Advanced.SyncBus.Publish(response.Data);
            }
            return await Task.FromResult(response.Data);
        }

        public async Task<long> UpdateStepAsync(long id, UpdateTrackingStepRequest updateTrackingStepRequest)
        {
            return await Task.FromResult(RestClient.Put<long>(new RestRequest($"{Resource}/step/{id}").AddJsonBody(updateTrackingStepRequest)).Data);
        }

        public async Task<long> CompleteAsync(long id, CompleteTrackingRequest completeTrackingRequest)
        {
            return await Task.FromResult(RestClient.Put<long>(new RestRequest($"{Resource}/complete/{id}").AddJsonBody(completeTrackingRequest)).Data);
        }
    }
}
