using Amazon.CognitoIdentityProvider.Model;

namespace Clouds.Net.AWS.Interfaces
{
    public interface ICognitoHelper : IDisposable
    {
        /// <summary>
        /// Asynchronously retrieves a user's information using their access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the user's information.</returns>
        Task<GetUserResponse> GetUserAsync(string accessToken);

        /// <summary>
        /// Asynchronously deletes a user based on their email address.
        /// </summary>
        /// <param name="email">The email address of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation and returns the delete response.</returns>
        Task<AdminDeleteUserResponse> DeleteUserAsync(string email);

        /// <summary>
        /// Asynchronously initiates an authentication session using an email and password.
        /// </summary>
        /// <param name="email">The email of the user attempting authentication.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the authentication response.</returns>
        Task<InitiateAuthResponse> InitiateAuthAsync(string email, string password);

        /// <summary>
        /// Asynchronously registers a new user with a username, password, and a list of additional attributes.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="attributes">A list of additional user attributes.</param>
        /// <returns>A task that represents the asynchronous operation and returns the sign-up response.</returns>
        Task<SignUpResponse> SignUpAsync(string username, string password, List<AttributeType> attributes);

        /// <summary>
        /// Retrieves the subject identifier (Sub ID) associated with a given access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>The Sub ID of the user, or an empty string if not found.</returns>
        Task<string> GetUserRoleAsync(string accessToken);
        
        /// <summary>
        /// Asynchronously retrieves the role of a user using their access token.
        /// </summary>
        /// <param name="accessToken">The access token of the user.</param>
        /// <returns>A task that represents the asynchronous operation and returns the user's role, or an empty string if not found.</returns>
        string GetSubIdByAccessToken(string accessToken);
    }
}
