using Amazon.SQS;
using Amazon.SQS.Model;
using Clouds.Net.AWS.Interfaces;
using Clouds.Net.AWS.Utils;
using Newtonsoft.Json;

namespace Clouds.Net.AWS.Helpers
{
    public class SQSHelper : ISQSHelper
    {
        private readonly AmazonSQSClient _client;
        private readonly string _queueUrl;

        public SQSHelper(
            string accessKey,
            string secretKey,
            string region,
            string queueUrl)
        {
            _client = new AmazonSQSClient(accessKey, secretKey, new AmazonSQSConfig
            {
                RegionEndpoint = AWSUtils.GetRegionFromString(region),
            });

            _queueUrl = queueUrl;
        }

        public SQSHelper(
            string accessKey,
            string secretKey,
            string queueUrl)
            : this(
                  accessKey,
                  secretKey,
                  SD.DefaultRegion,
                  queueUrl)
        {
        }

        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        public async Task<List<T?>> WaitForNewMessages<T>()
        {
            return await WaitForNewMessages<T>(_queueUrl);
        }

        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the specified queue URL.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        public async Task<List<T?>> WaitForNewMessages<T>(string queueUrl)
        {
            var messages = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 20
            });

            return messages.Messages.ConvertAll(message => JsonConvert.DeserializeObject<T>(message.Body));
        }

        /// <summary>
        /// Asynchronously waits for and retrieves new messages of type <typeparamref name="T"/> from the default queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for new messages in seconds.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        public async Task<List<T?>> WaitForNewMessages<T>(int waitTimeSeconds)
        {
            return await WaitForNewMessages<T>(waitTimeSeconds, _queueUrl);
        }

        /// <summary>
        /// Asynchronously waits for and retrieves multiple new messages of type <typeparamref name="T"/> from the specified queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the messages to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for new messages in seconds.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of deserialized messages.</returns>
        public async Task<List<T?>> WaitForNewMessages<T>(int waitTimeSeconds, string queueUrl)
        {
            var messages = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = waitTimeSeconds
            });

            return messages.Messages.ConvertAll(message => JsonConvert.DeserializeObject<T>(message.Body));
        }

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        public async Task<T?> WaitForNewMessage<T>()
        {
            return await WaitForNewMessage<T>(_queueUrl);
        }

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the specified queue URL.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        public async Task<T?> WaitForNewMessage<T>(string queueUrl)
        {
            var response = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 20
            });

            var message = response.Messages.Count > 0 ? JsonConvert.DeserializeObject<T>(response.Messages[0].Body) : default;

            return message;
        }

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the default queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for a new message in seconds.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        public async Task<T?> WaitForNewMessage<T>(int waitTimeSeconds)
        {
            return await WaitForNewMessage<T>(waitTimeSeconds, _queueUrl);
        }

        /// <summary>
        /// Asynchronously waits for and retrieves a single new message of type <typeparamref name="T"/> from the specified queue with a specified wait time.
        /// </summary>
        /// <typeparam name="T">The type of the message to retrieve.</typeparam>
        /// <param name="waitTimeSeconds">The time to wait for a new message in seconds.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns a single deserialized message or null if none are available.</returns>
        public async Task<T?> WaitForNewMessage<T>(int waitTimeSeconds, string queueUrl)
        {
            var response = await _client.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = waitTimeSeconds
            });

            var message = response.Messages.Count > 0 ? JsonConvert.DeserializeObject<T>(response.Messages[0].Body) : default;

            return message;
        }

        /// <summary>
        /// Asynchronously deletes a list of messages from the default queue.
        /// </summary>
        /// <param name="messages">The list of messages to delete.</param>
        public async Task DeleteMessages(List<Message> messages)
        {
            await DeleteMessages(messages, _queueUrl);
        }

        /// <summary>
        /// Asynchronously deletes a list of messages from the specified queue.
        /// </summary>
        /// <param name="messages">The list of messages to delete.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        public async Task DeleteMessages(List<Message> messages, string queueUrl)
        {
            var deleteTasks = new List<Task>();

            foreach (var message in messages)
            {
                deleteTasks.Add(_client.DeleteMessageAsync(new()
                {
                    QueueUrl = queueUrl,
                    ReceiptHandle = message.ReceiptHandle
                }));
            }

            await Task.WhenAll(deleteTasks);
        }

        /// <summary>
        /// Asynchronously deletes a single message from the default queue.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        public async Task DeleteMessage(Message message)
        {
            await DeleteMessage(message, _queueUrl);
        }

        /// <summary>
        /// Asynchronously deletes a single message from the specified queue.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        public async Task DeleteMessage(Message message, string queueUrl)
        {
            await _client.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = message.ReceiptHandle
            });
        }

        /// <summary>
        /// Asynchronously adds multiple new messages of type <typeparamref name="T"/> to the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to add.</typeparam>
        /// <param name="messages">The list of messages to add.</param>
        /// <returns>A task representing the asynchronous operation that returns the list of added messages.</returns>
        public async Task<List<T>> AddNewMessages<T>(List<T> messages) where T : notnull
        {
            return await AddNewMessages(messages, _queueUrl);
        }

        /// <summary>
        /// Asynchronously adds multiple new messages of type <typeparamref name="T"/> to the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of the messages to add.</typeparam>
        /// <param name="messages">The list of messages to add.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns the list of added messages.</returns>
        public async Task<List<T>> AddNewMessages<T>(List<T> messages, string queueUrl) where T : notnull
        {
            var tasks = new List<Task>();

            foreach (var message in messages)
            {
                tasks.Add(AddNewMessage(message, queueUrl));
            }

            await Task.WhenAll(tasks);
            return messages;
        }

        /// <summary>
        /// Asynchronously adds a new message of type <typeparamref name="T"/> to the default queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to add.</typeparam>
        /// <param name="message">The message to add.</param>
        /// <returns>A task representing the asynchronous operation that returns the added message.</returns>
        public async Task<T> AddNewMessage<T>(T message) where T : notnull
        {
            return await AddNewMessage(message, _queueUrl);
        }

        /// <summary>
        /// Asynchronously adds a new message of type <typeparamref name="T"/> to the specified queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to add.</typeparam>
        /// <param name="message">The message to add.</param>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <returns>A task representing the asynchronous operation that returns the added message.</returns>
        public async Task<T> AddNewMessage<T>(T message, string queueUrl) where T : notnull
        {
            var serializeMessage = JsonConvert.SerializeObject(message);

            await _client.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = serializeMessage
            });

            return message;
        }
    }
}
