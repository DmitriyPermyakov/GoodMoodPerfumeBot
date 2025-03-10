namespace GoodMoodPerfumeBot.Services
{
    public interface IUploadImageService
    {
        public Task<List<string>> UploadImage(IFormFile[] formFile);
    }
}
