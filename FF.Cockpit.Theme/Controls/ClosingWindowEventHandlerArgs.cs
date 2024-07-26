using System;

namespace FF.Cockpit.Theme.Controls
{
    public class ClosingWindowEventHandlerArgs : EventArgs
    {
        public bool Cancelled { get; set; }
    }
}