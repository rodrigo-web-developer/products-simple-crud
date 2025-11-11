using System.ComponentModel.DataAnnotations;

namespace SimpleCrud.Validations
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public PositiveNumberAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = validationContext?.DisplayName ?? validationContext?.MemberName ?? "Value";

            return value switch
            {
                null => new ValidationResult($"The '{name}' cannot be null."),
                decimal d when d <= 0 => new ValidationResult($"The '{name}' must be a positive number."),
                int i when i <= 0 => new ValidationResult($"The '{name}' must be a positive number."),
                decimal _ => ValidationResult.Success,
                int _ => ValidationResult.Success,
                _ => new ValidationResult($"The '{name}' is an unsupported data type."),
            };
        }
    }
}
