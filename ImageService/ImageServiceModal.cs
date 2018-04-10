using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members

        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            //check the path exists
            if (File.Exists(path))
            {
                //temp
                result = true;
                return "temp";
            }
            else
            {
                result = false;
                return "Failed";
            }
        }

        public void CreateDir(string path)
        {
            //if output folder doesn't exist, create it
            DirectoryInfo di = Directory.CreateDirectory(this.m_OutputFolder);
            DateTime dt = File.GetCreationTime(path);
        }

        #endregion

    }
}