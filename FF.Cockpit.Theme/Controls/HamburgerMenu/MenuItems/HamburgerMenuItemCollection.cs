﻿using System.Windows;

namespace FF.Cockpit.Theme.Controls
{
    /// <summary>
    /// The HamburgerMenuItemCollection provides typed collection of HamburgerMenuItem.
    /// </summary>
    public class HamburgerMenuItemCollection : FreezableCollection<HamburgerMenuItem>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new HamburgerMenuItemCollection();
        }
    }
}
