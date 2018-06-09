using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {

        private List<PhotoArgs> m_photosList= new List<PhotoArgs>();
        private string thumbnailPath = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotosModel()
        {

        }
        /// <summary>
        /// Update the photos list
        /// </summary>
        public void updateList()
        {
            m_photosList.Clear();
            string[] extensions = { "*.png", "*.jpg", "*.gif", "*.bmp" };
            foreach (string extension in extensions)
            {
                getParams(extension);
            }
        }
        /// <summary>
        /// Delete a photo from list and from folder
        /// </summary>
        /// <param name="realPath">Photo's path</param>
        public void deletePhoto(string realPath)
        {
            try
            {
                foreach(PhotoArgs photo in m_photosList)
                {
                    if (photo.realPath == realPath)
                    {
                        File.Delete(photo.FullRealPath);
                        File.Delete(photo.FullThumbPath);
                        m_photosList.Remove(photo);
                        break;
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Get the param of photo
        /// </summary>
        /// <param name="extension">extension of photo</param>
        public void getParams(string extension)
        {
            try
            {
                string workingDir = System.AppDomain.CurrentDomain.BaseDirectory;
                string location = workingDir + ThumbnailDirectory;
                string[] imagePaths = Directory.GetFiles(location, extension, SearchOption.AllDirectories);

                foreach (string path in imagePaths)
                {
                    string name = Path.GetFileNameWithoutExtension(path);
                    string month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(path));
                    string year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(path))));
                    string htmlThumbPath = path.Substring(path.IndexOf(workingDir) + workingDir.Length);
                    string htmlRealPath = htmlThumbPath.Replace("\\Thumbnails", "");
                    string fullThumbPath = path;
                    string fullRealPath = path.Replace("\\Thumbnails", "");
                    string[] allPaths = { htmlRealPath, fullRealPath, htmlThumbPath, fullThumbPath };

                    m_photosList.Add(new PhotoArgs(year, month, name, allPaths));
                }
            }
            catch { }

        }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photos List")]
        public List<PhotoArgs> PhotosList
        {
            get { return m_photosList; }
            set { m_photosList = value; }
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Directory")]
        public string ThumbnailDirectory { get { return thumbnailPath; }
                                           set { thumbnailPath = value; } }

            }
}
