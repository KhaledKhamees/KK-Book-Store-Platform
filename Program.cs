using Microsoft.EntityFrameworkCore;
using MVCProject2.Data;
using MVCProject2.Reprository;
using MVCProject2.Reprository.IRepository;
using MVCProject2.Reprository.IReprository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MVCProject2.Utility;
using Stripe;
using System.Configuration;

namespace MVCProject2
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			// Link SQL server and add connection string from Appsettings json file 
			builder.Services.AddDbContext<ApplecationDBContext>(
				options => options.UseSqlServer
				(builder.Configuration.GetConnectionString("FirstConnection")));

			builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            
			builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplecationDBContext>().AddDefaultTokenProviders();
            
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Login";
                options.AccessDeniedPath = $"/Identity/Account/Login";

            });
			builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(100);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
			builder.Services.AddAuthentication().AddGoogle(options => {
				IConfigurationSection googleAuthentication = builder.Configuration.GetSection("Authentication:Google");
				options.ClientId = googleAuthentication["ClientId"];
				options.ClientSecret = googleAuthentication["ClientSecret"];
			}).AddFacebook(Facebookoptions => {
                IConfigurationSection googleAuthentication = builder.Configuration.GetSection("Authentication:Facebook");
                Facebookoptions.ClientId = googleAuthentication["AppId"];
                Facebookoptions.ClientSecret = googleAuthentication["AppSecret"];
            });


            #region DI
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
			builder.Services.AddScoped<IApplecationUserRepository, ApplecationUserRepository>();
            builder.Services.AddScoped<IEmailSender,EmailSender>();
            #endregion
            builder.Services.AddRazorPages(); //this and the [app.MapRazorPages();] below use to can display the layout of login and regestration pages 

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			
			app.UseStaticFiles();
			StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseSession();
			app.MapRazorPages();
			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
