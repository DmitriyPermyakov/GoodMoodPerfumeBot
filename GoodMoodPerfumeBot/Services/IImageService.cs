namespace GoodMoodPerfumeBot.Services
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(IFormFile formFile, string fileName);
        public void DeleteImages(string imagesToDelete);
        public void RenameProductImageFolder(string oldName, string newName);
        public void DeleteFolder(string productName);
    }
}
