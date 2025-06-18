using System;

namespace Mtf.Extensions.Exceptions
{
    public class LocalizedException : Exception
    {
        private readonly object[] args;
        private readonly string lastMessage;

        public LocalizedException(string message, params object[] args) : this(message, null, args) { }

        public delegate string TranslatorCallback(string elementIdentifier, int index = 0, params object[] args);

        public static TranslatorCallback Translator { get; set; }

        public LocalizedException(string message, Exception innerException, params object[] args)
            : base(message, innerException)
        {
            this.args = args;

            Exception exception = this;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            lastMessage = exception.Message;
        }

        public string LocalizedMessage =>
            Translator != null ? Translator(lastMessage, 0, args) : lastMessage;
    }
}
