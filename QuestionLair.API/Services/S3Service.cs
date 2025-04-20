using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace QuestionLair.API.Services;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IConfiguration config)
    {
        var awsOptions = config.GetSection("Upload");
        var s3Config = new AmazonS3Config()
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions["BucketRegion"] ?? "us-east-1"),
        };

        if (!string.IsNullOrEmpty(awsOptions["MinIOServiceURL"])) // MinIO
        {
            s3Config.ServiceURL = awsOptions["MinIOServiceURL"];
            s3Config.ForcePathStyle = true;
        }


        _s3Client = new AmazonS3Client(
          awsOptions["AccessKey"],
          awsOptions["SecretKey"],
          s3Config
        );

        _bucketName = awsOptions["BucketName"]!;
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        using (Stream fileStream = file.OpenReadStream())
        {
            string fileExt = Path.GetExtension(file.FileName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_");
            string fileName = $"{fileNameWithoutExt}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{fileExt}";

            var request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = file.ContentType,
            };
            await _s3Client.PutObjectAsync(request);
            var serviceUrl = (_s3Client.Config as AmazonS3Config)?.ServiceURL;

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                // MinIO path
                return $"{serviceUrl.TrimEnd('/')}/{_bucketName}/{fileName}";
            }

            // AWS S3 path
            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }
    }
}