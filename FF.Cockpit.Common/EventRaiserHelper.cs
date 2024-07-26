using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Common
{
    public static class EventRaiserHelper
    {
        public static void Raise(this EventHandler handler, object sender)
        {
            handler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<EventArgsHelper<T>> handler, object sender, T value)
        {
            handler?.Invoke(sender, new EventArgsHelper<T>(value));
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T value) where T : EventArgs
        {
            handler?.Invoke(sender, value);
        }

        public static void Raise<T>(this EventHandler<EventArgsHelper<T>> handler, object sender, EventArgsHelper<T> value)
        {
            handler?.Invoke(sender, value);
        }
    }
}
