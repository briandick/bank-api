using Bank.Domain.Services;
using Bank.Infrastructure.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Infrastructure.Services
{
    public class DbBasedRepository<T> : IRepository<T> where T : class
    {
        private readonly Bank.Infrastructure.DbContext.BankDbContext _context;

        public DbBasedRepository(Bank.Infrastructure.DbContext.BankDbContext context)
        {
            var serviceProvider = new TestSetup().SetupInMemoryDatabase();
            _context = serviceProvider.GetRequiredService<BankDbContext>();
        }

        public async Task<T?> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> Add(T record)
        {
            await _context.AddAsync<T>(record);
            _context.SaveChanges();
            return record;
        }

        public async Task<T> Update(T record)
        {
            _context.Update(record);
            _context.SaveChanges();
            return record;
        }

        public void Delete(int id)
        {
            var record = GetById(id);
            if (record != null)
            {
                _context.Remove(record);
                _context.SaveChanges();
            }
        }
    }
}
