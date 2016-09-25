using System;
using System.Runtime.Serialization;

namespace Complexity.EsapiApertureMetric
{
    [Serializable]
    public class LeafWidthsNotFoundException : Exception
    {
        public LeafWidthsNotFoundException() { }

        public LeafWidthsNotFoundException(string message)
            : base(message) { }

        public LeafWidthsNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        protected LeafWidthsNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
