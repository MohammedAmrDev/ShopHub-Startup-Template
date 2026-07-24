using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using myshop.Models.Settings;
using System.ComponentModel.DataAnnotations;


namespace myshop.Models.Attributes
{
	public class MaximumAllowedSizeAttribute : ValidationAttribute
	{
		private readonly int _maxSizeInMB;
		private readonly int _maxSizeInBytes;

		public MaximumAllowedSizeAttribute(int maxSizeInMB = FileSettings.MaxFileSizeInMB)
		{
			_maxSizeInMB = maxSizeInMB;
			_maxSizeInBytes = _maxSizeInMB * 1024 * 1024;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			var file = value as IFormFile;

			if (file is not null)
			{
				if (file.Length > _maxSizeInBytes)
					return new ValidationResult($"Maximum size allowed is {_maxSizeInMB}MB");
			}

			return ValidationResult.Success;
		}
	}
}
