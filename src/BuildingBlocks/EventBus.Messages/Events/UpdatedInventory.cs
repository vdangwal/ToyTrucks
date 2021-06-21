namespace EventBus.Messages.Events
{
    public class UpdatedInventory
    {
        public int Quantity { get; set; }

        //public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}