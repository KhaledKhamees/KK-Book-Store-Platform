using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCProject2.Models;
using MVCProject2.Models.Models;


namespace MVCProject2.Data
{
	public class ApplecationDBContext : IdentityDbContext
	{
        public ApplecationDBContext(DbContextOptions<ApplecationDBContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }

        public DbSet<ApplecationUser> applecationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
        .HasOne(p => p.Category)
        .WithMany(c => c.Products) 
        .HasForeignKey(p => p.CategoryId);


            modelBuilder.Entity<Category>().HasData(
				new
				{
					Id = 1,
					Name = "Action",
					DisplayOrder = 1
				},
				new
				{
					Id = 2 , 
					Name = "Drama",
					DisplayOrder = 2
				}
				);
            modelBuilder.Entity<Product>().HasData(
               new Product
               {
                   Id = 1,
                   Name = "The Tragedy of Hamlet",
                   Description = "A Shakespearean drama about revenge and tragedy.",
                   ISBN = "1111111111",
                   Author = "William Shakespeare",
                   ListPrice = 29.99,
                   Price = 25.99,
                   Price50 = 23.99,
                   Price100 = 20.99,
                   CategoryId = 1,
                   ImageUrl =""
               },
               new Product
               {
                   Id = 2,
                   Name = "The Art of War",
                   Description = "An ancient Chinese military treatise attributed to Sun Tzu.",
                   ISBN = "2222222222",
                   Author = "Sun Tzu",
                   ListPrice = 19.99,
                   Price = 17.99,
                   Price50 = 15.99,
                   Price100 = 12.99,
                   CategoryId = 1,
                   ImageUrl = ""
               },
               new Product
               {
                   Id = 3,
                   Name = "A Brief History of Time",
                   Description = "An exploration of cosmology and the universe.",
                   ISBN = "3333333333",
                   Author = "Stephen Hawking",
                   ListPrice = 35.99,
                   Price = 32.99,
                   Price50 = 30.99,
                   Price100 = 28.99,
                   CategoryId = 1,
                   ImageUrl = ""
               },
               new Product
               {
                   Id = 4,
                   Name = "1984",
                   Description = "A dystopian science fiction novel about a totalitarian regime.",
                   ISBN = "4444444444",
                   Author = "George Orwell",
                   ListPrice = 24.99,
                   Price = 22.99,
                   Price50 = 20.99,
                   Price100 = 18.99,
                   CategoryId = 1,
                   ImageUrl = ""
               },
               new Product
               {
                   Id = 5,
                   Name = "The History of Ancient Egypt",
                   Description = "A deep dive into the civilization of ancient Egypt.",
                   ISBN = "5555555555",
                   Author = "John Romer",
                   ListPrice = 40.99,
                   Price = 37.99,
                   Price50 = 35.99,
                   Price100 = 32.99,
                   CategoryId = 1,
                   ImageUrl = ""
               },
               new Product
               {
                   Id = 6,
                   Name = "The Story of Art",
                   Description = "A renowned book that traces the history of visual arts.",
                   ISBN = "6666666666",
                   Author = "E. H. Gombrich",
                   ListPrice = 45.99,
                   Price = 42.99,
                   Price50 = 39.99,
                   Price100 = 36.99,
                   CategoryId = 1,
                   ImageUrl = ""
               }
           );
        }

	}
}
