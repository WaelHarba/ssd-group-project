using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SSD_Lab1_TeamsWithMembership.Utils;

namespace SSD_Lab1_TeamsWithMembership.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        [AgeValidator(false)]
        public DateTimeOffset BirthDate { get; set; }
    }
}
