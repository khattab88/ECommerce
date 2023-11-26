using ECommerce.Models;
using Newtonsoft.Json;
using Orders.DataAccess;
using Plain.RabbitMQ;

namespace Orders
{
    public class InventoryResponseListener : IHostedService
    {
        private readonly ISubscriber subscriber;
        private readonly IOrderDeletor orderDeletor;

        public InventoryResponseListener(ISubscriber subscriber, IOrderDeletor orderDeletor)
        {
            this.subscriber = subscriber;
            this.orderDeletor = orderDeletor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }

        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<InventoryResponse>(message);

            // SAGA Patterns Rollback with Compensation action (delete order if inverntory update failed)
            if (!response.IsSuccess)
            {
                orderDeletor.Delete(response.OrderId).GetAwaiter().GetResult();
            }
            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
