using System;
using System.Runtime.Serialization;

namespace FF.Cockpit.Theme
{
    [Serializable]
    public class FFCockpitException : Exception
    {
        public FFCockpitException()
        {
        }

        public FFCockpitException(string message)
            : base(message)
        {
        }

        public FFCockpitException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected FFCockpitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}