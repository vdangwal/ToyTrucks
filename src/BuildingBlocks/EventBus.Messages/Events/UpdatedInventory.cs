namespace EventBus.Messages.Events
{
    public class UpdatedInventory
    {
        public int Quantity { get; set; }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
}