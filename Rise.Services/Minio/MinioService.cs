using System.Net.Http.Headers;
using Minio;
using Minio.DataModel.Args;
using Rise.Shared.Exceptions;
using Rise.Shared.Minio;

namespace Rise.Services.Minio;

public class MinioService : IMinioService
{
    private readonly IMinioClient client;
    private readonly string url;
    private readonly string bucket;

    public MinioService(string endpoint, string region, string accessKey, string secretKey, bool useSSL, string bucket, string url)
    {
        client = new MinioClient()
            .WithEndpoint(endpoint)
            .WithRegion(region)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(useSSL)
            .Build();
        this.bucket = bucket;
        this.url = url;
    }

    public async Task<string> UploadImageAsync(string objectName, Stream fileStream, long fileSize, string contentType)
    {
        if (fileStream == null)
        {
            throw new BadRequestException("The uploaded file is null, please try again.");
        }

        if (!contentType.StartsWith("image/"))
        {
            throw new BadRequestException("The uploaded file is not an image");
        }

        bool found = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
        if (!found)
        {
            throw new Exception("Images cannot be uploaded because of a server error");
        }

        try
        {
            var args = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);
            await client.PutObjectAsync(args).ConfigureAwait(false);
            return $"{url}/{bucket}/{objectName}";
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while uploading the image", e);
        }
    }

    public async Task<bool> DeleteImageAsync(string url)
    {
        try
        {
            // format: http(s)://HOST/BUCKET/FILENAME
            string objectName = url.Split('/').Last();
            await client.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(bucket).WithObject(objectName));
            return true;
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while deleting the image", e);
        }
    }

    public async Task<StreamContent> GetImageAsync(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            url = "default.png";
        }

        try
        {
            string objectName = url.Split('/').Last();

            var stream = new MemoryStream();

            GetObjectArgs getObjectArgs = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithCallbackStream((s) => { s.CopyTo(stream); });

            var statObjectArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            var statObject = await client.StatObjectAsync(statObjectArgs);
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(statObject.ContentType);

            await client.GetObjectAsync(getObjectArgs);
            stream.Position = 0;
            return streamContent;
        }
        catch (Exception e)
        {
            // get the default image if the image is not found
            if (url == "default.png")
            {
                throw new Exception("An error occurred while getting the default image", e);
            }
            else
            {
                return await GetImageAsync("default.png");
            }
        }
    }
}