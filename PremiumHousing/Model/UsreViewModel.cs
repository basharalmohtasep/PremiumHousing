using System.Security.Cryptography;
using System.Text;

namespace PremiumHousing.Model
{

    public class UserViewModel
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public DateTime? VerificationDate { get; set; }
    }
}
