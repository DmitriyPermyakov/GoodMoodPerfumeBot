namespace GoodMoodPerfumeBot.Services
{
    public interface IImageService
    {
        public Task<List<string>> UploadImageAsync(IFormFile[] formFile, string fileName);
        public void DeleteImages(List<string> imagesToDelete);
        public void RenameProductImageFolder(string oldName, string newName);
        public void DeleteFolder(string productName);
    }
}
