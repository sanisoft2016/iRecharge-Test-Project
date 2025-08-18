using Amazon.SQS;
using Amazon.SQS.Model;
using iRechargeTestProject.Application.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.Service
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;

        public SqsService(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        public async Task SendMessageAsync(string queueUrl, string messageBody)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = messageBody
            };
            await _sqsClient.SendMessageAsync(request);
        }

        public async Task DeleteMessageAsync(string queueUrl, string receiptHandle)
        {
            var request = new DeleteMessageRequest
            {
                QueueUrl = queueUrl,
                ReceiptHandle = receiptHandle
            };
            await _sqsClient.DeleteMessageAsync(request);
        }

        public async Task<List<Message>> ReceiveMessagesAsync(string queueUrl, int maxMessages = 10)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = maxMessages,
                WaitTimeSeconds = 5 // Long polling for up to 5 seconds
            };

            var response = await _sqsClient.ReceiveMessageAsync(request);
            return response.Messages;
        }
    }
}
