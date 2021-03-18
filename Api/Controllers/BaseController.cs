namespace Api.Controllers
{
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController<T> : ControllerBase
        where T : BaseModel, new()
    {
        private readonly IService<T> _service;

        public BaseController(IService<T> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public virtual async Task<dynamic> GetAsync(long? id = null)
        {
            if (id.HasValue)
            {
                return new List<T>() { await _service.GetAsync(id.Value) };
            }

            var request = new DotnetStandardQueryBuilder.OData.UriParser(new DotnetStandardQueryBuilder.OData.UriParserSettings()).Parse<T>(Request.QueryString.ToString());

            if (request.Count)
            {
                return await _service.PaginateAsync(request);
            }

            return await _service.GetAsync(request);
        }

        [HttpGet("paginate")]
        public virtual async Task<IResponse<T>> PaginateAsync(int? pageSize = null, int page = 1)
        {
            return await _service.PaginateAsync(new Request { Count = true, Page = 1, PageSize = pageSize });
        }

        [HttpPost]
        public virtual async Task<T> CreateAsync(T skill)
        {
            return await _service.CreateAsync(skill);
        }

        [HttpPut]
        public virtual async Task UpdateAsync(long id, T t)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _service.UpdateAsync(id, t);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(long id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(id)))
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _service.RemoveAsync(id);
        }
    }
}
