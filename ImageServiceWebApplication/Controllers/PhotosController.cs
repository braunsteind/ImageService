using ImageServiceWebApplication.Models;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class PhotosController : Controller
    {
        //static members
        public static Photos photos = new Photos();
        private static Photo currentPhoto;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotosController()
        {
            photos.update -= Update;
            photos.update += Update;
        }

        /// <summary>
        /// Generate a new list on each access to
        /// photos page
        /// </summary>
        /// <returns></returns>
        public ActionResult Photos()
        {
            photos.PhotosList.Clear();
            photos.GetPhotos();
            return View(photos.PhotosList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public ActionResult PhotosViewer(string relativePath)
        {
            foreach (Photo photo in photos.PhotosList)
            {
                if (photo.ImagePath == relativePath)
                {
                    currentPhoto = photo;
                    break;
                }
            }
            return View(currentPhoto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativelPath"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string relativelPath)
        {
            foreach (Photo photo in photos.PhotosList)
            {
                if (photo.ImagePath == relativelPath)
                {
                    currentPhoto = photo;
                    break;
                }
            }
            return View(currentPhoto);
        }

        /// <summary>
        /// Deleting both photo and thumbnail
        /// </summary>
        /// <param name="relativelPath"></param>
        /// <returns></returns>
        public ActionResult DeleteYes(string relativelPath)
        {
            System.IO.File.Delete(currentPhoto.Url);
            System.IO.File.Delete(currentPhoto.FullPath);
            photos.PhotosList.Remove(currentPhoto);
            return RedirectToAction("Photos");
        }

        /// <summary>
        /// Update function
        /// </summary>
        void Update()
        {
            Photos();
        }
    }
}