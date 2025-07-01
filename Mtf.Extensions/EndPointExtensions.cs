using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static Tuple<string, ushort> GetIpAddressAndPort(this EndPoint endpoint)
        {
            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return new Tuple<string, ushort>(String.Empty, 0);
            }

            var ipAndPort = endpointText.Split(':');
            return new Tuple<string, ushort>(ipAndPort[0], Convert.ToUInt16(ipAndPort[1], CultureInfo.InvariantCulture));
        }

        public static string GetIpAddress(this EndPoint endpoint)
        {
            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return String.Empty;
            }

            var ipAndPort = endpointText.Split(':');
            return ipAndPort[0];
        }

        public static ushort GetPort(this EndPoint endpoint)
        {
            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return 0;
            }

            var ipAndPort = endpointText.Split(':');
            return Convert.ToUInt16(ipAndPort[1], CultureInfo.InvariantCulture);
        }
    }
}
