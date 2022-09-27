using System;
using System.ComponentModel.DataAnnotations;

namespace KcloudScript.Model
{
    public class UrlValidateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? Url, ValidationContext validationContext)
        {
            Uri? uriResult;
            if (Url != null)
            {
                bool result = Uri.TryCreate(Url.ToString(), UriKind.Absolute, out uriResult)
                  && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Please enter a valid Url");
                }
            }
            else
            {
                return new ValidationResult("Url can not be blank");
            }
        }
    }

    public class UrlRequestEntity
    {
        [Required]
        [UrlValidateAttribute(ErrorMessage = "Please enter a valid Url")]
        public string? RequestUrl { get; set; }
        public DateTime RequestDate { get; set; }
        public UrlRequestEntity()
        {
            RequestDate = DateTime.Now;
        }
    }
}
