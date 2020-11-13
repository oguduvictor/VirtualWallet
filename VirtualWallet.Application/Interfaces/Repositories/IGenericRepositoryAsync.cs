using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VirtualWallet.Application.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetPagedReponseAsync<TProperty>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TProperty>> navigationalPropertyPath);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
