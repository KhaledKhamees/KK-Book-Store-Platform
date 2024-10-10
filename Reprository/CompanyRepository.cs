using MVCProject2.Data;
using MVCProject2.Models.Models;
using MVCProject2.Reprository.IRepository;

namespace MVCProject2.Reprository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplecationDBContext _dbContext;
        public CompanyRepository(ApplecationDBContext applecationDBContext) : base(applecationDBContext)
        {
            _dbContext = applecationDBContext;
        }
        public void Update(Company company)
        {
            _dbContext.Update(company);
        }
    }
}
