using PT.BuildingBlocks.Abstractions;
using PT.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PT.Infrastructure
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T model)
        {
           await _context.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _context.Remove(model);
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> ex)
        {
            return _context.Set<T>().Where(ex);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T model)
        {
           _context.Set<T>().Update(model);
        }
    }
}
