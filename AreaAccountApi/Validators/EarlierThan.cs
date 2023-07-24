using System;
using System.ComponentModel.DataAnnotations;

namespace AreaAccountApi.Validators;

public class EarlierThan : ValidationAttribute
{
    private readonly string _otherDateFieldName;

    public EarlierThan(string otherDateFieldName)
    {
        _otherDateFieldName = otherDateFieldName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var date = (DateTime)value;
        var propertyInfo = validationContext.ObjectType.GetProperty(_otherDateFieldName);
        if (propertyInfo.PropertyType != typeof(DateTime))
        {
            return new ValidationResult("invalid type");
        }

        var otherDate = (DateTime)propertyInfo.GetValue(validationContext.ObjectInstance)!;

        return date < otherDate
            ? ValidationResult.Success
            : new ValidationResult($"{validationContext.MemberName} should be earlier than {_otherDateFieldName}");
    }
}