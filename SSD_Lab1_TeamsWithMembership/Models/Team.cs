using SSD_Lab1_TeamsWithMembership.Utils;
using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1_TeamsWithMembership.Models
{
    public class Team
    {
        [Key]
        public Guid Id { get; set; }

        [Required, Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required]
        [EmailValidator]
        public string Email { get; set; }

        [Display(Name = "Established Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [AgeValidator(true)]
        public DateTimeOffset? EstablishedDate { get; set; }
    }
}
