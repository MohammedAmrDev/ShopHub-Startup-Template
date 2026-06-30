using Microsoft.AspNetCore.Http;

namespace myshop.BLL.Interfaces
{
	public interface IImageService
	{
		string UploadImage(IFormFile file);
		bool DeleteImage(string imageURL);
	}
}
