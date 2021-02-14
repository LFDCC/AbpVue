
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

using Z.EntityFramework.Plus;

namespace AbpVue.EntityFrameworkCore
{
    public class CustomRepository<TEntity, TKey> : EfCoreRepository<AbpVueDbContext, TEntity, TKey>, ICustomRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public CustomRepository(
            IDbContextProvider<AbpVueDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var result = await DbContext.Set<TEntity>().Where(predicate).DeleteAsync(cancellationToken: cancellationToken);
            return result;
        }

        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory, CancellationToken cancellationToken = default)
        {
            var result = await DbContext.Set<TEntity>().Where(predicate).UpdateAsync(updateFactory, cancellationToken: cancellationToken);
            return result;
        }
    }
}
