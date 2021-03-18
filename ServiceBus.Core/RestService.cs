using RestSharp;
using System;

namespace ServiceBus.Core
{
    public class RestService : IRestService
    {
        RestClient _client;

        public RestService()
        {
            _client = new RestClient("https://api.twitter.com/1.1");
        }

        public object Get(RestRequest restRequest)
        {
            if (restRequest.Method == null)
            {
                restRequest.Method = (Method)DataFormat.Json;
            }
            var request = new RestRequest("statuses/home_timeline.json", restRequest.Method);

            _client.Get(restRequest);
        }
    }
}
