namespace Clouds.Net.AWS.Infrastructure.Interfaces
{
    public interface IAWSConfigurator
    {
        /// <summary>
        /// Sets the default AWS credentials for the application.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator SetDefaultRegion(string region);
        
        /// <summary>
        /// Sets the default AWS region for the application.
        /// </summary>
        /// <param name="region">The AWS region to be used as the default.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator SetDefaultCredentials(string accessKey, string secretKey);

        /// <summary>
        /// Enables LocalStack for AWS service emulation, optionally specifying a custom LocalStack URL.
        /// </summary>
        /// <param name="url">The LocalStack service URL. Defaults to "http://localhost:4572" if not provided.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator UseLocalStack(string? url = null);

        #region S3
        /// <summary>
        /// Adds AWS S3 storage support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the S3 bucket is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddS3(string bucketName, string region);

        /// <summary>
        /// Adds AWS S3 storage support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the S3 bucket is located.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddS3(string bucketName, string region, string accessKey, string secretKey);

        /// <summary>
        /// Adds AWS S3 storage support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddS3(string bucketName);

        /// <summary>
        /// Adds AWS S3 storage support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddS3(string bucketName, string accessKey, string secretKey);
        #endregion

        #region Congnito
        /// <summary>
        /// Adds AWS Cognito authentication using the configured AWS credentials and default region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddCognito(string clientId, string clientSecret, string userPoolId);

        /// <summary>
        /// Adds AWS Cognito authentication with a specified AWS access key and secret key, using the default AWS region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddCognito(
            string clientId,
            string clientSecret,
            string userPoolId,
            string accessKey,
            string secretKey);

        /// <summary>
        /// Adds AWS Cognito authentication using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <param name="region">The AWS region where Cognito is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddCognito(
            string clientId,
            string clientSecret,
            string userPoolId,
            string region);

        /// <summary>
        /// Adds AWS Cognito authentication with a specified AWS access key, secret key, and region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="region">The AWS region where Cognito is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddCognito(
            string clientId,
            string clientSecret,
            string userPoolId,
            string accessKey,
            string secretKey,
            string region);
        #endregion

        #region SES
        /// <summary>
        /// Adds AWS Simple Email Service (SES) support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSES(string sourceMail);

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <param name="region">The AWS region where SES is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSES(string sourceMail, string region);

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSES(string accessKey, string secretKey, string sourceMail);

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <param name="region">The AWS region where SES is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSES(string accessKey, string secretKey, string sourceMail, string region);
        #endregion

        #region SQS
        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSQS(string queueUrl);

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="region">The AWS region where the SQS queue is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSQS(string queueUrl, string region);

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSQS(string accessKey, string secretKey, string queueUrl);

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="region">The AWS region where the SQS queue is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        IAWSConfigurator AddSQS(string accessKey, string secretKey, string queueUrl, string region);
        #endregion
    }
}
