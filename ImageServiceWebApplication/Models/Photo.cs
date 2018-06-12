using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ImageServiceWebApplication.Models
{
    public class Photo
    {
        
           
        public Photo(string imagePath)
        {
            Url = imagePath;
            FullPath = imagePath.Replace(@"Thumbnails\", string.Empty);
            Name = Path.GetFileNameWithoutExtension(Url);
            Month = Path.GetFileNameWithoutExtension(Path.GetDirectoryName(Url));
            Year = Path.GetFileNameWithoutExtension(Path.GetDirectoryName((Path.GetDirectoryName(Url))));
            string substring;
            int index, length;
            length = imagePath.Length;
            index = imagePath.IndexOf("OutputDir");
            substring = imagePath.Substring(index, length - index);
            ThumbnailPath = @"~\" + substring;
            ImagePath = ThumbnailPath.Replace(@"Thumbnails\", string.Empty);
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
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "FullPath")]
        public string FullPath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ThumbnailPath")]
        public string ThumbnailPath { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImagePath")]
        public string ImagePath { get; set; }
    }
}