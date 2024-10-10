using MVCProject2.Models;
using MVCProject2.Models.Models;
using MVCProject2.Repository.IRepository;

namespace MVCProject2.Reprository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
