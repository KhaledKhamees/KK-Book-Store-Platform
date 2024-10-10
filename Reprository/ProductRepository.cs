using MVCProject2.Data;
using MVCProject2.Models.Models;
using MVCProject2.Reprository.IRepository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplecationDBContext _dbContext;

    public ProductRepository(ApplecationDBContext applecationDBContext) : base(applecationDBContext)
    {
        _dbContext = applecationDBContext;
    }

    public void Update(Product product)
    {
        _dbContext.Update(product);
    }
}
