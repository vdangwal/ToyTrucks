using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBase
    {
        public DateTime CreationDateTime { get; set; }
        public SecurityContext SecurityContext { get; set; } = new SecurityContext();
    }

    public class SecurityContext
    {
        public string AccessToken { get; set; }
    }
}