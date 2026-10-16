using Mtf.Extensions.Exceptions;
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

        public static Tuple<string, ushort> GetIpAddressAndPort(this EndPoint endpoint)
        {
            if (endpoint is IPEndPoint ipEndPoint)
            {
                return new Tuple<string, ushort>(ipEndPoint.Address.ToString(), (ushort)ipEndPoint.Port);
            }

            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return new Tuple<string, ushort>(String.Empty, 0);
            }

            if (!endpointText.TryGetIpAddressAndPortFromEndPoint(out var ipAddress, out var port))
            {
                throw new LocalizedException("Port value cannot be parsed from value: {0}.", endpointText);
            }
            return new Tuple<string, ushort>(ipAddress, port);
        }

        public static string GetIpAddress(this EndPoint endpoint)
        {
            if (endpoint is IPEndPoint ipEndPoint)
            {
                return ipEndPoint.Address.ToString();
            }

            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return String.Empty;
            }

            return endpointText.GetIpAddressFromEndPoint();
        }

        public static ushort GetPort(this EndPoint endpoint)
        {
            if (endpoint is IPEndPoint ipEndPoint)
            {
                return (ushort)ipEndPoint.Port;
            }

            var endpointText = endpoint?.ToString();
            if (String.IsNullOrEmpty(endpointText))
            {
                return 0;
            }

            if (!endpointText.TryGetIpAddressAndPortFromEndPoint(out _, out var port))
            {
                throw new LocalizedException("Port value cannot be parsed from value: {0}.", endpointText);
            }
            return port;
        }
    }
}
