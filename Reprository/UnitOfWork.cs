using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Models.Models;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using System.Diagnostics;

namespace MVCProject2.Reprository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplecationDBContext _dbContext;
        public ICategoryRepository Category {  get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplecationUserRepository ApplecationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public UnitOfWork(ApplecationDBContext dBContext)
        {
            _dbContext = dBContext;
            ShoppingCart = new ShoppingCartRepository(_dbContext);
            ApplecationUser = new ApplecationUserRepository(dBContext);
            Category = new CategoryRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            Company = new CompanyRepository(_dbContext);
            OrderDetail = new OrderDetailRepository(_dbContext);
            OrderHeader = new OrderHeaderRepository(_dbContext);
        }
        void IUnitOfWork.Save()
        {
            _dbContext.SaveChanges();

    }
    }
}
