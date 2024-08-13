using Bank.Domain.Models;
using System.Threading.Tasks;

namespace Bank.Domain.Services
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(int id);

        IQueryable<T> GetAll();

        Task<T> Add(T record);

        Task<T> Update(T record);

        void Delete(int id);
    }
}
