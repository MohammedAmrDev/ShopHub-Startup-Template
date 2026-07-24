using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using myshop.BLL.Interfaces;
using myshop.Models.Settings;

namespace myshop.BLL.Services
{
	public class LocalFileService : IFileService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public LocalFileService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public string UploadImage(IFormFile file)
		{
			string RootPath = _webHostEnvironment.WebRootPath;
			
			string filename = Guid.NewGuid().ToString();
			var upload = Path.Combine(RootPath, FileSettings.ProductsImagePath);
			var ext = Path.GetExtension(file.FileName);

			using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
			{
				file.CopyTo(filestream);
			}
			return FileSettings.ProductsImagePath + filename + ext;
		}
		public bool DeleteImage(string imageURL)
		{
			string RootPath = _webHostEnvironment.WebRootPath;
			var oldimg = Path.Combine(RootPath, imageURL.TrimStart('\\'));

			if (File.Exists(oldimg))
				File.Delete(oldimg);

			return !File.Exists(oldimg);
		}
	}
}
