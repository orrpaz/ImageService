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
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="outputFolder">The target folder of the Image </param>
        /// <param name="thumbnailSize">size of thumbnail image. </param>


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
            using (Image myImage = Image.FromFile(path))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = new Regex(":").Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }

        /// <summary>
        /// this method check for the existence of files with tempFileName and increment
        /// the number by one until it finds a name that does not exist in the directory.
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="target">The target Path of the Image</param>
        /// <returns>new full path of Image.</returns>
        public string CheckIfExists(string path,string target)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            // string directoryPath = Path.GetDirectoryName(path);
             string newFullPath = fileNameOnly + extension;

            while (File.Exists(target + newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = tempFileName + extension;
            }
            return newFullPath;
        }
        
        public string AddFile(string path, out bool result)
        {
            System.Threading.Thread.Sleep(100);
            int count=0;
            if (File.Exists(path))
               
            {
                  DateTime date;
                try
                {
                    // get the taken date of the image , if there isnt taken date than get the LastWriteTime
                    date = GetDateTakenFromImage(path);  
                    string year = String.Empty;
                    string month = String.Empty;
                    string msg = string.Empty;
                   
                    year = date.Year.ToString();
                    month = date.Month.ToString();
                    string yearAndMonth = year + "\\" + month;
                    DirectoryInfo directory = Directory.CreateDirectory(m_OutputFolder);
                    directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    // create output folder.
                    string TargetFolder = m_OutputFolder + "\\" + yearAndMonth + "\\";
                    Directory.CreateDirectory(m_OutputFolder + "\\" +yearAndMonth);
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + yearAndMonth);

                    System.Threading.Thread.Sleep(10);
                    string p = CheckIfExists(path, TargetFolder);
                    if (!File.Exists(TargetFolder + p))
                    {
                        File.Move(path, TargetFolder + p);
                        msg = "Added " + p + " to " + TargetFolder;
                        count ++;
                    }
                  

                    if (!File.Exists(m_OutputFolder + "\\" + "Thumbnails" + "\\" + yearAndMonth + "\\" + p))
                    {
                        using (Image image = Image.FromFile(m_OutputFolder + "\\" + yearAndMonth + "\\" + p))
                        using (Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero))
                        {
                            thumb.Save(m_OutputFolder + "\\" + "Thumbnails" + "\\" + yearAndMonth + "\\" + p);
                            count++;
                        }
                    }
                //    if (count == 2) { msg = "Added " + p + " to " + TargetFolder + "and to" + m_OutputFolder + "\\" + "Thumbnails" + "\\" + yearAndMonth + "\\"; }
                    result = true;
                    return msg;
                }
                catch (Exception e)
                {
                    result = false;
                    return "Error in taking date from image" + e.Message;
                }
            }
            else
            {
                result = false;
                return path + "file didnt exist";
            }
        }
        #endregion
    }
}
            