using ImageServiceWebApplication.Models;
using System.Web.Mvc;

namespace ImageServiceWebApplication.Controllers
{
    public class PhotosController : Controller
    {
        public static Photos photos = new Photos();
        private static Photo m_currentPhoto;

        
        public PhotosController()
        {
            photos.NotifyEvent -= Notify;
            photos.NotifyEvent += Notify;

        }

        
        void Notify()
        {
            Photos();
        }

        public ActionResult Photos()
        {
            photos.PhotosList.Clear();
            photos.GetPhotos();
            return View(photos.PhotosList);
        }

        
        public ActionResult PhotosViewer(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        
        public ActionResult DeletePhoto(string photoRelPath)
        {
            UpdateCurrentPhotoFromRelPath(photoRelPath);
            return View(m_currentPhoto);
        }

        
        public ActionResult DeleteYes(string photoRelPath)
        {
            System.IO.File.Delete(m_currentPhoto.ImageUrl);
            System.IO.File.Delete(m_currentPhoto.ImageFullUrl);
            photos.PhotosList.Remove(m_currentPhoto);
            return RedirectToAction("Photos");
        }

        
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