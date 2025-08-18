using Amazon.SQS.Model;
using iRechargeTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.IService
{
    public interface ISqsService
    {
        /// <summary>
        /// Sends a message to the specified SQS queue.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="messageBody">The body of the message to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendMessageAsync(string queueUrl, string messageBody);
        /// <summary>
        /// Receives messages from the specified SQS queue.
        /// </summary>
        /// <param name="queueUrl">The URL of the SQS queue.</param>
        /// <param name="maxMessages">The maximum number of messages to receive.</param>
        /// <returns>A task that returns a list of received messages.</returns>
        Task<List<Message>> ReceiveMessagesAsync(string queueUrl, int maxMessages = 10);
        Task DeleteMessageAsync(string queueUrl, string receiptHandle);


    }
}
