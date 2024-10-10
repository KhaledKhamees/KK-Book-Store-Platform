using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ICompanyRepository Company { get; }
    IShoppingCartRepository ShoppingCart { get; }
    IApplecationUserRepository ApplecationUser { get; }
    IOrderDetailRepository OrderDetail { get; }
    IOrderHeaderRepository OrderHeader { get; }
    void Save();
}
