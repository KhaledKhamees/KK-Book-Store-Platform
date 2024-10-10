using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Reprository;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using System.Linq.Expressions;
using MVCProject2.Models;
namespace MVCProject2.Reprository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        private  ApplecationDBContext _db;
        public CategoryRepository(ApplecationDBContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
