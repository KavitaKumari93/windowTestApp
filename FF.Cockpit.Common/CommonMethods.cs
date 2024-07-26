using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace FF.Cockpit.Common
{
    public static class CommonMethods
    {
        public static void WriteUnderXMLDoc(string propertyName, object value)
        {
            ///////////GetPath
            var path = AppStarter.AppStartXmlFileURL();
            if (path != null && value != null)
            {
                if (File.Exists(path))
                {
                    XElement appStartXmlFile = XElement.Load(path);

                    var setting = appStartXmlFile.Element(propertyName);
                    if (setting != null)
                        setting.Value = value.ToString();
                    appStartXmlFile.Save(path);
                }
            }
        }
        public static string ReadXMLDoc(string propertyName)
        {
            string finalValue = string.Empty;
            var path = AppStarter.AppStartXmlFileURL();
            if (path != null)
            {
                if (!File.Exists(path))
                {
                    CreateXMLforLogin(path);
                }
                if (File.Exists(path))
                {
                    XElement appStartXmlFile = XElement.Load(path);
                    var setting = appStartXmlFile.Element(propertyName);
                    if (setting != null)
                        finalValue = setting.Value.ToString();
                }
            }
            return finalValue;
        }
        public static void CreateXMLforLogin(string path)
        {
            // Create XML document
            XmlDocument xmlDoc = new XmlDocument();

            // Create root element
            XmlElement appStarterElement = xmlDoc.CreateElement("AppStarter");
            xmlDoc.AppendChild(appStarterElement);

            // Add child elements
            AddChildElement(xmlDoc, appStarterElement, "DataSource", "");
            AddChildElement(xmlDoc, appStarterElement, "InitialCatalog", "");
            AddChildElement(xmlDoc, appStarterElement, "UserName", "");
            AddChildElement(xmlDoc, appStarterElement, "UserPassword", "");
            AddChildElement(xmlDoc, appStarterElement, "IsSaveDatabaseServerDetails", "False");
            AddChildElement(xmlDoc, appStarterElement, "IsSaveDatabseUserDetails", "False");
            AddChildElement(xmlDoc, appStarterElement, "IsWindowsAuthenticationOn", "False");
            AddChildElement(xmlDoc, appStarterElement, "LastSyncDate", "");
            // Save the document to a file
            xmlDoc.Save(path);
            Console.WriteLine("XML file created successfully.");
        }

        static void AddChildElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement childElement = xmlDoc.CreateElement(elementName);
            childElement.InnerText = elementValue;
            parentElement.AppendChild(childElement);
        }
    }
}
