using System.ComponentModel.DataAnnotations;

namespace ImageServiceWebApplication.Models
{
    public class Log
    {
        //members
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message Type")]
        public string MessageType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }

    public enum MessageEnum
    {
        INFO,
        FAIL,
        WARNING
    }
}