using System;

namespace FF.Cockpit.Common
{
    public sealed class SchedulerFilterEvent
    {
        public static SchedulerFilterEvent SchedulerFilterEventInstance { get; } = new SchedulerFilterEvent();

        public event EventHandler<EventArgs> SchedulerFilterEventHandler;

        public static void RaiseSchedulerFilterEvent(object sender = null)
            => SchedulerFilterEventInstance.SchedulerFilterEventHandler?.Invoke(sender ?? SchedulerFilterEventInstance, EventArgs.Empty);
    }
}
