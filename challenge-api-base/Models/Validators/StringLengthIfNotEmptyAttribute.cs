using System.ComponentModel.DataAnnotations;

public class StringLengthIfNotEmptyAttribute : ValidationAttribute
{
    private readonly int _maxLength;
    private readonly int _minLength;

    public StringLengthIfNotEmptyAttribute(int minLength, int maxLength)
    {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue) && (stringValue.Length < _minLength || stringValue.Length > _maxLength))
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}