using System.Collections.Generic;
using System.Net.Sockets;

namespace Mtf.Extensions
{
    public static class Delegates
    {
        public delegate IEnumerable<string> GetLocalIpAddressesCallback(AddressFamily addressFamily);
    }
}
