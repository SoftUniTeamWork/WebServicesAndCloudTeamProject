using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.Services.Providers
{
    public interface IIdProvider
    {
        string GetId();
    }
}