using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity;

namespace WebChat.Services.Providers
{
    public class IdProvider : IIdProvider
    {
        public string GetId()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId();
        }
    }
}