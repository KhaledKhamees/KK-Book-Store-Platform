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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository 
    {
        private  ApplecationDBContext _db;
        public OrderDetailRepository(ApplecationDBContext db) : base(db)
        {
            _db = db;
        }
        

        public void Update(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }
    }
}
