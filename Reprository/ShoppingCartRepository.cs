using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Reprository;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using System.Linq.Expressions;
using MVCProject2.Models;
using MVCProject2.Models.Models;
namespace MVCProject2.Reprository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private  ApplecationDBContext _db;
        public ShoppingCartRepository(ApplecationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ShoppingCart shoppingCart)
        {
            _db.shoppingCarts.Update(shoppingCart);
        }
    }
}
