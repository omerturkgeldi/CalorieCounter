using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CalorieCounter.Core.Repositories;
using CalorieCounter.Core.Services;
using CalorieCounter.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Service.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository=repository;
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var hasProduct = await _repository.GetByIdAsync(id);

            //if(hasProduct == null)
            //{
            //    throw new NotFoundException($"{typeof(T).Name} ({id}) not found");
            //}
            return hasProduct;

        }


        public async Task<T> GetByGuidIdAsync(string id)
        {
            var hasProduct = await _repository.GetByGuidIdAsync(id);
            return hasProduct;
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        } 
        
        public async Task UpdateWithUserIdAsync(T entity, string id)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsyncWithUser(id);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }

        public async Task<T> AddWithUserIdAsync(T entity, string id)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsyncWithUser(id);
            return entity;
        }
    }
}
