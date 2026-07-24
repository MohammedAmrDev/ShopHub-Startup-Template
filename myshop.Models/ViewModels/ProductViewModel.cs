using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace myshop.Models.ViewModels
{
	public class ProductViewModel : IValidatableObject
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(70, ErrorMessage = "Product name max length is 70")]
		public string? Name { get; set; }

		[Required]
		[MaxLength(250, ErrorMessage = "Product name max length is 250")]
		public string? Description { get; set; }

		[Required]
		[Range(1, 500)]
		public decimal Price { get; set; }

		[Required]
		[AllowedExtensions]
		[MaximumAllowedSize]
		public IFormFile? ImageFile { get; set; }

		public string? ImageURL {  get; set; }

		[Required]
		[Display(Name = "Category")]
		public int? CategoryId { get; set; }

		[ValidateNever]
		public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

		// To make the ImageFile input required in the product creation
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (ImageURL == null && ImageFile == null)
			{
				yield return new ValidationResult("Product image is required", new[] { nameof(ImageFile) }); // Add the Property name of make its span that is tied with it to write the error message
			}
		}
	}
}
