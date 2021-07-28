using System;
using Microsoft.AspNetCore.Http;
using Web.Models;

namespace Web.Extensions
{
    public static class RequestCookieCollection
    {
        public static Guid GetCurrentBasketId(this IRequestCookieCollection cookies, Settings settings)
        {
            Guid.TryParse(cookies[settings.BasketIdCookieName], out Guid baskedId);
            return baskedId;
        }
    }
}