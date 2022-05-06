using System;
using System.Runtime.Serialization;

namespace SqlServerDEID.Common.Globals
{
    [Serializable]
    internal class CredentialNotFoundException : Exception
    {
        public CredentialNotFoundException()
        {
        }

        public CredentialNotFoundException(string message) : base(message)
        {
        }

        public CredentialNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CredentialNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}