namespace Bookify.Web.Services
{
	public interface IImageService
	{
		Task<(bool isUploded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail);
		void Delete(string imagePath, string? imageThumbnailPath = null);
	}
}
