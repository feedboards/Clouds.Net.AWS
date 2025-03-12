using Clouds.Net.AWS.Helpers;
using Clouds.Net.AWS.Infrastructure.Interfaces;
using Clouds.Net.AWS.Infrastructure.Options;
using Clouds.Net.AWS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Clouds.Net.AWS.Infrastructure
{
    public class AWSConfigurator : IAWSConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly AWSOptions _awsOptions;
        private readonly LocalStackOptions _localStackOptions;

        internal IServiceCollection Services
        {
            get { return _services; }
        }

        public AWSConfigurator(IServiceCollection services)
        {
            _services = services;
            _awsOptions = new AWSOptions();
            _localStackOptions = new LocalStackOptions();

            _localStackOptions.UseLocalStack = false;
        }

        // TODO add LocalStack support

        #region Cognito
        /// <summary>
        /// Adds AWS Cognito authentication using the configured AWS credentials and default region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddCognito(string clientId, string clientSecret, string userPoolId)
        {
            ValidateAWSCredentials();
            ValidateAWSDefaultRegion();

            return AddCognito(
                clientId,
                clientSecret,
                userPoolId,
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Cognito authentication with a specified AWS access key and secret key, using the default AWS region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddCognito(string clientId, string clientSecret, string userPoolId, string accessKey, string secretKey)
        {
            ValidateAWSDefaultRegion();

            return AddCognito(
                clientId,
                clientSecret,
                userPoolId,
                accessKey,
                secretKey,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Cognito authentication using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="clientId">The Cognito application client ID.</param>
        /// <param name="clientSecret">The Cognito application client secret.</param>
        /// <param name="userPoolId">The Cognito user pool ID.</param>
        /// <param name="region">The AWS region where Cognito is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddCognito(string clientId, string clientSecret, string userPoolId, string region)
        {
            ValidateAWSCredentials();

            return AddCognito(
                clientId,
                clientSecret,
                userPoolId,
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                region);
        }

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
        public IAWSConfigurator AddCognito(string clientId, string clientSecret, string userPoolId, string accessKey, string secretKey, string region)
        {
            _services.AddSingleton<ICognitoHelper, CognitoHelper>(provider =>
                new CognitoHelper(
                    clientId,
                    clientSecret,
                    userPoolId,
                    accessKey,
                    secretKey,
                    region));

            return this;
        }
        #endregion

        #region S3
        /// <summary>
        /// Adds AWS S3 storage support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the S3 bucket is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddS3(string bucketName, string region)
        {
            ValidateAWSCredentials();

            return AddS3(
                bucketName,
                region,
                _awsOptions.AccessKey,
                _awsOptions.SecretKey);
        }

        /// <summary>
        /// Adds AWS S3 storage support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="region">The AWS region where the S3 bucket is located.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddS3(string bucketName, string region, string accessKey, string secretKey)
        {
            if (_localStackOptions.UseLocalStack)
            {
                _services.AddSingleton<IS3Helper, S3Helper>(provider =>
                    new S3Helper(bucketName, accessKey, secretKey, region, true, _localStackOptions.URL));
            }
            else
            {
                _services.AddSingleton<IS3Helper, S3Helper>(provider =>
                    new S3Helper(bucketName, accessKey, secretKey, region));
            }

            return this;
        }

        /// <summary>
        /// Adds AWS S3 storage support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddS3(string bucketName)
        {
            ValidateAWSCredentials();
            ValidateAWSDefaultRegion();

            return AddS3(
                bucketName,
                SD.DefaultRegion,
                _awsOptions.AccessKey,
                _awsOptions.SecretKey);
        }

        /// <summary>
        /// Adds AWS S3 storage support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="bucketName">The name of the S3 bucket.</param>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddS3(string bucketName, string accessKey, string secretKey)
        {
            ValidateAWSDefaultRegion();

            return AddS3(
                bucketName,
                SD.DefaultRegion,
                accessKey,
                secretKey);
        }
        #endregion

        #region SES
        /// <summary>
        /// Adds AWS Simple Email Service (SES) support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSES(string sourceMail)
        {
            ValidateAWSCredentials();
            ValidateAWSDefaultRegion();

            return AddSES(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                sourceMail,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <param name="region">The AWS region where SES is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSES(string sourceMail, string region)
        {
            ValidateAWSCredentials();

            return AddSES(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                sourceMail,
                region);
        }

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSES(string accessKey, string secretKey, string sourceMail)
        {
            ValidateAWSDefaultRegion();

            return AddSES(
                accessKey,
                secretKey,
                sourceMail,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Simple Email Service (SES) support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="sourceMail">The default sender email address for outgoing emails.</param>
        /// <param name="region">The AWS region where SES is configured.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSES(string accessKey, string secretKey, string sourceMail, string region)
        {
            _services.AddSingleton<SESHelper, SESHelper>(provider =>
                new SESHelper(accessKey, secretKey, sourceMail, region));

            return this;
        }
        #endregion

        #region SQS
        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support using the configured AWS credentials and the default region.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSQS(string queueUrl)
        {
            ValidateAWSCredentials();
            ValidateAWSDefaultRegion();

            return AddSQS(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                queueUrl,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support using the configured AWS credentials and a specified region.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="region">The AWS region where the SQS queue is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSQS(string queueUrl, string region)
        {
            ValidateAWSCredentials();

            return AddSQS(
                _awsOptions.AccessKey,
                _awsOptions.SecretKey,
                queueUrl,
                region);
        }

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support with custom AWS credentials, using the default AWS region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSQS(string accessKey, string secretKey, string queueUrl)
        {
            ValidateAWSDefaultRegion();

            return AddSQS(
                accessKey,
                secretKey,
                queueUrl,
                SD.DefaultRegion);
        }

        /// <summary>
        /// Adds AWS Simple Queue Service (SQS) support with custom AWS credentials and a specified region.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="region">The AWS region where the SQS queue is located.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator AddSQS(string accessKey, string secretKey, string queueUrl, string region)
        {
            _services.AddSingleton<ISQSHelper, SQSHelper>(provider =>
                new SQSHelper(accessKey, secretKey, region, queueUrl));

            return this;
        }
        #endregion

        /// <summary>
        /// Sets the default AWS credentials for the application.
        /// </summary>
        /// <param name="accessKey">The AWS access key.</param>
        /// <param name="secretKey">The AWS secret key.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator SetDefaultCredentials(string accessKey, string secretKey)
        {
            _awsOptions.AccessKey = accessKey;
            _awsOptions.SecretKey = secretKey;

            return this;
        }

        /// <summary>
        /// Sets the default AWS region for the application.
        /// </summary>
        /// <param name="region">The AWS region to be used as the default.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator SetDefaultRegion(string region)
        {
            SD.DefaultRegion = region;

            return this;
        }

        /// <summary>
        /// Enables LocalStack for AWS service emulation, optionally specifying a custom LocalStack URL.
        /// </summary>
        /// <param name="url">The LocalStack service URL. Defaults to "http://localhost:4572" if not provided.</param>
        /// <returns>The current instance of <see cref="IAWSConfigurator"/> for method chaining.</returns>
        public IAWSConfigurator UseLocalStack(string? url = null)
        {
            _localStackOptions.UseLocalStack = true;

            if (url == null)
            {
                _localStackOptions.URL = "http://localhost:4572";

                return this;
            }

            _localStackOptions.URL = url;

            return this;
        }


        #region Validations
        private void ValidateAWSCredentials()
        {
            if (string.IsNullOrEmpty(_awsOptions.SecretKey) && string.IsNullOrEmpty(_awsOptions.AccessKey))
            {
                throw new ArgumentException("Hasn't been set up AWS Credentials");
            }
        }

        private void ValidateAWSDefaultRegion()
        {
            if (string.IsNullOrEmpty(SD.DefaultRegion))
            {
                throw new ArgumentException("Hasn't been set up default aws region");
            }
        }
        #endregion
    }
}