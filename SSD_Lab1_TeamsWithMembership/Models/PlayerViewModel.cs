using SSD_Lab1_TeamsWithMembership.Utils;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SSD_Lab1_TeamsWithMembership.Models
{
    public class PlayerViewModel
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

        [Phone]
        [Display(Name = "Phone Number"), DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid phone number. Please use a phone number of 10 digits")]
        public string PhoneNumber { get; set; }

    }
}
