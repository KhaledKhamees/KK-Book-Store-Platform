using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCProject2.Models.Models;
using MVCProject2.Utility;

namespace MVCProject2.Data.DbIntiallizer
{
    public class DBIntializer : IDbIntiallizer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplecationDBContext _dbContext;
        public DBIntializer(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager , ApplecationDBContext dBContext)
        {
            _dbContext = dBContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0) 
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplecationUser
                {
                    UserName = "Admin@kk.com",
                    Email = "Admin@kk.com",
                    Name = "Admin",
                    PhoneNumber = "1234567890",
                    PostelCode = "55555",
                    City = "Fayoum",

                }, "Admin@123456").GetAwaiter().GetResult();

                ApplecationUser user = _dbContext.applecationUsers.FirstOrDefault(u => u.Email == "Admin@kk.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return; 
        }
    }
}
