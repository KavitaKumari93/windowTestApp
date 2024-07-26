using iTextSharp.xmp.impl.xpath;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Xml;
using System.Xml.Linq;
namespace FF.Cockpit.Common
{
    public static class AppStarter
    {
        #region Properties
        private static int _selectedPatientId;
        public static int SelectedPatientId
        {
            get { return _selectedPatientId; }
            set { _selectedPatientId = value; }
        }

        private static bool _isPatientNameAnontmized;
        public static bool IsPatientNameAnontmized
        {
            get { return _isPatientNameAnontmized; }
            set { _isPatientNameAnontmized = value; }
        }

        private static bool _isServiceTabVisible;
        public static bool IsServiceTabVisible
        {
            get { return _isServiceTabVisible; }
            set { _isServiceTabVisible = value; }
        }
        private static ObservableCollection<object> _selectedPatientsIdForCleanStorage;
        public static ObservableCollection<object> SelectedPatientsIdForCleanStorage
        {
            get { return _selectedPatientsIdForCleanStorage; }
            set { _selectedPatientsIdForCleanStorage = value; }
        }

        private static int _cleanLocalStorageProgessValue;
        public static int CleanLocalStorageProgessValue
        {
            get { return _cleanLocalStorageProgessValue; }
            set { _cleanLocalStorageProgessValue = value; }
        }

        private static bool _isCleanLocalStorageRunning;
        public static bool IsCleanLocalStorageRunning
        {
            get { return _isCleanLocalStorageRunning; }
            set { _isCleanLocalStorageRunning = value; }
        }
        private static int _totalFilesForCleanStorage;
        public static int TotalFilesForCleanStorage
        {
            get { return _totalFilesForCleanStorage; }
            set { _totalFilesForCleanStorage = value; }
        }


        private static bool _isDownloadNow_InProgress;
        public static bool IsDownloadNow_InProgress
        {
            get { return _isDownloadNow_InProgress; }
            set { _isDownloadNow_InProgress = value; }
        }

        private static int _downloadedImageCount_InProgress;
        public static int DownloadedImageCount_InProgress
        {
            get { return _downloadedImageCount_InProgress; }
            set { _downloadedImageCount_InProgress = value; }
        }

        private static int _totalDownloadableImageCount_InProgress;
        public static int TotalDownloadableImageCount_InProgress
        {
            get { return _totalDownloadableImageCount_InProgress; }
            set { _totalDownloadableImageCount_InProgress = value; }
        }

        #region Scheduler Filter

        public static CultureInfo sysCultureInfo { get; set; }
        public static object Appointment { get; set; }

        public static SchedulerTypes common_SchedulerType { get; set; }
        public static CustomSchedulerViewType common_SelectedView { get; set; }
        public static DateTime common_SelectedDate { get; set; }
        public static object common_SelectedResource { get; set; }
        #endregion

        public static Popup IsPerformanceCalendarPopUpOpen { get; set; }
        public static Popup IsSchedulerCalendarPopUpOpen { get; set; }
        public static object mainVM = null;
        public static string DatabaseVersion = string.Empty;
        public static Window UserLoginWindow = null;
        public static Window LoginWindow = null;
        public static Window MainWindow = null;
        private static XElement appStartXmlFile = null;
        public static int ValidatedRoleId = 0;
        public static object LoginModel = null;

        public static bool IsNavigateToPatientDashboard;
        public static bool IsNavigateFromOperationalDashboard;
        public static bool IsNavigateFromPerformanceDashboard;

        public static string AppStartXmlFilePath
        {
            get
            {
                string appStartXmlFilePath = ConstantHelper.XmlFileName;
                string appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (Directory.Exists(appBaseDir))
                {
                    appStartXmlFilePath = Path.Combine(appBaseDir, appStartXmlFilePath);
                }
                return appStartXmlFilePath;
            }
        }
        public static string CockpitSettingXmlFilePath
        {
            get
            {
                string appStartXmlFilePath = ConstantHelper.CockpitXMLSettingFileName;
                string appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (Directory.Exists(appBaseDir))
                {
                    appStartXmlFilePath = Path.Combine(appBaseDir, appStartXmlFilePath);
                }
                return appStartXmlFilePath;
            }
        }

        #endregion

        #region Methods
        public static void Load()
        {
            try
            {
                string universeProgramDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                       @"FotoFinderCockpit Systems\FotoFinderCockpit\CockpitSettings.xml");

                if (File.Exists(universeProgramDataPath))
                    ReadFromCockpitSettings(universeProgramDataPath, "FotoFinder.Cockpit/Settings/Language");
                else
                {
                    CockpitSettingXmlFileURL();
                    ReadFromCockpitSettings(universeProgramDataPath, "FotoFinder.Cockpit/Settings/Language");
                }
            }
            catch (Exception ex)
            {

                LogHelper.LogError(ex);
            }
        }
        public static string AppStartXmlFileURL()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                    @"FotoFinderCockpit Systems\FotoFinderCockpit");
            //string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ConstantHelper.XmlFilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return Path.Combine(folderPath, ConstantHelper.XmlFileName);

        }
        public static string CockpitSettingXmlFileURL()
        {
            string cockpitSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                    @"FotoFinderCockpit Systems\FotoFinderCockpit");
            cockpitSettingsPath = cockpitSettingsPath.ToString().Replace(@"\\", @"\");
            if (!Directory.Exists(cockpitSettingsPath))
            {
                Directory.CreateDirectory(cockpitSettingsPath);
            }
            return CockpitSettingPath(cockpitSettingsPath); ;

        }
        public static string CockpitSettingPath(string cockpitSettingsPath)
        {
            if (Directory.Exists(cockpitSettingsPath))
            {
                var path = Path.Combine(cockpitSettingsPath, ConstantHelper.CockpitXMLSettingFileName);
                {
                    if (!File.Exists(path))
                    {
                        CreateSettingXML(path);
                    }

                }
            }
            return Path.Combine(cockpitSettingsPath, ConstantHelper.CockpitXMLSettingFileName);
        }
        public static bool CreateSettingXML(string filePath)
        {
            bool result = false;
            try
            {
                // Create an XML document
                XmlDocument doc = new XmlDocument();

                // Create the root element (parent node) named "FotoFinder.Cockpit"
                XmlElement cockpitSettingElement = doc.CreateElement("FotoFinder.Cockpit");
                doc.AppendChild(cockpitSettingElement);

                // Create child node named "Settings"
                XmlElement settingsElement = doc.CreateElement("Settings");
                cockpitSettingElement.AppendChild(settingsElement);

                // Create child node named "language" and set its value
                XmlElement languageElement = doc.CreateElement("Language");
                LanguageHelper.LanguageName = languageElement.InnerText = ConstantHelper.DefaultLanguage;
                settingsElement.AppendChild(languageElement);
                // Save the XML document to the specified file path
                doc.Save(filePath);
                result = true;
            }
            catch
            {

                result = false;
            }
            return result;
        }

        public static void ReadFromCockpitSettings(string filepath, string xmlNodePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            // Navigate to the language element using XPath
            XmlNode languageNode = doc.SelectSingleNode(xmlNodePath);

            // Check if the language node exists
            if (languageNode != null)
            {
                // Get the language value
                LanguageHelper.LanguageName = languageNode.InnerText;
            }
        }
        

        #endregion
    }
}
