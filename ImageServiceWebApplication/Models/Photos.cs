using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ImageServiceWebApplication.Models.Config;

namespace ImageServiceWebApplication.Models
{
    public class Photos
    {
        public event PropertyChanged update;
        private static Config m_config;
        private string m_outputDir;
        public List<Photo> PhotosList = new List<Photo>();

        
        public Photos()
        {
            m_config = new Config();
            m_config.propertyChanged += Update;
        }
        

        void Update()
        {
            if (m_config.OutputDirectory != "")
            {
                m_outputDir = m_config.OutputDirectory;
                GetPhotos();
                update?.Invoke();
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
                            Photo p = PhotosList.Find(x => (x.Url == fileInfo.FullName));
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