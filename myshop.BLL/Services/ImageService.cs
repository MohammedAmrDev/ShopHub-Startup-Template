using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using myshop.BLL.Interfaces;

namespace myshop.BLL.Services
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ImageService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public string UploadImage(IFormFile file)
		{
			string RootPath = _webHostEnvironment.WebRootPath;
			
			string filename = Guid.NewGuid().ToString();
			var Upload = Path.Combine(RootPath, @"Images\Products");
			var ext = Path.GetExtension(file.FileName);

			using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
			{
				file.CopyTo(filestream);
			}
			return @"Images\Products\" + filename + ext;
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
