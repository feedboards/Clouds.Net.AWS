using Clouds.Net.AWS.DTOs.Request;

namespace Clouds.Net.AWS.Interfaces
{
    public interface ISESHelper : IDisposable
    {
        /// <summary>
        /// Asynchronously sends an email using the details specified in the <see cref="SESRequestDto"/> object.
        /// </summary>
        /// <param name="obj">The email request object containing recipient, subject, and body details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <c>true</c> if the email is sent successfully; otherwise, <c>false</c>.</returns>
        Task<bool> SendMail(SESRequestDto obj);

        /// <summary>
        /// Asynchronously sends an email using the details specified in the <see cref="SESRequestDto"/> object and allows specifying a source email address.
        /// </summary>
        /// <param name="obj">The email request object containing recipient, subject, and body details.</param>
        /// <param name="sourceMail">The sender's email address.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <c>true</c> if the email is sent successfully; otherwise, <c>false</c>.</returns>
        Task<bool> SendMail(SESRequestDto obj, string sourceMail);
    }
}
