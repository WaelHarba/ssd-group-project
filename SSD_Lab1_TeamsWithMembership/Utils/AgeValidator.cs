using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1_TeamsWithMembership.Utils
{
    public class AgeValidator : ValidationAttribute
    {
        public bool AllowUnder18 { get;}

        public AgeValidator(bool allowUnder18)
        {
            AllowUnder18 = allowUnder18;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Age must be at least 18 years old.");

            DateTimeOffset birthDate;
            try
            {
                birthDate = (DateTimeOffset)value;
            }
            catch
            {
                return new ValidationResult("Invalid date. Age must be at least 18 years old");
            }

            if(birthDate.DateTime > DateTime.Now.Date)
            {
                return new ValidationResult("Birth date cannot be in the future");
            }
            else if ((DateTime.Today.AddYears(-18) >= birthDate.DateTime) && !AllowUnder18)
            {
                return ValidationResult.Success;
            }
            else if (AllowUnder18)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Age must be at least 18 years old");
            }
        }
    }
}
