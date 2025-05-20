using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using static Mtf.Extensions.Delegates;

namespace Mtf.Extensions
{
    public static class EndPointExtensions
    {
        public const string IpAny = "0.0.0.0";
        public const string IpAnyWithColon = "0.0.0.0:";

        public static string GetEndPointInfo(this EndPoint endpoint, GetLocalIpAddressesCallback getLocalIpAddressesCallback, string separator = "|")
        {
            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return String.Empty;
            }
            if (endpointText.StartsWith(IpAnyWithColon, StringComparison.OrdinalIgnoreCase))
            {
                return $"{endpointText} {String.Join(separator, getLocalIpAddressesCallback(AddressFamily.InterNetwork))}";
            }
            return endpointText;
        }
    }
}
