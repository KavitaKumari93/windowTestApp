//using Localisation = FF.Cockpit.Common.Properties.Resources;
using System;
using System.Diagnostics;
using System.IO;

namespace FF.Cockpit.Common
{
    public static class LogHelper
    {
        public static void InitiateLogging()
        {
            // Local path created to save the log details...
            string logFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ConstantHelper.LogFileFolder);
            string logFileName = string.Format(@"FotoFinder.Cockpit{0}.log", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
            if (!Directory.Exists(logFileFolder))
            {
                Directory.CreateDirectory(logFileFolder);
            }
            //Delete the existing logs file in case file are in bulk
            DeleteOldLogs(logFileFolder);

            string logFile = Path.Combine(logFileFolder, logFileName);
            TraceListener traceListener = new TextWriterTraceListener(logFile);
            traceListener.TraceOutputOptions = TraceOptions.DateTime;
            Trace.Listeners.Add(traceListener);
            Trace.AutoFlush = true;

        }
        public static void LogStep(string stepDescription)
        {
            Trace.TraceInformation(stepDescription);
        }
        public static void LogError(Exception ex)
        {
            if (ex.InnerException != null)
                Trace.TraceError(ex.InnerException?.ToString(), DateTime.Now.ToString(), ex.StackTrace);
            else
                Trace.TraceError(DateTime.Now.ToString(), ex.StackTrace);
        }
        private static void DeleteOldLogs(string logFileFolder)
        {
            try
            {
                if (!string.IsNullOrEmpty(logFileFolder))
                {
                    string[] files = Directory.GetFiles(logFileFolder);

                    DateTime now = DateTime.Now;

                    foreach (var file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        if (fileInfo.CreationTime < now.AddDays(-14))
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception while Deleting the logs:" + ex.ToString());
            }

        }
    }
}
