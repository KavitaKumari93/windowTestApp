using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace FF.Cockpit.Theme.Controls
{
    /// <summary>
    /// The MetroHeaderAutomationPeer class exposes the <see cref="T:FF.Cockpit.Theme.Controls.MetroHeader" /> type to UI Automation.
    /// </summary>
    public class MetroHeaderAutomationPeer : GroupBoxAutomationPeer
    {
        public MetroHeaderAutomationPeer(GroupBox owner)
            : base(owner)
        {
        }

        protected override string GetClassNameCore()
        {
            return "MetroHeader";
        }
    }
}