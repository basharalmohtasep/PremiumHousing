using PremiumHousing.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace PremiumHousing.Model
{

    public class UserDtoModle
    {

        [Required(ErrorMessage = "Full name is required.")]
        [RegularExpression(@"^[A-Za-z]+(?:[-'\s][A-Za-z]+)*\s[A-Za-z]+(?:[-'\s][A-Za-z]+)*$",
                           ErrorMessage = "Enter a valid full name. Only letters, spaces, hyphens, and apostrophes are allowed.")]
        public string FullName { get; set; } = string.Empty;
        [RegularExpression(@"^[a-zA-Z0-9_-]{3,16}$",
                           ErrorMessage = "Username must be 3-16 characters and can only contain letters, numbers, underscores, and hyphens.")]
        public string Username { get; set; } = string.Empty;
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                           ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmed { get; set; } = string.Empty;
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                           ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression(@"^(?:\+962|0)7[789]\d{7}$",
                           ErrorMessage = "Enter a valid Jordanian phone number. Examples: +9627XXXXXXXX, 07XXXXXXXX.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        [Range(1, 3, ErrorMessage = "Invalid gender.")]
        public Gender Gender { get; set; }
        [Required]
        public bool Type { get; set; }
        public UserType UserType { get; set; } = UserType.Client;
        [RegularExpression(@"^(?:\+962|0(?:6|5|2|7))[789]\d{7}|(?:\+962|0(?:6|5|2))\d{7}$",
      ErrorMessage = "Enter a valid Jordanian phone number. Examples: +9627XXXXXXXX, 07XXXXXXXX, 062XXXXXXXX, 052XXXXXXXX, 022XXXXXXXX, 062XXXXXXX (landline).")]
        public string? BusinessPhoneNumper { get; set; }
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Business License Number must be 9 digits.")]
        public string? BusinessLicenseNumper { get; set; } = string.Empty;
        [Required(ErrorMessage = "Business Type is required.")]
        public BusinessType? BusinessType { get; set; }
        [Required(ErrorMessage = "Business Address is required.")]
        public City? BusinessAddress { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Tax Number must be 10 digits.")]
        public string? TaxNumber { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerificationDate { get; set; }
    }
}
