using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPubApp.Common
{
    public class PositionControlAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public PositionControlAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) // Unfinished
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (short)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            try
            {
                var comparisonValue = (Publication)property.GetValue(validationContext.ObjectInstance);
                if (currentValue != comparisonValue.Authorships.Max(x => x.Position)+ 1)
                    return new ValidationResult(ErrorMessage);
            }
            catch (Exception) { }

            return ValidationResult.Success;
        }
    }
}