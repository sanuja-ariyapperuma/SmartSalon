using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SalonCS.CustomValidations
{
    public class CustomValidationPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            string password = value as string;

            if (!string.IsNullOrEmpty(password))
            {
                
                if (password.Length < 8)
                {
                    return new ValidationResult("Password should be more than 8 characters");
                }else if (!Regex.IsMatch(password, @"^[a-z]+$")) 
                {
                    return new ValidationResult("Password should contain atleast one simple letter");
                }
                else if (!Regex.IsMatch(password, @"^[A-Z]+$"))
                {
                    return new ValidationResult("Password should contain atleast one capital letter");
                }
                else if (!Regex.IsMatch(password, @"\d"))
                {
                    return new ValidationResult("Password should contain atleast one number");
                }

            }

            return ValidationResult.Success;
        }
    }
}
