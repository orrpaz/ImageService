using Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Windows;
using static ImageServiceWeb.Models.ConfigModel;

namespace ImageServiceWeb.Models
{
    public class ImageWebModel
    {
        /// <summary>
        /// Consturctor
        /// </summary>
        public ImageWebModel()
        {
            try
            {
                WebCommunicator client = WebCommunicator.Instance;
                IsConnected = client.ServiceIsOn;
                ImageCount = 0;
                Students = GetStudents();
            }
            catch { }
        }
        /// <summary>
        /// Check for connection with service and if isn't try to connect
        /// </summary>
        public void checkConnection()
        {
            WebCommunicator client = WebCommunicator.Instance;
            if (client.ServiceIsOn == 0)
            {
                bool result;
                client.StartConnection(out result);
                if (result)
                    IsConnected = 1;
                else
                    IsConnected = 0;
            }
            else
            {
                IsConnected = client.ServiceIsOn;
            }
        }


        /// <summary>
        /// Get student details
        /// </summary>
        /// <returns>student details</returns>
        public static List<Students> GetStudents()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/info.txt");
            string[] lines = System.IO.File.ReadAllLines(@path);
            List<Students> students = new List<Students>()
        {
            new Students {   FirstName = lines[0].Split(' ')[0],
                LastName = lines[0].Split(' ')[1],
                ID = lines[0].Split(' ')[2] },
            new Students {   FirstName = lines[1].Split(' ')[0],
                LastName = lines[1].Split(' ')[1],
                ID = lines[1].Split(' ')[2] }
        };
            return students;
        }
        /// <summary>
        /// Get num of images
        /// </summary>
        /// <param name="outputDir">Get num of images</param>
        public void GetNumOfImage(string outputDir)
        {

            try
            {
                ImageCount = 0;
                string[] extensions = { "*.jpg", "*.gif", "*.png", "*.bmp" };
                foreach (string ext in extensions)
                {
                    ImageCount += Directory.GetFiles(outputDir, ext, SearchOption.AllDirectories).Length;
                }
                ImageCount = ImageCount / 2;

            }
            catch { }
        }


        [Required]
        [Display(Name = "Names")]
        public string Names { get; set; }

        [Required]
        [Display(Name = "Is Connected")]
        public int IsConnected { get; set; }

        [Required]
        [Display(Name = "Image Count")]
        public int ImageCount { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Students> Students { get; set; }
    }
}