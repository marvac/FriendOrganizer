using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task SaveAsync();
        bool HasChanges();
        void Add(T model);
        void Delete(T model);
    }
}