using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using myshop.Models.Settings;
using System.ComponentModel.DataAnnotations;


namespace myshop.Models.Attributes
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string _allowedExtensions;

		public AllowedExtensionsAttribute(string allowedExtensions = FileSettings.AllowedImageFileExtensions) =>
			_allowedExtensions = allowedExtensions;

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file is not null)
			{
				var fileExtension = Path.GetExtension(file.FileName);
				var extensions = _allowedExtensions.Split(',');
				if (!extensions.Contains(fileExtension))
					return new ValidationResult($"Only {_allowedExtensions} are allowed");
			}

			return ValidationResult.Success;
		}
	}
}
