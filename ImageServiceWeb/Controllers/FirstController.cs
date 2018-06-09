using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageServiceWeb.Models;
using Communication;
using Infrastructure.Logs;
using System.Collections.ObjectModel;

namespace ImageServiceWeb.Controllers
{
    public class FirstController : Controller
    {
        static ConfigModel configModel = new ConfigModel();
        static LogsModel logsModel = new LogsModel();
        static ImageWebModel webModel = new ImageWebModel();
        static PhotosModel photosModel = new PhotosModel();
        private static string removedHandler;

        #region photos
        // GET: First/Photos
        public ActionResult Photos()
        {
            try
            {

                if (configModel.OutputDirectory != string.Empty)
                {
                    string toBeSearched = System.AppDomain.CurrentDomain.BaseDirectory;
                    string str = configModel.OutputDirectory.Substring(configModel.OutputDirectory.IndexOf(toBeSearched) + toBeSearched.Length - 1);
                    str += "\\Thumbnails";
                    photosModel.ThumbnailDirectory = str;

                    photosModel.updateList();
                }
            }
            catch { }
            return View(photosModel);
        }
        /// <summary>
        /// When pressed view on a photo, show the real photo
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult ShowRealPhoto(string path)
        {
            foreach(PhotoArgs photo in photosModel.PhotosList)
            {
                if (photo.realPath == path)
                    return View(photo);
            }
            return View();
        }
        /// <summary>
        /// Delete the photo after pressed "I'm Sure!"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string path)
        {
            foreach (PhotoArgs photo in photosModel.PhotosList)
            {
                if (photo.realPath == path)
                    return View(photo);
            }
            return View();
        }

        // GET: First/MakePhotoDelete/
        public ActionResult MakePhotoDelete(string realPath)
        {
            photosModel.deletePhoto(realPath);
            return RedirectToAction("Photos");
        }
        #endregion photos

        #region ImageWeb
        // GET: First/ImageWebView/
        public ActionResult ImageWebView()
        {
            webModel.checkConnection();
            if (!configModel.alreadyGotConfig)
                configModel.getConfig();
            
            if (configModel.OutputDirectory != string.Empty)
            {
                webModel.GetNumOfImage(configModel.OutputDirectory);
            }
            return View(webModel);
        }
        #endregion ImageWeb
        

        #region logs
        /// <summary>
        /// When log changed
        /// </summary>
        public void LogsChanged()
        {
            Logs();
        }
        // GET: First/Logs
        public ActionResult Logs()
        {
            logsModel.getCurrentLog();
            logsModel.FilterdLogs = logsModel.AllLogs;
            return View(logsModel);
        }

        // POST: First/Logs
        [HttpPost]
        public ActionResult Logs(FormCollection filter)
        {
            try
            {
                string type = Request.Form["FilterBy"];
                logsModel.updateFilterList(type);
                return View(logsModel);
            }
            catch
            {
                return RedirectToAction("ImageWebView");
            }
        }
        #endregion logs
        #region config
        /// <summary>
        /// When config changed
        /// </summary>
        public void ConfigChanged()
        {
            Config();
        }
        // GET: First/Config
        public ActionResult Config()
        {
            if (!configModel.alreadyGotConfig)
                configModel.getConfig();
           
            return View(configModel);
        }
        // GET: First/AreYouSure
        public ActionResult AreYouSure()
        {
            return View(configModel);
        }

        // GET: First/RemoveHandler/
        public ActionResult RemoveHandler(string selectedHandler)
        {
            removedHandler = selectedHandler;
            return RedirectToAction("AreYouSure");
        }

        // GET: First/MakeRemove/
        public ActionResult MakeRemove()
        {
            configModel.RemoveHandler(removedHandler);
            return RedirectToAction("Config");
        }
        #endregion config
    }
}
