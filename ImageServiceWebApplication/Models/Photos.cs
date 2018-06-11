using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ImageServiceWebApplication.Models.Config;

namespace ImageServiceWebApplication.Models
{
    public class Photos
    {
        public event NotifyAboutChange NotifyEvent;
        private static Config m_config;
        private string m_outputDir;
        public List<Photo> PhotosList = new List<Photo>();

        
        public Photos()
        {
            m_config = new Config();
            m_config.Notify += Notify;
        }
        

        void Notify()
        {
            if (m_config.OutputDirectory != "")
            {
                m_outputDir = m_config.OutputDirectory;
                GetPhotos();
                NotifyEvent?.Invoke();
            }
        }

        

        public void GetPhotos()
        {
            if (m_outputDir == "")
            {
                return;
            }
            string thumbnailDir = m_outputDir + "\\Thumbnails";
            if (!Directory.Exists(thumbnailDir))
            {
                return;
            }
            DirectoryInfo di = new DirectoryInfo(thumbnailDir);
            
            string[] validExtensions = { ".jpg", ".png", ".gif", ".bmp"};
            foreach (DirectoryInfo yearDirInfo in di.GetDirectories())
            {
                if (!Path.GetDirectoryName(yearDirInfo.FullName).EndsWith("Thumbnails"))
                {
                    continue;
                }
                foreach (DirectoryInfo monthDirInfo in yearDirInfo.GetDirectories())
                {


                    foreach (FileInfo fileInfo in monthDirInfo.GetFiles())
                    {
                        if (validExtensions.Contains(fileInfo.Extension.ToLower()))
                        {
                            Photo p = PhotosList.Find(x => (x.ImageUrl == fileInfo.FullName));
                            if (p == null)
                            {
                                PhotosList.Add(new Photo(fileInfo.FullName));

                            }
                        }
                    }
                }
            }
        }
    }
}