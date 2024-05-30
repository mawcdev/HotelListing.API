using HotelListing.API.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.API.Core.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<TResult> GetAsync<TResult>(int? id);
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);
        Task<TResult> AddAsync<TResult, TSource>(TSource source);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task UpdateAsync<TSource>(int id, TSource source);
        Task<bool> Exists(int id);
    }
}
