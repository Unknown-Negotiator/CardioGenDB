using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPubApp.Common
{
    public class DateCoherenceAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateCoherenceAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;
            
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");

            try
            {
                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
                if (currentValue != comparisonValue.Year)
                    return new ValidationResult(ErrorMessage);
            }
            catch (Exception) { }

            return ValidationResult.Success;
        }
    }
}