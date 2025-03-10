
namespace GoodMoodPerfumeBot.Services
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IWebHostEnvironment environment;
        public UploadImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<List<string>> UploadImage(IFormFile[] filesToUpload)
        {
            var uploadPath = Path.Combine(environment.WebRootPath, "images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<string> uploadedFiles = new List<string>();

            foreach(var file in filesToUpload)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                uploadedFiles.Add(filePath);
            }

            return uploadedFiles;
        }
    }
}
