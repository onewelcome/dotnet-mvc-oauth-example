using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace DotnetAspCoreResourceGatewayExample.Model
{
    public class WishList
    {
        public int UserId { get; set; }
        public List<String> wishes { get; set; } = new List<string>();
    }
}