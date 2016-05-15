using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernatorFramework
{

    [global::System.Serializable]
    public class NHibernatorException : Exception
    {
        public NHibernatorException() { }
        public NHibernatorException(string message) : base(message) { }
        public NHibernatorException(string message, Exception inner) : base(message, inner) { }
        protected NHibernatorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}

