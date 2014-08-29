using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace TeamManager.Common
{
    public class ValidateName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = value as string;
            if (String.IsNullOrEmpty(name)) return null;
            if (name == "-") return new ValidationResult("- Is not a valid name.");
            return ValidationResult.Success;
        }
    }
}