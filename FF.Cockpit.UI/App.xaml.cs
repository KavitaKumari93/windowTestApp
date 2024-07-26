using FF.Cockpit.Common;
using Localisation = FF.Cockpit.Common.Properties.Resources;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using System.Globalization;
using System.Linq;
using FF.Cockpit.UI.Properties;
using System.Xaml;
using System.Threading;
using System.Windows.Markup;
using FF.Cockpit.UI.CustomControl;
using localisation = FF.Cockpit.Common.Properties.Resources;
namespace FF.Cockpit.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly string[] m_SupportedCultures = { "de", "es", "fr", "it", "pt", "zh", "tr", "ru", "pl", "en" };
        private static Mutex m_Mutex = null;

        public App()
        {

            System.Threading.Thread threadForCulture = new System.Threading.Thread(delegate () { });
            AppStarter.sysCultureInfo = threadForCulture.CurrentCulture;

            AppStarter.common_SelectedView = CustomSchedulerViewType.Week;
            AppStarter.common_SelectedDate = DateTime.Today;
            AppStarter.common_SelectedResource = null;

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(ConfigurationManager.AppSettings["SyncfusionLicenseKey"]);
            LogHelper.InitiateLogging();
            CryptoString.Key = CryptoString.FFKey;
            CryptoString.IV = CryptoString.FFIV;
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            // To check if any of the cockpit application instance is open
            if (!IsFirstInstance())
            {
                CustomMessageBox.ShowOK(localisation.ApplicationInstanceAlreadyExists_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Information);
                Current.Shutdown();
            }
            else
            {
                base.OnStartup(e);
            }

            LogHelper.LogStep("App start-up.");
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            SetCulture();
            var args = e.Args;
            if (args != null && args.Count() > 0)
            {
                //bypass login if we triggering from Universe cockpit button 
                foreach (string arg in args)
                {
                    MessageBox.Show("Arg =" + arg);
                }
            }
            else
            {
                //open login window here

                AppStarter.LoginWindow = new SystemLoginWindow();
                AppStarter.LoginWindow?.Show();

            }


            base.OnStartup(e);
        }

        private void SetCulture()
        {
            try
            { ////Read initial XML file set through FotoFinder.Universe
                ///
                //Code Here 
                AppStarter.Load();
                //////
                LogHelper.LogStep("AFTER LOAD");
                /// Otherwise set it according to the default system culture or default english
                #region Set the langauge culture
                if (string.IsNullOrEmpty(LanguageHelper.LanguageName))
                {
                    LogHelper.LogStep($"After load language selected: {LanguageHelper.LanguageName}");
                    string selectedUiCulture = new CultureInfo(AppStarter.sysCultureInfo.Name).TwoLetterISOLanguageName;
                    if (!string.IsNullOrEmpty(AppStarter.sysCultureInfo.Name) && m_SupportedCultures.Contains(selectedUiCulture))
                    {
                        LanguageHelper.LanguageName = ConstantHelper.DefaultLanguage;

                    }
                }
                ////set the language as current UI info
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(LanguageHelper.LanguageName);
                LogHelper.LogStep($"Setting up culture: {LanguageHelper.LanguageName}");

            }
            #endregion

            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        static bool IsFirstInstance()
        {
            const string appName = "FFCockpit";
            bool createdNew;

            // Attempt to create a mutex with a unique name
            m_Mutex = new Mutex(true, appName, out createdNew);

            //// If the mutex already exists, another instance is running
            //if (!createdNew)
            //{
            //    MessageBox.Show("Another instance of the application is already running.", "Application Running", MessageBoxButton.OK, MessageBoxImage.Information);
            //    Current.Shutdown();
            //}
            return createdNew;
        }
        static void OnExit(object sender, EventArgs args)
        {
            m_Mutex.ReleaseMutex();
            m_Mutex.Close();
        }

        #region Event Handlers
        /// <summary>
        ///  /// Event to handle the uncaught exceptions
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">DispatcherUnhandledExceptionEventArgs</param>
        public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ConstantHelper.ErrorLogFolder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, string.Format(@"Cockpit.Exception{0}.log", DateTime.Now.ToString("yyyyMMdd_HHmmssfff")));

            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                if (info.Length > ConstantHelper.LOG_MAX_FILESIZE)
                {
                    try
                    {
                        File.Delete(path);
                        Trace.TraceInformation("TraceMessageLogFileDeleted", ConstantHelper.LOG_MAX_FILESIZE);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError(string.Format("TraceMessageErrorDeleteLogFile", ex.ToString()));
                        LogHelper.LogError(ex);
                    }
                }
            }
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Close();
                }
            }
            string messageText = string.Format(Localisation.Error_Description_resx, DateTime.Now.ToString("yyyyMMdd_HHmmssfff"), e.Exception);

            try
            {
                // write the expection details over log filr
                using (TextWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(messageText);
                    writer.Flush();
                    writer.Close();
                }
            }
            finally
            {
                //Trace over log file
                Trace.TraceError(e.Exception.ToString());
                e.Handled = true;

                if (!AppStarter.IsNavigateToPatientDashboard)
                    MessageBox.Show(
                        Application.Current.MainWindow,
                        string.Format(Localisation.Error_AppClose_resx, Localisation.AppTitle_resx, e.Exception.Message),
                        Localisation.Error_Headline_resx, MessageBoxButton.OK, MessageBoxImage.Error);

                //  Application.Current.Shutdown();
            }
        }
        #endregion
    }
}
