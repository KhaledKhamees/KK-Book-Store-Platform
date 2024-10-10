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
    public class ApplecationUserRepository : Repository<ApplecationUser>, IApplecationUserRepository
    {
        private  ApplecationDBContext _db;
        public ApplecationUserRepository(ApplecationDBContext db) : base(db)
        {
            _db = db;
        }
        
    }
}
