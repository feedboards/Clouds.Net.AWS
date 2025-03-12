using Amazon.SQS.Model;

namespace Clouds.Net.AWS.Interfaces
{
    public interface ISQSHelper
    {
        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        Task<List<T?>> WaitForNewMessages<T>();

        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the specified queue URL.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        Task<List<T?>> WaitForNewMessages<T>(string queueUrl);

        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the default queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for new messages in seconds.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        Task<List<T?>> WaitForNewMessages<T>(int waitTimeSeconds);

        /// <summary>
        /// Asynchronously waits for and retrieves multiple new messages of type <typeparamref name="T"/> from the specified queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for new messages in seconds.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        Task<List<T?>> WaitForNewMessages<T>(int waitTimeSeconds, string queueUrl);

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        Task<T?> WaitForNewMessage<T>();

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the specified queue URL.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        Task<T?> WaitForNewMessage<T>(string queueUrl);

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the default queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for a new message in seconds.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        Task<T?> WaitForNewMessage<T>(int waitTimeSeconds);

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the specified queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for a new message in seconds.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        Task<T?> WaitForNewMessage<T>(int waitTimeSeconds, string queueUrl);

        /// <summary>
        /// Asynchronously deletes a list of messages from the default queue.
        /// </summary>
        /// <param name="messages">The list of messages to delete.</param>
        Task DeleteMessages(List<Message> messages);

        /// <summary>
        /// Asynchronously deletes a list of messages from the specified queue.
        /// </summary>
        /// <param name="messages">The list of messages to delete.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        Task DeleteMessages(List<Message> messages, string queueUrl);

        /// <summary>
        /// Asynchronously deletes a single message from the default queue.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        Task DeleteMessage(Message message);

        /// <summary>
        /// Asynchronously deletes a single message from the specified queue.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        Task DeleteMessage(Message message, string queueUrl);

        /// <summary>
        /// Asynchronously adds multiple new messages of type <typeparamref name="T"/> to the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to add.</typeparam>
        /// <param name="messages">The list of messages to add.</param>
        /// <returns>A task representing the asynchronous operation that returns the list of added messages.</returns>
        Task<List<T>> AddNewMessages<T>(List<T> messages) where T : notnull;

        /// <summary>
        /// Asynchronously adds multiple new messages of type <typeparamref name="T"/> to the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to add.</typeparam>
        /// <param name="messages">The list of messages to add.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns the list of added messages.</returns>
        Task<List<T>> AddNewMessages<T>(List<T> messages, string queueUrl) where T : notnull;

        /// <summary>
        /// Asynchronously adds a new message of type <typeparamref name="T"/> to the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to add.</typeparam>
        /// <param name="message">The message to add.</param>
        /// <returns>A task representing the asynchronous operation that returns the added message.</returns>
        Task<T> AddNewMessage<T>(T message) where T : notnull;

        /// <summary>
        /// Asynchronously adds a new message of type <typeparamref name="T"/> to the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to add.</typeparam>
        /// <param name="message">The message to add.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns the added message.</returns>
        Task<T> AddNewMessage<T>(T message, string queueUrl) where T : notnull;
    }
}
