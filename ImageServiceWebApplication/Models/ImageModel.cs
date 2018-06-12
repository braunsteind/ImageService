using ImageServiceWebApplication.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static ImageServiceWebApplication.Models.Config;

namespace ImageServiceWebApplication.Models
{
    public class ImageModel
    {

        private static ICommunicationSingleton client { get; set; }
        public event PropertyChanged NotifyEvent;
        private static Config m_config;
        private static string m_outputDir;

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
            client = Communication.CommunicationSingleton.Instance;
            IsConnected = client.IsConnected;
            NumofPics = 0;
            m_outputDir = "";
            m_config = new Config();
            m_config.propertyChanged += Update;
            Students = GetStudents();
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

        public static int GetNumOfPics()
        {
            try
            {
                if (m_outputDir == null || m_outputDir == "")
                {
                    return 0;
                }


                int counter = 0;
                while (m_outputDir == null && (counter < 2)) { System.Threading.Thread.Sleep(1000); counter++; }
                int sum = 0;
                string path = m_outputDir + "\\Thumbnails";
                DirectoryInfo di = new DirectoryInfo(path);
                sum += di.GetFiles("*.JPG", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.GIF", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.PNG", SearchOption.AllDirectories).Length;
                sum += di.GetFiles("*.BMP", SearchOption.AllDirectories).Length;

                return sum;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        void Update()
        {
            if (m_config.OutputDirectory != "")
            {
                m_outputDir = m_config.OutputDirectory;
                NumofPics = GetNumOfPics();
                NotifyEvent?.Invoke();
            }
        }

    }
}