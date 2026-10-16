using System;

namespace Mtf.Exceptions
{
    public class ConnectionTimedOutException : Exception
    {
        public ConnectionTimedOutException(string serverAddress, uint port, string localEndPoint)
            : this(serverAddress, port, localEndPoint, null)
        { }

        public ConnectionTimedOutException(string serverAddress, uint port, string localEndPoint, Exception innerException)
            : base(BuildMessage(serverAddress, port, localEndPoint), innerException)
        { }

        private static string BuildMessage(string serverAddress, uint port, string localEndPoint) =>
            $"Connection timed out from {localEndPoint} to: {serverAddress}:{port}\r\nCheck the connection timeout, ensure the service is running on the remote machine, verify that you are using the correct username and password, and confirm the firewall settings are correct.";
    }
}
