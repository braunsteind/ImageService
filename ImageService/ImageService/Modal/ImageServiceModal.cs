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

        //The Output Folder
        private string m_OutputFolder;
        //The Size Of The Thumbnail Size
        private int m_thumbnailSize;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m_OutputFolder">The output folder</param>
        /// <param name="m_thumbnailSize">The thumbnail size</param>
        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            string copyPath;
            string copyThumb;

            //adding this feature to support instant download of images directly to the folder
            System.Threading.Thread.Sleep(500);

            //check if the path exists
            if (File.Exists(path))
            {
                //try to create the out folder
                try
                {
                    CreateDir(path, out copyPath, out copyThumb);
                }
                //if failed
                catch (Exception e)
                {
                    //set result to false
                    result = false;
                    //return fail message
                    return "Failed creating directory. error: " + e.Message;
                }
                
                //get the image from the path
                Image img = Image.FromFile(path);
                //get the thumbnail
                Image thumb = img.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
                //save the image in the copy path
                img.Save(copyPath);
                //save the thumbnail
                thumb.Save(Path.ChangeExtension(copyThumb, "thumb"));
                //dispose
                img.Dispose();
                File.Delete(path);

                //check the copies worked and exists
                if (File.Exists(copyPath) && File.Exists(Path.ChangeExtension(copyThumb, "thumb")))
                {
                    result = true;
                    return copyPath;
                }
            }
            //failure
            result = false;
            return "Failed";
        }

        /// <summary>
        /// Create the relavent directory if not already exists
        /// </summary>
        /// <param name="path">The image path</param>
        /// <param name="copyPath">The copy picture path</param>
        private void CreateDir(string path, out string copyPath, out string copyThumb)
        {
            //if output folder doesn't exist, create it as hiden folder
            DirectoryInfo di = Directory.CreateDirectory(this.m_OutputFolder);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            //get month and year
            DateTime dt = File.GetLastWriteTime(path);
            int month = dt.Month;
            int year = dt.Year;

            //build the path of the copy
            copyPath = this.m_OutputFolder + "\\" + year.ToString() + "\\" + month.ToString();
            copyThumb = this.m_OutputFolder + "\\Thumbnails" + "\\" + year.ToString() + "\\" + month.ToString();

            //create directories
            Directory.CreateDirectory(copyPath);
            Directory.CreateDirectory(copyThumb);

            //get the picture name
            string imageName = path.Substring(path.LastIndexOf("\\"));

            //add the picture name to the copy
            copyPath += imageName;
            copyThumb += imageName;
        }
        #endregion
    }
}