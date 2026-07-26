using Mtf.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using static Mtf.Extensions.Delegates;

namespace Mtf.Extensions
{
    public static class SocketExtensions
    {
        public const string IpAny = "0.0.0.0:";
        public const string IpAnyWithoutColon = "0.0.0.0";

        public static IEnumerable<string> GetLocalIPAddresses(this Socket socket, GetLocalIpAddressesCallback getLocalIpAddressesCallback)
        {
            var ipAddress = socket?.LocalEndPoint?.ToString();
            if (String.IsNullOrEmpty(ipAddress))
            {
                return Enumerable.Empty<string>();
            }
            if (ipAddress.StartsWith(IpAny, StringComparison.OrdinalIgnoreCase))
            {
                return getLocalIpAddressesCallback(AddressFamily.InterNetwork);
            }
            return new string[] { ipAddress.Substring(0, ipAddress.IndexOf(':')) };
        }

        public static string GetLocalIPAddressesInfo(this Socket socket, GetLocalIpAddressesCallback getLocalIpAddressesCallback, string separator = ", ")
        {
            return socket?.LocalEndPoint?.GetEndPointInfo(getLocalIpAddressesCallback, separator);
        }

        public static void Connect(this Socket socket, string serverIp, ushort serverPort, int timeoutMs, GetLocalIpAddressesCallback getLocalIpAddressesCallback)
        {
            if (socket == null)
            {
                throw new ArgumentNullException(nameof(socket));
            }

            if (IsSocketConnected(socket))
            {
                return;
            }

            var result = socket.BeginConnect(serverIp, serverPort, null, null);
            var ipAddress = socket.GetLocalIPAddressesInfo(getLocalIpAddressesCallback);
            if (!result.AsyncWaitHandle.WaitOne(timeoutMs))
            {
                socket.Close();
                throw new ConnectionFailedException(serverIp, serverPort, ipAddress);
            }

            if (!IsSocketConnected(socket))
            {
                socket.Close();
                throw new ConnectionTimedOutException(serverIp, serverPort, ipAddress);
            }
        }

        public static bool IsSocketConnected(this Socket socket) => socket != null && socket.Connected;

        public static void CloseSocket(this Socket socket)
        {
            if (socket == null)
            {
                return;
            }

            try
            {
                if (socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch { }
            finally
            {
                socket.Close();
                socket.Dispose();
            }
        }
    }
}
