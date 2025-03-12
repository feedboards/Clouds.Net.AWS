using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime;
using Clouds.Net.AWS.Interfaces;
using Clouds.Net.AWS.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Clouds.Net.AWS.Helpers
{
    public class CognitoHelper : ICognitoHelper
    {
        private readonly AmazonCognitoIdentityProviderClient _providerClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _userPoolId;

        public CognitoHelper(
            string clientId,
            string clientSecret,
            string userPoolId,
            string accessKey,
            string secretKey)
            : this(
                  clientId,
                  clientSecret,
                  userPoolId,
                  accessKey,
                  secretKey,
                  SD.DefaultRegion)
        {
        }

        public CognitoHelper(
            string clientId,
            string clientSecret,
            string userPoolId,
            string accessKey,
            string secretKey,
            string region)
        {
            _clientId = clientId ?? throw new InvalidOperationException("Client ID must be provided.");

            _clientSecret = clientSecret ?? throw new InvalidOperationException("Client Secret must be provided.");

            _userPoolId = userPoolId ?? throw new InvalidOperationException("User Pool ID must be provided.");

            _providerClient =
                new AmazonCognitoIdentityProviderClient(
                    new BasicAWSCredentials(accessKey, secretKey),
                    AWSUtils.GetRegionFromString(region));
        }

        /// <summary>
        /// Asynchronously retrieves a user's information using their access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the user's information.</returns>
        public async Task<AdminDeleteUserResponse> DeleteUserAsync(string email)
        {
            return await _providerClient.AdminDeleteUserAsync(new AdminDeleteUserRequest
            {
                Username = email,
                UserPoolId = _userPoolId
            });
        }

        /// <summary>
        /// Asynchronously deletes a user based on their email address.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation and returns the delete response.</returns>
        public async Task<GetUserResponse> GetUserAsync(string accessToken)
        {
            return await _providerClient.GetUserAsync(new GetUserRequest
            {
                AccessToken = accessToken,
            });
        }

        /// <summary>
        /// Asynchronously initiates an authentication session using an email and password.
        /// </summary>
        /// <param name="email">The email of the user attempting authentication.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the authentication response.</returns>
        public async Task<InitiateAuthResponse> InitiateAuthAsync(string email, string password)
        {
            return await _providerClient.InitiateAuthAsync(new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", email },
                    { "PASSWORD", password },
                    { "SECRET_HASH", GenerateSecretHash(email) }
                }
            });
        }

        /// <summary>
        /// Asynchronously registers a new user with a username, password, and a list of additional attributes.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="attributes">A list of additional user attributes.</param>
        /// <returns>A task that represents the asynchronous operation and returns the sign-up response.</returns>
        public async Task<SignUpResponse> SignUpAsync(string username, string password, List<AttributeType> attributes)
        {
            return await _providerClient.SignUpAsync(new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = GenerateSecretHash(username),
                Username = username,
                Password = password,
                UserAttributes = attributes
            });
        }

        /// <summary>
        /// Retrieves the subject identifier (Sub ID) associated with a given access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>The Sub ID of the user, or an empty string if not found.</returns>
        public string GetSubIdByAccessToken(string accessToken)
        {
            var jsonToken = new JwtSecurityTokenHandler()
                .ReadToken(accessToken) as JwtSecurityToken;
            return jsonToken?.Claims
                .FirstOrDefault(claim => claim.Type == "sub")?.Value ?? string.Empty;
        }

        /// <summary>
        /// Asynchronously retrieves the role of a user using their access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the user's role, or an empty string if not found.</returns>
        public async Task<string> GetUserRoleAsync(string accessToken)
        {
            try
            {
                var responseString = string.Empty;

                foreach (var attribute in
                    (await GetUserAsync(accessToken)).UserAttributes)
                {
                    _ = attribute.Name == "custom:Role" ?
                        responseString = attribute.Value : string.Empty;
                }

                return responseString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void Dispose()
        {
            _providerClient.Dispose();
            GC.SuppressFinalize(this);
        }

        private string GenerateSecretHash(string username)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_clientSecret));
            return Convert.ToBase64String(
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(username + _clientId)));
        }
    }
}
