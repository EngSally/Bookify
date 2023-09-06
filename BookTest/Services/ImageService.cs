using BookTest.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace BookTest.Services
{
    public class ImageService : IImageService
    {

       private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly List<string> _allowedImageExtension=new(){ ".jpg",".jpeg",".png",".ico"};
        private readonly int _allowedSize=3145728;//2Mb


        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        

        public async  Task<(bool isUploded, string? errorMessage)> UploadAsync(IFormFile image, string imageName, string folderPath, bool hasThumbnail)
        {
            string extension=Path.GetExtension(image.FileName.ToLower());
            if (!_allowedImageExtension.Contains(extension))
            {
                return (isUploded: false, errorMessage: Errors.AllowedImageExtension);
            }
            if (image.Length > _allowedSize)
            {
                return (isUploded: false, errorMessage: Errors.AllowedImageSize);
            }
            #region Save Image On HardDisk
            var path=Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}",imageName);
             using var stream=System.IO.File.Create(path);
            image.CopyTo(stream);
            stream.Dispose();
            if (hasThumbnail)
            {
                var pathThumbnail=Path.Combine($"{_webHostEnvironment.WebRootPath}{folderPath}/thumb",imageName);
                using   var lodedImage=   Image.Load(image.OpenReadStream());
                var width=(float) lodedImage.Width/200;
                var height= lodedImage.Height/width;
                lodedImage.Mutate(i => i.Resize(width: 200, height: (int)height));
                lodedImage.Save(pathThumbnail);
            }
            #endregion
            return (isUploded: true, errorMessage: null);

        }

        public void Delete(string imagePath, string? imageThumbnailPath = null)
        {
            string oldimage=$"{_webHostEnvironment.WebRootPath}{imagePath}";
            if (File.Exists(oldimage)) System.IO.File.Delete(oldimage);
            if(!string.IsNullOrEmpty(imageThumbnailPath))
            {
                string oldThumbnail=$"{_webHostEnvironment.WebRootPath}{imageThumbnailPath!}";
                if (File.Exists(oldThumbnail)) System.IO.File.Delete(oldThumbnail);
            }
        }
    }
}
