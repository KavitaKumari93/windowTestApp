using System.Windows.Controls.Primitives;

namespace FF.Cockpit.Theme.Controls
{
    public class MetroThumbContentControlDragCompletedEventArgs : DragCompletedEventArgs
    {
        public MetroThumbContentControlDragCompletedEventArgs(double horizontalOffset, double verticalOffset, bool canceled)
            : base(horizontalOffset, verticalOffset, canceled)
        {
            this.RoutedEvent = MetroThumbContentControl.DragCompletedEvent;
        }
    }
}