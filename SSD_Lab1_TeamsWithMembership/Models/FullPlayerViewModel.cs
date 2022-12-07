using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using SSD_Lab1_TeamsWithMembership.Utils;

namespace SSD_Lab1_TeamsWithMembership.Models
{
    public class FullPlayerViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [AgeValidator(false)]
        public DateTimeOffset BirthDate { get; set; }

        [Required]
        [EmailValidator]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Phone]
        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid phone number. Please use a phone number of 10 digits")]
        public string PhoneNumber { get; set; }
    }
}
