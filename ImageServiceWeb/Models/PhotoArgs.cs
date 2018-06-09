using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceWeb.Models
{
    public class PhotoArgs
    {


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FullThumbPath ")]
        public string FullThumbPath { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FullRealPath")]
        public string FullRealPath { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string year { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string month { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "RealPath")]
        public string realPath { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail Path")]
        public string thumbnailPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="date"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public PhotoArgs(string photoYear, string photoMonth, string photoName, string[] allPaths)
        {
            name = photoName;
            month = photoMonth;
            year = photoYear;
            realPath = allPaths[0];
            FullRealPath = allPaths[1];
            thumbnailPath = allPaths[2];
            FullThumbPath = allPaths[3];
        }
    }
}
