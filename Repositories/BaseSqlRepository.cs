namespace Repositories
{
    using Configuration.Options;
    using Dapper;
    using Models;
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Data.SqlClient;
    using System;

    public class BaseSqlRepository<T> : IDisposable
        where T : BaseModel
    {
        // To detect redundant calls
        private bool _disposed;

        private readonly SqlConnection _sqlConnection;

        private readonly string _tableName;

        ~BaseSqlRepository() => Dispose(false);

        public BaseSqlRepository(IDbOptions dbOptions, string tableName)
        {
            _sqlConnection = new SqlConnection(dbOptions.ConnectionString);
            _tableName = !string.IsNullOrEmpty(dbOptions.SchemaName) ? $"[{dbOptions.SchemaName}].[{tableName}]" : tableName;
        }

        public async Task<long> BulkCreateAsync(List<T> tList)
        {
            foreach (var item in tList)
            {
                item.CreatedDateTime = DateTime.Now; 
                item.UpdatedDateTime = DateTime.Now;
            }
            var sqlQuery = new Request().CreateQuery(tList, _tableName);

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }

        public async Task<long> BulkRemoveAsync(List<long> ids)
        {
            var request = new Request()
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsContainedIn,
                    Property = nameof(BaseModel.Id),
                    Value = ids
                },
            };
            var sqlQuery = request.DeleteQuery(_tableName);

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }

        public async Task<long> BulkRemoveAsync(IFilter filter)
        {
            var request = new Request()
            {
                Filter = filter
            };
            var sqlQuery = request.DeleteQuery(_tableName);

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }

        public async Task<long> BulkUpdateAsync(List<T> tList)
        {
            foreach (var item in tList)
            {
                item.UpdatedDateTime = DateTime.Now;
            }
            var sqlQuery = new Request().UpdateQuery(tList, _tableName);

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                _sqlConnection?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }

        public async Task<List<T>> GetAsync(IRequest request = null)
        {
            var sqlQuery = request.Query(_tableName);

            var items = await _sqlConnection.QueryAsync<T>(sqlQuery.Query, sqlQuery.Values);

            return await Task.FromResult(items.ToList());
        }

        public async Task<T> GetAsync(long id)
        {
            var sqlQuery = new Request()
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsEqualTo,
                    Property = nameof(BaseModel.Id),
                    Value = id
                },
                PageSize = 1
            }.Query(_tableName);

            var item = await _sqlConnection.QueryFirstOrDefaultAsync<T>(sqlQuery.Query, sqlQuery.Values);

            return await Task.FromResult(item);
        }

        public async Task<List<T>> GetAsync(List<long> ids)
        {
            var sqlQuery = new Request()
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsContainedIn,
                    Property = nameof(BaseModel.Id),
                    Value = ids
                },
            }.Query(_tableName);

            var items = await _sqlConnection.QueryAsync<T>(sqlQuery.Query, sqlQuery.Values);

            return await Task.FromResult(items.ToList());
        }

        public async Task<IResponse<T>> PaginateAsync(IRequest request)
        {
            var sqlQuery = request.Query(_tableName);
            var sqlQueryCount = request.QueryCount(_tableName);

            var items = await _sqlConnection.QueryAsync<T>(sqlQuery.Query, sqlQuery.Values);
            var count = await _sqlConnection.QueryFirstAsync<long>(sqlQueryCount.Query, sqlQueryCount.Values);

            return new Response<T>
            {
                Count = count,
                Items = items.ToList()
            };
        }

        public async Task<T> CreateAsync(T t)
        {
            t.CreatedDateTime = DateTime.Now;
            t.UpdatedDateTime = DateTime.Now;

            var sqlQuery = new Request().CreateQuery(new List<T> { t }, _tableName, new List<string> { nameof(BaseModel.Id) });

            sqlQuery.Query = $"{sqlQuery.Query};SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await _sqlConnection.QueryFirstAsync<long>(sqlQuery.Query, sqlQuery.Values);

            if (id > 0)
            {
                return await GetAsync(id);
            }

            return null;
        }

        public async Task<long> UpdateAsync(long id, T t)
        {
            t.Id = id;
            t.UpdatedDateTime = DateTime.Now;
            var request = new Request()
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsEqualTo,
                    Property = nameof(BaseModel.Id),
                    Value = id
                },
            };
            var sqlQuery = request.UpdateQuery<T>(new List<T> { t }, _tableName, new List<string> { nameof(BaseModel.Id) });

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }

        public async Task<long> RemoveAsync(long id)
        {
            var request = new Request()
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsEqualTo,
                    Property = nameof(BaseModel.Id),
                    Value = id
                },
            };
            var sqlQuery = request.DeleteQuery(_tableName);

            return await _sqlConnection.ExecuteAsync(sqlQuery.Query, sqlQuery.Values);
        }
    }
}