using ImageServiceWebApplication.Models;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class PhotosController : Controller
    {
        public static Photos photos = new Photos();
        private static Photo m_currentPhoto;

        /// <summary>
        /// constructor.
        /// </summary>
        public PhotosController()
        {
            photos.NotifyEvent -= Notify;
            photos.NotifyEvent += Notify;

        }

        /// <summary>
        /// Notify function.
        /// notify view about update.
        /// </summary>
        void Notify()
        {
            Photos();
        }

        // GET: Photos
        public ActionResult Photos()
        {
            photos.PhotosList.Clear();
            photos.GetPhotos();
            return View(photos.PhotosList);
        }

        /// <summary>
        /// PhotosViewer function.
        /// </summary>
        /// <param name="photoRelPath"> the pic to be presented</param>
        /// <returns></returns>
        public ActionResult PhotosViewer(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeletePhoto function.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        /// <summary>
        /// DeleteYes function.
        /// confirmation of the delete.
        /// </summary>
        /// <param name="photoRelPath"></param>
        /// <returns></returns>
        public ActionResult DeleteYes(string photoRelPath)
        {

            System.IO.File.Delete(m_currentPhoto.ImageUrl);
            System.IO.File.Delete(m_currentPhoto.ImageFullUrl);
            photos.PhotosList.Remove(m_currentPhoto);
            return RedirectToAction("Photos");
        }

        /// <summary>
        /// UpdateCurrentPhotoFromRelPath function.
        /// updates the current photo.
        /// </summary>
        /// <param name="photoRelPath"></param>
        private void UpdateCurrentPhotoFromRelPath(string photoRelPath)
        {
            foreach (Photo photo in photos.PhotosList)
            {
                if (photo.ImageRelativePath == photoRelPath)
                {
                    m_currentPhoto = photo;
                    break;
                }
            }
        }
    }
}