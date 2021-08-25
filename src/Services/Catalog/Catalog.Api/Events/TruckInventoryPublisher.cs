using System;
using System.Threading.Tasks;
using MassTransit;
using EventBus.Messages.Events;
namespace Catalog.Api.Events
{
    public class TruckInventoryPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TruckInventoryPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task GetTruckInventory()
        {
            var eventMessage = new InventoryWanted();
            await _publishEndpoint.Publish(eventMessage);
        }
    }
}