using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ImageServiceWebApplication.Models
{
    public class Photo
    {
        
        public Photo(string imagePath)
        {
            ImageUrl = imagePath;
            ImageFullUrl = imagePath.Replace(@"Thumbnails\", string.Empty);
            Name = Path.GetFileNameWithoutExtension(ImageUrl);
            Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(ImageUrl));
            Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(ImageUrl))));
            string strDirName;
            int intLocation, intLength;
            intLength = imagePath.Length;
            intLocation = imagePath.IndexOf("OutputDir");
            strDirName = imagePath.Substring(intLocation, intLength - intLocation);
            ImageRelativePathThumbnail = @"~\" + strDirName;
            ImageRelativePath = ImageRelativePathThumbnail.Replace(@"Thumbnails\", string.Empty);
        }



        //members
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Year")]
        public string Year { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePath")]
        public string ImageRelativePathThumbnail { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePath")]
        public string ImageRelativePath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageRelativePath")]
        public string ImageFullUrl { get; set; }
    }
}