using MVCProject2.Models;
using MVCProject2.Repository.IRepository;

namespace MVCProject2.Reprository.IReprository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category); 
    }
}
