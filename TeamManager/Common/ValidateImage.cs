using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace TeamManager.Common
{
    public class ValidateImage : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return null;
            }
            if (file.ContentLength > 1 * 1024 * 1024)
            {
                return new ValidationResult("Image too large.");
            }
            try
            {
                using (var img = Image.FromStream(file.InputStream))
                {
                    if (img.RawFormat.Equals(ImageFormat.Png) ||
                        img.RawFormat.Equals(ImageFormat.Jpeg))
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            catch { }
            return new ValidationResult("Not a PNG or a JPEG.");
        }
    }
}