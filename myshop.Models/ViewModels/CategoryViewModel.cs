using System.ComponentModel.DataAnnotations;

namespace myshop.Models.ViewModels
{
	public class CategoryViewModel
	{
		public int? Id { get; set; }
		[Required(ErrorMessage = "Category Name is required")]
		public string Name { get; set; }

		public string Description { get; set; }
	}
}