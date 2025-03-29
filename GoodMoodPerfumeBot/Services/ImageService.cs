
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System.IO;

namespace GoodMoodPerfumeBot.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment environment;
        public ImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<string> UploadImageAsync(IFormFile fileToUpload, string productName)
        {
            string images = "images";
            var uploadPath = Path.Combine(environment.WebRootPath, images, productName);
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileNameWithoutSpaces = fileToUpload.FileName.Replace(" ", "_");

            string uploadedFiles;
            var filePath = Path.Combine(uploadPath, fileNameWithoutSpaces);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileToUpload.CopyToAsync(stream);
            }

            uploadedFiles = Path.Combine(images, productName, fileNameWithoutSpaces);

            return "http://localhost:5070/" + uploadedFiles;
        }

        public void RenameProductImageFolder(string oldName, string newName)
        {
            string oldPath = Path.Combine(environment.WebRootPath, "images", oldName);
            string newPath = Path.Combine(environment.WebRootPath, "images", newName);


            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
            }
        }

        public void DeleteImages(string imageToDelete)
        {
            if (string.IsNullOrEmpty(imageToDelete))
                return;
            var fullPath = Path.Combine(environment.WebRootPath, imageToDelete);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public void DeleteFolder(string productName)
        {
            string path = Path.Combine(environment.WebRootPath, "images", productName);

            if (Directory.Exists(path))
                Directory.Delete(path);                
        }

        
    }
}
