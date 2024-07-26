using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Data;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Forms;
using System.Globalization;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.Common
{
    public static class Extension
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static async Task ExportToPDF(DataTable dt, string strFilePath, string title)
        {
            strFilePath += ".pdf";
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFilePath, FileMode.Create));
            document.Open();


            PdfPTable table = new PdfPTable(dt.Columns.Count);
            PdfPCell cell = null;

            Array floatArray = Array.CreateInstance(typeof(float), dt.Columns.Count);
            for (int i = 0; i < dt.Columns.Count; i++) floatArray.SetValue(4f, i);
            table.SetWidths((float[])floatArray);



            iTextSharp.text.Font ColFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
            Chunk chunkCols = new Chunk(title, ColFont);
            cell = new PdfPCell(new iTextSharp.text.Paragraph(chunkCols));
            cell.Colspan = dt.Columns.Count;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 5;

            table.AddCell(cell);

            foreach (DataColumn c in dt.Columns)
            {
                iTextSharp.text.Font ColHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
                Chunk chunkColHeader = new Chunk(c.ColumnName, ColHeaderFont);
                cell = new PdfPCell(new iTextSharp.text.Paragraph(chunkColHeader));
                cell.Padding = 1;

                table.AddCell(cell);
            }
            int targetColumnIndex = 9;
            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        iTextSharp.text.Font ColValueFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);
                        Chunk chunkColValue = new Chunk(r[i].ToString(), ColValueFont);
                        cell = new PdfPCell(new iTextSharp.text.Paragraph(chunkColValue));
                        cell.Padding = 1;


                        // Set font color for a specific column
                        if (i == targetColumnIndex)
                            cell.Phrase = new Phrase(r[i].ToString(), new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.HELVETICA, 5, BaseColor.BLUE)));
                        table.AddCell(cell);
                    }
                }
            }
            document.Add(table);
            document.Close();
            await Task.FromResult(0);
        }
        public static async Task ExportToCSV(DataTable dt, string strFilePath)
        {
            strFilePath += ".csv";
            StreamWriter sw = new StreamWriter(strFilePath, false);

            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    await sw.WriteAsync(",");
                }
            }
            await sw.WriteAsync(sw.NewLine);

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        await sw.WriteAsync(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                await sw.WriteAsync(sw.NewLine);
            }
            sw.Close();
        }

        public static string GetFilePath(string moduleName,bool isPDF = false)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.Title = localisation.SaveExportFilesHeader_resx;
            saveFileDialog.FileName = $"{moduleName}_{DateTime.Now.ToString("dd.MM.yyyy hh.mm tt", CultureInfo.InvariantCulture)}";
            //saveFileDialog1.CheckFileExists = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.RestoreDirectory = true;

            saveFileDialog.Filter = isPDF ? "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*" : "Excel files (*.csv)|*.csv|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), Path.GetFileNameWithoutExtension(saveFileDialog.FileName));
            }
            else
                return string.Empty;

            //string exportFileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderName);
            //string exportFileName = string.Format(@"{0}", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
            //if (!Directory.Exists(exportFileFolder))
            //{
            //    Directory.CreateDirectory(exportFileFolder);
            //}

            //return Path.Combine(exportFileFolder, exportFileName);
        }
        public static Dictionary<double, string> GetTimeWithMinutes(double fromTime, double toTime)
        {
            Dictionary<double, string> dic = new Dictionary<double, string>();

            for (double i = fromTime; i <= toTime; i++)
            {
                var time = TimeSpan.FromMinutes(i);
                dic.Add(i, string.Format("{0:00}:{1:00}", (int)time.TotalHours, time.Minutes));
                i = i + 4;
            }

            return dic;
        }

        public static Dictionary<int, string> GetEnumDictionary<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T is not an Enum type");

            return Enum.GetValues(typeof(T))
                .Cast<object>()
                .ToDictionary(k => (int)k, v => ((Enum)v).GetEnumDescription());
        }

        public static double ConvertToDouble(this string Value)
        {
            if (Value == null)
            {
                return 0;
            }
            else
            {
                double OutVal;
                double.TryParse(Value, out OutVal);

                if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                {
                    return 0;
                }
                return OutVal;
            }
        }
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime FirstDayOfPreviousWeek(this DateTime dt)
        {
            return dt.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
        }

        public static DateTime FirstDayOfNextWeek(this DateTime dt)
        {
            return dt.StartOfWeek(DayOfWeek.Monday).AddDays(7);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            var col = new ObservableCollection<T>();
            foreach (var cur in enumerable)
            {
                col.Add(cur);
            }
            return col;
        }
        public static string GetEnumDescription(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
        public static String withSuffix(long count)
        {
            if (count < 1000) return "" + count;
            int exp = (int)(Math.Log(count) / Math.Log(1000));
            return String.Format("%.1f %c", count / Math.Pow(1000, exp), "kMGTPE".ElementAt(exp - 1));
        }
        public static byte[] GetBytesFromImagePath(this string imagePath)
        {
            FileStream fs;
            BinaryReader br;
            try
            {
                using (fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    using (br = new BinaryReader(fs))
                    {
                        br = new BinaryReader(fs);
                        return br.ReadBytes((int)fs.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return null;
        }
        public static BitmapImage GetBitmapImageFromBytes(this byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public static BitmapImage GetBitmapImageFromImagePath(this string imagePath)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                using (FileStream stream = File.OpenRead(imagePath))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }
                return image;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return null;
        }

        public static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }


        /// <summary>
        /// Keyboard Accellerators are used in Windows to allow easy shortcuts to controls like Buttons and 
        /// MenuItems. These allow users to press the Alt key, and a shortcut key will be highlighted on the 
        /// control. If the user presses that key, that control will be activated.
        /// This method checks a string if it contains a keyboard accellerator. If it doesn't, it adds one to the
        /// beginning of the string. If there are two strings with the same accellerator, Windows handles it.
        /// The keyboard accellerator character for WPF is underscore (_). It will not be visible.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TryAddKeyboardAccellerator(this string input)
        {
            const string accellerator = "_";            // This is the default WPF accellerator symbol - used to be & in WinForms

            // If it already contains an accellerator, do nothing
            if (input.Contains(accellerator)) return input;

            return accellerator + input;
        }
    }
}