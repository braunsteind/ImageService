using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ImageServiceWebApplication.Models.Config;

namespace ImageServiceWebApplication.Models
{
    public class Photos
    {
        public event PropertyChanged update;
        private static Config config;
        private string outputDir;
        public List<Photo> PhotosList = new List<Photo>();

        /// <summary>
        /// Constructor
        /// </summary>
        public Photos()
        {
            config = new Config();
            config.propertyChanged += Update;
        }
        

        void Update()
        {
            if (config.OutputDirectory != "")
            {
                outputDir = config.OutputDirectory;
                GetPhotos();
                update?.Invoke();
            }
        }

        

        public void GetPhotos()
        {
            //if outputDir is empty, return
            if (outputDir == "")
            {
                return;
            }
            //get thumbDir
            string thumbDir = outputDir + "\\Thumbnails";
            //if directory not exist, return
            if (!Directory.Exists(thumbDir))
            {
                return;
            }
            DirectoryInfo info = new DirectoryInfo(thumbDir);
            
            string[] validExtensions = { ".jpg", ".png", ".gif", ".bmp"};
            foreach (DirectoryInfo yearDirInfo in info.GetDirectories())
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