using System;
namespace EventBus.Messages.Common
{
    public class IntegrationBase
    {
        public SecurityContext SecurityContext { get; set; } = new SecurityContext();
        public DateTime CreationDateTime { get; set; }
    }
}