using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Common
{
    public class EventArgsHelper<T> : EventArgs
    {
        public EventArgsHelper(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
