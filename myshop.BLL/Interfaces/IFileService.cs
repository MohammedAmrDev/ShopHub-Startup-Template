using Microsoft.AspNetCore.Http;

namespace myshop.BLL.Interfaces
{
	public interface IFileService
	{
		string UploadImage(IFormFile file);
		bool DeleteImage(string imageURL);
	}
}
