using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PromenaEmployeeManagement.Repository.Service
{
    public class S3Service
    {
        private readonly string _bucketName;
        private readonly string _region;
        private readonly IAmazonS3 _s3Client;

        public S3Service(IConfiguration configuration)
        {
            // Read AWS settings from appsettings.json
            _bucketName = configuration["AWS:BucketName"];
            _region = configuration["AWS:Region"];

            // Read the AWS Access Key and Secret Key from appsettings.json
            var awsAccessKey = configuration["AWS:AccessKey"];
            var awsSecretKey = configuration["AWS:SecretKey"];

            // Create AWS credentials from Access and Secret keys
            var credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

            // Initialize the Amazon S3 client with credentials and region
            _s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.GetBySystemName(_region));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                // Generate a unique file name using a GUID
                var uniqueFileName = $"{Guid.NewGuid()}-{fileName}";

                // Create the upload request
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    Key = uniqueFileName,
                    BucketName = _bucketName,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead  // Makes the file publicly accessible
                };

                // Initialize the TransferUtility for the upload operation
                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                // Construct the public URL of the uploaded file
                return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file to S3", ex);
            }
        }
    }
}
