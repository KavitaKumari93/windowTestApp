

using FF.Cockpit.Common;
using FF.Cockpit.Theme;
using FF.Cockpit.Theme.Controls;
using System;
using System.Linq;
using System.Windows;


namespace FF.Cockpit.UI
{
    public sealed partial class ThemeSettingFlyout : Flyout
    {

        public ThemeSettingFlyout()
        {
            this.InitializeComponent();

            cmbThemeList.ItemsSource = ThemeManager.Themes.GroupBy(x => x.BaseColorScheme).Select(x => x.First()).ToList();
            cmbColorList.ItemsSource = ThemeManager.Themes.GroupBy(x => x.ColorScheme).Select(x => x.First()).ToList();

            FF.Cockpit.Theme.Theme _DefaultTheme = ThemeManager.Themes.GroupBy(x => x.BaseColorScheme).Select(x => x.First()).ToList().FirstOrDefault();
            FF.Cockpit.Theme.Theme _DefaultThemeColor = ThemeManager.Themes.GroupBy(x => x.ColorScheme).Select(x => x.First()).ToList().FirstOrDefault();

            cmbThemeList.SelectedValue = _DefaultTheme;
            cmbColorList.SelectedValue = _DefaultThemeColor;

            #region Change Theme
            ResourceDictionary dic = new ResourceDictionary();
            dic.Source = new System.Uri("pack://application:,,,/FF.Cockpit.Theme;component/FF.Cockpit.Theme.xaml");
            
            ResourceDictionary dic2 = new ResourceDictionary();
            dic2.Source = new System.Uri("pack://application:,,,/FF.Cockpit.UI;component/CustomControl/SchedulerHeaderStyle.xaml");

            //ResourceDictionary dic3 = new ResourceDictionary();
            //dic3.Source = new System.Uri("pack://application:,,,/FF.Cockpit.UI;component/CustomControl/MonthViewStyle.xaml");


            Application.Current.Resources.MergedDictionaries.Add(dic);

            Application.Current.Resources.MergedDictionaries.Add(dic2);

           // Application.Current.Resources.MergedDictionaries.Add(dic3);

            ThemeManager.ChangeTheme(Application.Current, _DefaultTheme.BaseColorScheme + "." + _DefaultThemeColor.ColorScheme);
            this.Theme = FlyoutTheme.Adapt;


            #endregion

            cmbThemeList.SelectionChanged += CmbThemeList_SelectionChanged;
            cmbColorList.SelectionChanged += CmbColorList_SelectionChanged;
        }

        private void CmbThemeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbThemeList.SelectedValue != null)
                {
                    FF.Cockpit.Theme.Theme _SeletedTheme = cmbThemeList.SelectedValue as FF.Cockpit.Theme.Theme;
                    ThemeManager.ChangeThemeBaseColor(Application.Current.Resources, _SeletedTheme.BaseColorScheme);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void CmbColorList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbColorList.SelectedValue != null)
                {
                    FF.Cockpit.Theme.Theme theme = ThemeManager.DetectTheme(Application.Current);

                    FF.Cockpit.Theme.Theme _SeletedThemeColor = cmbColorList.SelectedValue as FF.Cockpit.Theme.Theme;
                    ThemeManager.ChangeTheme(Application.Current, theme.BaseColorScheme + "." + _SeletedThemeColor.ColorScheme);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
    }
}