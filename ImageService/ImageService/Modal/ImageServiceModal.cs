using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Logging;
using System.Drawing.Imaging;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;
        private static Regex r = new Regex(":");
        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            m_OutputFolder = outputFolder;
            m_thumbnailSize = thumbnailSize;
        }
        /// <summary>
        /// this method return the date that image was taken.
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>return the date that image was taken.</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                string year = String.Empty;
                string month = String.Empty;
                string msg = string.Empty;
                if (File.Exists(path))
                {
                    // DateTime date = File.GetCreationTime(path);
                    DateTime date = GetDateTakenFromImage(path);
                    year = date.Year.ToString();
                    month = date.Month.ToString();
                    DirectoryInfo directory = Directory.CreateDirectory(m_OutputFolder);
                    directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    //Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                    Directory.CreateDirectory(m_OutputFolder + "\\" + year + "\\" + month);
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                    string TargetFolder = m_OutputFolder + "\\" + year + "\\" + month + "\\";
                    if (!File.Exists(TargetFolder + Path.GetFileName(path)))
                    {
                        File.Copy(path, TargetFolder + Path.GetFileName(path));
                        msg = "Added " + Path.GetFileName(path) + " to " + TargetFolder;
                    }
                    if (!File.Exists((m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path))))
                    {
                        Image image = Image.FromFile(path);
                        Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);
                         thumb.Save(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month + "\\" + Path.GetFileName(path));

                    }
                    result = true;
                    return msg;
                }
                else
                {
                    throw new Exception("File doesn't exists");
                }
            } catch (Exception e)
            {
                // return the exception
                result = false;
                return e.Message;
            }
        }
        #endregion
    }
}
            