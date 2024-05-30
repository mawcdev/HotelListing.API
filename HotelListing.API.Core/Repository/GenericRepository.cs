using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Models;
using HotelListing.API.Data;
using HotelListing.API.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net.Quic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HotelListing.API.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TResult> AddAsync<TResult, TSource>(TSource source)
        {
            var entity = _mapper.Map<T>(source);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResult>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var get = await GetAsync(id);
            if (get == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            _context.Set<T>().Remove(get);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var get = await GetAsync(id);
            return get != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                    .Skip(queryParameters.Skip)
                    .Take(queryParameters.PageSize)
                    .ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                TotalCount = totalSize,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = items.Count
            };
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            var result = await GetAsync(id);

            if (result == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            return _mapper.Map<TResult>(result);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetAsync(id);
            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
