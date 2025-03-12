using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Clouds.Net.AWS.Interfaces;
using Clouds.Net.AWS.Utils;
using System.Net;
using System.Text;

namespace Clouds.Net.AWS.Helpers
{
    public class S3Helper : IS3Helper
    {
        private readonly AmazonS3Client _client;
        private readonly TransferUtility _transferUtility;
        private readonly string _bucketName;

        /// <summary>
        /// Initializes a new instance of the <see cref="S3Helper"/> class with AWS credentials, bucket name, region, and optional LocalStack configuration.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="region">The AWS region where the S3 bucket is located.</param>
        /// <param name="useLocalStack">Indicates whether to use LocalStack for S3 operations. Defaults to <c>false</c>.</param>
        /// <param name="localStackUrl">The URL of the LocalStack service (required if <paramref name="useLocalStack"/> is <c>true</c>).</param>
        public S3Helper(
            string bucketName,
            string accessKey,
            string secretKey,
            string region,
            bool useLocalStack = false,
            string? localStackUrl = null)
        {
            var config = new AmazonS3Config();

            _bucketName = bucketName;

            config.RegionEndpoint = AWSUtils.GetRegionFromString(region);

            if (useLocalStack)
            {
                config.ServiceURL = localStackUrl;
                config.UseHttp = true;
                config.ForcePathStyle = true;
            }

            _client = new AmazonS3Client(accessKey, secretKey, config);
            _transferUtility = new TransferUtility(_client);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="S3Helper"/> class with AWS credentials and bucket name, using the default AWS region and optional LocalStack configuration.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="useLocalStack">Indicates whether to use LocalStack for S3 operations. Defaults to <c>false</c>.</param>
        public S3Helper(
            string bucketName,
            string accessKey,
            string secretKey,
            bool useLocalStack = false)
            : this(
                  bucketName,
                  accessKey,
                  secretKey,
                  SD.DefaultRegion,
                  useLocalStack)
        {
        }

        /// <summary>
        /// Asynchronously checks if an object exists in the S3 bucket using the specified key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
        public async Task<bool> Exists(string s3Key)
        {
            return await Exists(_bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously checks if an object exists in the specified S3 bucket using the provided key.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
        public async Task<bool> Exists(string bucketName, string s3Key)
        {
            try
            {
                var metadate = await _client.GetObjectMetadataAsync(bucketName, s3Key);

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        /// Asynchronously downloads an S3 object to a local file.
        /// </summary>
        /// <param name="path">The local file path where the object should be saved.</param>
        /// <param name="s3Key">The key of the S3 object to download.</param>
        public async Task DownloadAsync(string path, string s3Key)
        {
            await DownloadAsync(path, _bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously downloads an S3 object from a specified bucket to a local file.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The local file path where the object should be saved.</param>
        /// <param name="s3Key">The key of the S3 object to download.</param>
        public async Task DownloadAsync(string bucketName, string path, string s3Key)
        {
            await _transferUtility.DownloadAsync(path, bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously deletes an S3 object using its key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to delete.</param>
        public async Task DeleteAsync(string s3Key)
        {
            await DeleteAsync(_bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously deletes an S3 object from a specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to delete.</param>
        public async Task DeleteAsync(string bucketName, string s3Key)
        {
            await _client.DeleteObjectAsync(bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a local file to S3 with public read access.
        /// </summary>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        public async Task UploadOrUpdateAsPublicToReadAsync(string path, string s3Key)
        {
            await UploadOrUpdateAsPublicToReadAsync(_bucketName, path, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a local file to a specified bucket on S3 with public read access.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        public async Task UploadOrUpdateAsPublicToReadAsync(string bucketName, string path, string s3Key)
        {
            await _transferUtility.UploadAsync(new TransferUtilityUploadRequest()
            {
                BucketName = bucketName,
                FilePath = path,
                Key = s3Key,
                CannedACL = S3CannedACL.PublicRead
            });
        }

        /// <summary>
        /// Asynchronously uploads or updates a stream to S3 with public read access.
        /// </summary>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        public async Task UploadOrUpdateAsPublicToReadAsync(Stream stream, string s3Key)
        {
            await UploadOrUpdateAsPublicToReadAsync(_bucketName, stream, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a stream to a specified bucket on S3 with public read access.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        public async Task UploadOrUpdateAsPublicToReadAsync(string bucketName, Stream stream, string s3Key)
        {
            await _transferUtility.UploadAsync(new TransferUtilityUploadRequest()
            {
                BucketName = bucketName,
                InputStream = stream,
                Key = s3Key,
                CannedACL = S3CannedACL.PublicRead
            });
        }

        /// <summary>
        /// Asynchronously uploads or updates a local file to S3.
        /// </summary>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        public async Task UploadOrUpdateAsync(string path, string s3Key)
        {
            await UploadOrUpdateAsync(_bucketName, path, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a local file to a specified bucket on S3.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        public async Task UploadOrUpdateAsync(string bucketName, string path, string s3Key)
        {
            await _transferUtility.UploadAsync(path, bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a stream to S3.
        /// </summary>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        public async Task UploadOrUpdateAsync(Stream stream, string s3Key)
        {
            await UploadOrUpdateAsync(_bucketName, stream, s3Key);
        }

        /// <summary>
        /// Asynchronously uploads or updates a stream to a specified bucket on S3.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        public async Task UploadOrUpdateAsync(string bucketName, Stream stream, string s3Key)
        {
            await _transferUtility.UploadAsync(stream, bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously retrieves a list of all S3 objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a list of S3 objects.</returns>
        public async Task<ListObjectsV2Response> GetObjectsV2()
        {
            return await GetObjectsV2(_bucketName);
        }

        /// <summary>
        /// Asynchronously retrieves a list of all S3 objects in a specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of S3 objects.</returns>
        public async Task<ListObjectsV2Response> GetObjectsV2(string bucketName)
        {
            return await _client.ListObjectsV2Async(new()
            {
                BucketName = bucketName,
            });
        }

        /// <summary>
        /// Asynchronously retrieves an S3 object using its key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the S3 object response.</returns>
        public async Task<GetObjectResponse> GetObject(string s3Key)
        {
            return await GetObject(_bucketName, s3Key);
        }

        /// <summary>
        /// Asynchronously retrieves an S3 object from a specified bucket using its key.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the S3 object response.</returns>
        public async Task<GetObjectResponse> GetObject(string bucketName, string s3Key)
        {
            return await _client.GetObjectAsync(bucketName, s3Key);
        }

        /// <summary>
        /// Retrieves the public URL of an S3 object in a specified region.
        /// </summary>
        /// <param name="region">The AWS region where the object is stored.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The public URL of the object.</returns>
        public string GetObjectUrlPublicFile(string region, string s3Key)
        {
            return GetObjectUrlPublicFile(_bucketName, region, s3Key);
        }

        /// <summary>
        /// Retrieves the public URL of an S3 object in a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the object is stored.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The public URL of the object.</returns>
        public string GetObjectUrlPublicFile(string bucketName, string region, string s3Key)
        {
            return $"https://{bucketName}.s3.{region}.amazonaws.com/{s3Key}";
        }

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from UTC now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByDefaultUTC(string s3Key)
        {
            return GetObjectUrlByDefaultUTC(_bucketName, s3Key);
        }

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from UTC now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByDefaultUTC(string bucketName, string s3Key)
        {
            return _client.GetPreSignedURL(new()
            {
                BucketName = bucketName,
                Key = s3Key,
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(3)),
            });
        }

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from now
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByDefaultNow(string s3Key)
        {
            return GetObjectUrlByDefaultNow(_bucketName, s3Key);
        }

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByDefaultNow(string bucketName, string s3Key)
        {
            return _client.GetPreSignedURL(new()
            {
                BucketName = bucketName,
                Key = s3Key,
                Expires = DateTime.Now.Add(TimeSpan.FromHours(3)),
            });
        }

        /// <summary>
        /// Retrieves the URL of an S3 object that expires after a specified duration from UTC now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByUTC(string s3Key, TimeSpan expires)
        {
            return GetObjectUrlByUTC(_bucketName, s3Key, expires);
        }

        /// <summary>
        /// Retrieves the URL of an S3 object from a specified bucket that expires after a specified duration from UTC now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByUTC(string bucketName, string s3Key, TimeSpan expires)
        {
            return _client.GetPreSignedURL(new()
            {
                BucketName = bucketName,
                Key = s3Key,
                Expires = DateTime.UtcNow.Add(expires),
            });
        }

        /// <summary>
        /// Retrieves the URL of an S3 object that expires after a specified duration from now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByNow(string s3Key, TimeSpan expires)
        {
            return GetObjectUrlByNow(_bucketName, s3Key, expires);
        }

        /// <summary>
        /// Retrieves the URL of an S3 object from a specified bucket that expires after a specified duration from now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        public string GetObjectUrlByNow(string bucketName, string s3Key, TimeSpan expires)
        {
            return _client.GetPreSignedURL(new()
            {
                BucketName = bucketName,
                Key = s3Key,
                Expires = DateTime.UtcNow.Add(expires),
            });
        }

        /// <summary>
        /// Generates an S3 key for a file.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The generated S3 key.</returns>
        public string GetS3Key(string file)
        {
            return file;
        }

        /// <summary>
        /// Generates an S3 key for a file located within specified folders.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <param name="folders">A list of folder names leading to the file.</param>
        /// <returns>The generated S3 key.</returns>
        public string GetS3Key(string file, List<string> folders)
        {
            var s3KeyBuilder = new StringBuilder();

            foreach (var folder in folders)
            {
                s3KeyBuilder.AppendLine(folder);
            }

            return $"{s3KeyBuilder}/{file}";
        }

        /// <summary>
        /// Extracts the S3 key from a given URL.
        /// </summary>
        /// <param name="url">The URL of the S3 object.</param>
        /// <returns>The extracted S3 key.</returns>
        public string GetS3KeyFromUrl(string url)
        {
            return Uri.UnescapeDataString(new Uri(url).AbsolutePath[1..]);
        }

        public void Dispose()
        {
            _client.Dispose();
            _transferUtility.Dispose();
        }
    }
}
