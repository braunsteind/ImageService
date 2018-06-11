using System;
using System.Drawing;
using System.IO;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        //The Output Folder
        private string m_OutputFolder;
        //The Size Of The Thumbnail Size
        private int m_thumbnailSize;
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="m_OutputFolder">The output folder</param>
        /// <param name="m_thumbnailSize">The thumbnail size</param>
        /// 
        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }

        public string OutputFolder
        {
            get { return this.m_OutputFolder; }
            set { this.m_OutputFolder = value; }
        }

        // The Size Of The Thumbnail Size
        public int ThumbnailSize
        {
            get { return this.m_thumbnailSize; }
            set { this.m_thumbnailSize = value; }
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
                thumb.Save(Path.ChangeExtension(copyThumb, "jpg"));

                //dispose
                img.Dispose();
                File.Delete(path);

                //check the copies worked and exists
                if (File.Exists(copyPath) && File.Exists(Path.ChangeExtension(copyThumb, "jpg")))
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

            //if the picture name already taken
            if (File.Exists(copyPath))
            {
                //the index to add to the image
                int i = 1;
                //the picture type
                string type = copyPath.Substring(copyPath.LastIndexOf("."));
                //temp name of the image
                string tempName = copyPath.Substring(0, copyPath.Length - type.Length);
                //while name taken - add index by 1
                while (File.Exists(tempName + "(" + i.ToString() + ")" + type))
                {
                    i++;
                }
                //change the image name in the path
                copyPath = copyPath.Substring(0, copyPath.Length - type.Length)
                    + "(" + i.ToString() + ")" + type;
                copyThumb = copyThumb.Substring(0, copyThumb.Length - type.Length)
                    + "(" + i.ToString() + ")" + type;
            }
        }
    }
}