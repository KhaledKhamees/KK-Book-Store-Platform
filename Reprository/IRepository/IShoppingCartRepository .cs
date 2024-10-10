using MVCProject2.Models;
using MVCProject2.Models.Models;
using MVCProject2.Repository.IRepository;

namespace MVCProject2.Reprository.IReprository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCart); 
    }
}
