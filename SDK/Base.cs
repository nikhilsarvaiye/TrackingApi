namespace SDK
{
    using DotnetStandardQueryBuilder.Core;
    using Rebus.Activation;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Base<T>
        where T: class, new()
    {
        protected readonly string Resource;

        protected readonly RestClient RestClient;

        protected readonly BuiltinHandlerActivator ServiceBus;

        public Base(string apiUrl, string resource, IHandlerActivator activator)
        {
            RestClient = !string.IsNullOrEmpty(apiUrl) ? new RestClient(apiUrl) : throw new ArgumentNullException(nameof(apiUrl));
            Resource = resource;
            ServiceBus = activator as BuiltinHandlerActivator ?? throw new ArgumentNullException(nameof(activator));
        }

        public virtual async Task<T> GetAsync(long id)
        {
            var result = RestClient.Get<List<T>>(new RestRequest($"{Resource}?id={id}", (Method)DataFormat.Json));

            return await Task.FromResult(result.Data.FirstOrDefault());
        }

        public virtual async Task<T> CreateAsync(T t)
        {
            var response = RestClient.Post<T>(new RestRequest(Resource).AddJsonBody(t));
            return await Task.FromResult(response.Data);
        }

        public virtual async Task<T> UpdateAsync(long id, T t)
        {
            var response = RestClient.Put<T>(new RestRequest($"{Resource}/{id}").AddJsonBody(t));
            return await Task.FromResult(response.Data);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await Task.FromResult(RestClient.Get<List<T>>(new RestRequest(Resource, (Method)DataFormat.Json)).Data);
        }

        public virtual async Task<List<T>> GetAsync(string queryString = null)
        {
            var url = !string.IsNullOrEmpty(queryString) ? $"{Resource}?{queryString}" : Resource;
            return await Task.FromResult(RestClient.Get<List<T>>(new RestRequest(url, (Method)DataFormat.Json)).Data);
        }

        public virtual async Task<Response<T>> PaginateAsync(int? pageSize = null, int page = 1)
        {
            var url = $"{Resource}/paginate?page={page}&pageSize={pageSize}";
            return await Task.FromResult(RestClient.Get<Response<T>>(new RestRequest(url, (Method)DataFormat.Json)).Data);
        }

        public virtual async Task<long> RemoveAsync(long id)
        {
            var response = RestClient.Delete<long>(new RestRequest($"{Resource}?id={id}"));
            return await Task.FromResult(response.Data);
        }
    }
}
