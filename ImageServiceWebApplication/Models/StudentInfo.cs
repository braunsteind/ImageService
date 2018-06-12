using System.ComponentModel.DataAnnotations;

namespace ImageServiceWebApplication.Models
{
    public class StudentInfo
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "ID")]
        public string ID { get; set; }

    }
}