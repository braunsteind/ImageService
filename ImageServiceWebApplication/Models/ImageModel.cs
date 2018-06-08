using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWebApplication.Models
{
    public class ImageModel
    {

        private static Communication.ICommunicationSingleton client { get; set; }
        //public event NotifyAboutChange NotifyEvent;
        //private static Config m_config;
        //private static string m_outputDir;

        //members
        [Required]
        [Display(Name = "Is Connected")]
        public bool IsConnected { get; set; }

        [Required]
        [Display(Name = "Num of Pics")]
        public int NumofPics { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<StudentInfo> Students { get; set; }



        public ImageModel()
        {
            try
            {
                client = Communication.CommunicationSingleton.Instance;
                IsConnected = client.IsConnected;
                NumofPics = 0;
                //m_config = new Config();
                //m_config.Notify += Notify;
                Students = GetStudents();
            }
            catch (Exception ex)
            {

            }
        }




        public static List<StudentInfo> GetStudents()
        {
            List<StudentInfo> students = new List<StudentInfo>();
            try
            {
                StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Info.txt"));
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] param = line.Split(';');
                    students.Add(new StudentInfo() { FirstName = param[0], LastName = param[1], ID = param[2] });
                }
                file.Close();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Console.WriteLine("Error parsing details of students.");
            }
            return students;
        }

    }
}