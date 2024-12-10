namespace Rise.Shared.Minio;

public interface IMinioService
{
    Task<string> UploadImageAsync(string objectName, Stream fileStream, long fileSize, string contentType);
    Task<bool> DeleteImageAsync(string url);
    Task<StreamContent> GetImageAsync(string url);
}