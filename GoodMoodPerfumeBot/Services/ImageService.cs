
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
        public async Task<List<string>> UploadImageAsync(IFormFile[] filesToUpload, string productName)
        {
            string images = "images";
            var uploadPath = Path.Combine(environment.WebRootPath, images, productName);
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            
            List<string> uploadedFiles = new List<string>();            

            foreach(var file in filesToUpload)
            {              
                var filePath = Path.Combine(uploadPath, file.FileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedFiles.Add(Path.Combine(images, productName, file.FileName));
            }

            return uploadedFiles;
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

        public void DeleteImages(List<string> imagesToDelete)
        {
            if(imagesToDelete.Count() > 0)
            {
                foreach (var item in imagesToDelete)
                {
                    var fullPath = Path.Combine(environment.WebRootPath, item);
                    if(File.Exists(fullPath))
                    {                       
                        File.Delete(fullPath);                        
                    } 
                    else
                    {
                        continue;
                    }
                    
                }
            }
        }

        public void DeleteFolder(string productName)
        {
            string path = Path.Combine(environment.WebRootPath, "images", productName);

            if (Directory.Exists(path))
                Directory.Delete(path);                
        }

        
    }
}
