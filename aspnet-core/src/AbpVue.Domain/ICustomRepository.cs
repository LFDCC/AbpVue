using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace AbpVue
{
    public interface ICustomRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
