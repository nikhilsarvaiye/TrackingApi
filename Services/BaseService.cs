namespace Services
{
    using Configuration.Options;
    using DotnetStandardQueryBuilder.Core;
    using Models;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class BaseService<T>
        where T : BaseModel, new()
    {
        private readonly IAppOptions _appOptions;

        private readonly ICacheService<T> _cacheService;

        private readonly IRepository<T> _repository;

        public BaseService(IAppOptions appOptions, ICacheService<T> cacheService, IRepository<T> repository)
        {
            _appOptions = appOptions ?? throw new ArgumentException(nameof(appOptions));
            _repository = repository ?? throw new ArgumentException(nameof(repository));
            _cacheService = cacheService ?? throw new ArgumentException(nameof(cacheService));
        }

        private bool _cache => _appOptions.Cache;

        public virtual async Task<long> BulkCreateAsync(List<T> tList)
        {
            if (tList.Count == 0)
            {
                return 0;
            }

            await ValidateOnBulkCreate(tList);

            return await _repository.BulkCreateAsync(tList);
        }

        public virtual async Task<long> BulkRemoveAsync(List<long> ids)
        {
            if (ids.Count == 0)
            {
                return 0;
            }
            return await _repository.BulkRemoveAsync(ids);
        }

        public virtual async Task<long> BulkRemoveAsync(IFilter filter)
        {
            return await _repository.BulkRemoveAsync(filter);
        }

        public virtual async Task<long> BulkUpdateAsync(List<T> tList)
        {
            if (tList.Count == 0)
            {
                return 0;
            }

            await ValidateOnBulkUpdate(tList);

            return await _repository.BulkUpdateAsync(tList);
        }

        public virtual async Task<long> CreateAsync(T t)
        {
            await OnCreating(t);

            return await _repository.CreateAsync(t).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> GetAsync(IRequest request = null)
        {
            return await _repository.GetAsync(request).ConfigureAwait(false);
        }

        public virtual async Task<T> GetAsync(long id)
        {
            return await _repository.GetAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<List<T>> GetAsync(List<long> ids)
        {
            return await _repository.GetAsync(ids).ConfigureAwait(false);
        }

        public virtual async Task<T> GetOrThrowAsync(long id)
        {
            var item = await GetAsync(id).ConfigureAwait(false);
            if (item == null)
            {
                throw new Exception("Resource not found with Id " + id);
            }
            return item;
        }

        public virtual async Task<IResponse<T>> PaginateAsync(IRequest request)
        {
            return await _repository.PaginateAsync(request);
        }

        public virtual async Task<long> RemoveAsync(long id)
        {
            await OnDeleting(id).ConfigureAwait(false);

            return await _repository.RemoveAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<long> UpdateAsync(long id, T t)
        {
            await OnUpdating(t).ConfigureAwait(false);

            return await _repository.UpdateAsync(id, t).ConfigureAwait(false);
        }

        protected virtual async Task ValidateOnBulkCreate(List<T> tList)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task ValidateOnBulkUpdate(List<T> tList)
        {
            await ValidateOnBulkCreate(tList);
        }

        protected abstract Task<T> OnCreating(T t);

        protected virtual async Task OnDeleting(long id)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task<T> OnUpdating(T t)
        {
            return await OnCreating(t);
        }
    }
}
