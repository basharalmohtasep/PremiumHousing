using PremiumHousing.Helper;
using PremiumHousing.Data;
using PremiumHousing.Model;
using PremiumHousing.Model.Enums;
using Microsoft.EntityFrameworkCore;
namespace PremiumHousing.Helper
{
    public class AdminUserSeeder
    {
        private readonly ApplicationDbContext _context;

        public AdminUserSeeder(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task SeedAdminUserAsync()
        {
            var adminUsername = "system";
            var adminPassword = "Admin@1234";


            var existingAdmin = await _context.User.FirstOrDefaultAsync(x => x.Username.Equals(adminUsername) && x.Type.Equals(UserType.system));
            if (existingAdmin is null)
            {
                var adminUser = new User()
                {
                    FullName = "system",
                    Username = adminUsername,
                    Password = HashHelper.HashPassword(adminPassword),
                    Email = "OneClick_Finder@info.com",
                    PhoneNumber = "",
                    Gender = Gender.Male,
                    Type = UserType.system,
                    CreatedDate = DateTime.Now,
                    VerificationDate=DateTime.Now
                };
                _context.User.Add(adminUser);
                await _context.SaveChangesAsync();
            }

        }

    }
}
