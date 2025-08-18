using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SQS.Model; // Make sure to add this using

namespace iRechargeTestProject.Application.Service
{
    public class SqsProductUpdateService
    {
        private readonly ISqsService _sqsService;
        private readonly IGenericRepository<ProductModel> _productRepository;
        private readonly string _queueUrl;

        public SqsProductUpdateService(
            ISqsService sqsService,
            IGenericRepository<ProductModel> productRepository)
        {
            _sqsService = sqsService;
            _productRepository = productRepository;
            _queueUrl = Environment.GetEnvironmentVariable("AWS__SqsQueueUrl");
        }

        public async Task SubscribeAndUpdateProductAsync()
        {
            // Receive messages from SQS
            List<Message>? messages = await _sqsService.ReceiveMessagesAsync(_queueUrl);
            if(messages!= null && messages.Any())
            {
                foreach (var message in messages)
                {
                    try
                    {
                        // Expecting message body as JSON: { "ProductId": 123, "Quantity": 10 }
                        var productUpdate = JsonSerializer.Deserialize<ProductUpdateMessage>(message.Body);

                        if (productUpdate != null)
                        {
                            var product = _productRepository.GetAll(p => p.Id == productUpdate.ProductId).FirstOrDefault();
                            if (product != null)
                            {
                                var newValue = product.Quantity - productUpdate.Quantity;
                                product.Quantity = newValue;
                                _productRepository.Update(product);
                                await _productRepository.SaveAsync();
                            }
                        }

                        // Delete the message from the queue after processing
                        await _sqsService.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing SQS message: {ex}");
                    }
                }

            }
        }

        // Helper class for deserialization
        private class ProductUpdateMessage
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
