using PremiumHousing.Model;
using PremiumHousing.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PremiumHousing.Helper;
using Microsoft.AspNetCore.Http.HttpResults;


namespace PremiumHousing.Data
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public UserType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? BusinessPhoneNumper { get; set; }
        public string? BusinessLicenseNumper { get; set; } = string.Empty;
        public BusinessType? BusinessType  { get; set; }
        public City? HasBusinessAddress {  get; set; }
        public string? TaxNumber { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerificationDate {  get; set; }

    }
}



