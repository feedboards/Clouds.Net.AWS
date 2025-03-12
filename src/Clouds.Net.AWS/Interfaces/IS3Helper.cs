using Amazon.S3.Model;

namespace Clouds.Net.AWS.Interfaces
{
    public interface IS3Helper : IDisposable
    {
        /// <summary>
        /// Asynchronously checks if an object exists in the S3 bucket using the specified key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
        Task<bool> Exists(string s3Key);

        /// <summary>
        /// Asynchronously checks if an object exists in the specified S3 bucket using the provided key.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the object exists; otherwise, <c>false</c>.</returns>
        Task<bool> Exists(string bucketName, string s3Key);

        /// <summary>
        /// Asynchronously downloads an S3 object to a local file.
        /// </summary>
        /// <param name="path">The local file path where the object should be saved.</param>
        /// <param name="s3Key">The key of the S3 object to download.</param>
        Task DownloadAsync(string localFile, string s3Key);

        /// <summary>
        /// Asynchronously downloads an S3 object from a specified bucket to a local file.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The local file path where the object should be saved.</param>
        /// <param name="s3Key">The key of the S3 object to download.</param>
        Task DownloadAsync(string bucketName, string localFile, string s3Key);

        /// <summary>
        /// Asynchronously deletes an S3 object using its key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to delete.</param>
        Task DeleteAsync(string s3Key);

        /// <summary>
        /// Asynchronously deletes an S3 object from a specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to delete.</param>
        Task DeleteAsync(string bucketName, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a local file to S3 with public read access.
        /// </summary>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        Task UploadOrUpdateAsPublicToReadAsync(string localFile, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a local file to a specified bucket on S3 with public read access.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        Task UploadOrUpdateAsPublicToReadAsync(string bucketName, string localFile, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a stream to S3 with public read access.
        /// </summary>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        Task UploadOrUpdateAsPublicToReadAsync(Stream stream, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a stream to a specified bucket on S3 with public read access.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        Task UploadOrUpdateAsPublicToReadAsync(string bucketName, Stream stream, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a local file to S3.
        /// </summary>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        Task UploadOrUpdateAsync(string localFile, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a local file to a specified bucket on S3.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="path">The path of the local file to upload.</param>
        /// <param name="s3Key">The key to store the file in S3.</param>
        Task UploadOrUpdateAsync(string bucketName, string localFile, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a stream to S3.
        /// </summary>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        Task UploadOrUpdateAsync(Stream stream, string s3Key);

        /// <summary>
        /// Asynchronously uploads or updates a stream to a specified bucket on S3.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="stream">The stream containing the data to upload.</param>
        /// <param name="s3Key">The key to store the data in S3.</param>
        Task UploadOrUpdateAsync(string bucketName, Stream stream, string s3Key);

        /// <summary>
        /// Asynchronously retrieves a list of all S3 objects.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a list of S3 objects.</returns>
        Task<ListObjectsV2Response> GetObjectsV2();

        /// <summary>
        /// Asynchronously retrieves a list of all S3 objects in a specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of S3 objects.</returns>
        Task<ListObjectsV2Response> GetObjectsV2(string bucketName);

        /// <summary>
        /// Asynchronously retrieves an S3 object using its key.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the S3 object response.</returns>
        Task<GetObjectResponse> GetObject(string s3Key);

        /// <summary>
        /// Asynchronously retrieves an S3 object from a specified bucket using its key.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the S3 object response.</returns>
        Task<GetObjectResponse> GetObject(string bucketName, string s3Key);

        /// <summary>
        /// Retrieves the public URL of an S3 object in a specified region.
        /// </summary>
        /// <param name="region">The AWS region where the object is stored.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The public URL of the object.</returns>
        string GetObjectUrlPublicFile(string region, string s3Key);

        /// <summary>
        /// Retrieves the public URL of an S3 object in a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the object is stored.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The public URL of the object.</returns>
        string GetObjectUrlPublicFile(string bucketName, string region, string s3Key);

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from UTC now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByDefaultUTC(string s3Key);

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from UTC now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByDefaultUTC(string bucketName, string s3Key);

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from now
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByDefaultNow(string s3Key);

        /// <summary>
        /// Retrieves a pre-signed URL for an S3 object with a default expiration time of 3 hours from now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByDefaultNow(string bucketName, string s3Key);

        /// <summary>
        /// Retrieves the URL of an S3 object that expires after a specified duration from UTC now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByUTC(string s3Key, TimeSpan expires);

        /// <summary>
        /// Retrieves the URL of an S3 object from a specified bucket that expires after a specified duration from UTC now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByUTC(string bucketName, string s3Key, TimeSpan expires);

        /// <summary>
        /// Retrieves the URL of an S3 object that expires after a specified duration from now.
        /// </summary>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByNow(string s3Key, TimeSpan expires);

        /// <summary>
        /// Retrieves the URL of an S3 object from a specified bucket that expires after a specified duration from now.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="s3Key">The key of the S3 object.</param>
        /// <param name="expires">The expiration duration of the URL.</param>
        /// <returns>The pre-signed URL of the object.</returns>
        string GetObjectUrlByNow(string bucketName, string s3Key, TimeSpan expires);

        /// <summary>
        /// Generates an S3 key for a file.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <returns>The generated S3 key.</returns>
        string GetS3Key(string file, List<string> folders);

        /// <summary>
        /// Generates an S3 key for a file located within specified folders.
        /// </summary>
        /// <param name="file">The file name.</param>
        /// <param name="folders">A list of folder names leading to the file.</param>
        /// <returns>The generated S3 key.</returns>
        string GetS3Key(string file);

        /// <summary>
        /// Extracts the S3 key from a given URL.
        /// </summary>
        /// <param name="url">The URL of the S3 object.</param>
        /// <returns>The extracted S3 key.</returns>
        string GetS3KeyFromUrl(string url);
    }
}
