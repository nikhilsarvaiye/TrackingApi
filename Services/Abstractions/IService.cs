namespace Services
{
    using DotnetStandardQueryBuilder.Core;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<T>
    {
        Task<long> BulkCreateAsync(List<T> tList);

        Task<long> BulkRemoveAsync(List<long> ids);

        Task<long> BulkRemoveAsync(IFilter filter);

        Task<long> BulkUpdateAsync(List<T> tList);

        Task<long> CreateAsync(T t);

        Task<List<T>> GetAsync(IRequest request = null);

        Task<T> GetAsync(long id);

        Task<List<T>> GetAsync(List<long> ids);

        Task<IResponse<T>> PaginateAsync(IRequest request);

        Task<long> RemoveAsync(long id);

        Task<long> UpdateAsync(long id, T t);
    }
}
