using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PT.BuildingBlocks.Abstractions
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        IQueryable<T> Get(Expression<Func<T, bool>> ex);
        Task CreateAsync(T model);
        void Update(T model);
        void Delete(T model);

        Task SaveChangesAsync();
    }
}
