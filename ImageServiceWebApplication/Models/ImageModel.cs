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
        //class members
        private static ICommunicationSingleton client { get; set; }
        public event PropertyChanged UpdateEvent;
        private static Config config;
        private static string outputDir;

        [Required]
        [Display(Name = "Is Connected")]
        public bool IsConnected { get; set; }

        [Required]
        [Display(Name = "Num of Pics")]
        public int TotalPics { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<StudentInfo> StudentsInfo { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public ImageModel()
        {  
            TotalPics = 0;
            outputDir = "";
            client = Communication.CommunicationSingleton.Instance;
            IsConnected = client.IsConnected;
            config = new Config();
            config.propertyChanged += Update;
            StudentsInfo = GetStudents();
        }

        /// <summary>
        /// Get the students' info
        /// </summary>
        /// <returns></returns>
        public static List<StudentInfo> GetStudents()
        {
            List<StudentInfo> students = new List<StudentInfo>();
            try
            {
                StreamReader file = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Info.txt"));
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] info = line.Split(';');
                    students.Add(new StudentInfo() { FirstName = info[0], LastName = info[1], ID = info[2] });
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

        /// <summary>
        /// Get number of pics
        /// </summary>
        /// <returns></returns>
        public static int GetNumOfPics()
        {
            try
            {
                if (outputDir == "")
                {
                    return 0;
                }

                int counter = 0;
                while (outputDir == null && (counter < 2)) { System.Threading.Thread.Sleep(1000); counter++; }
                int pics = 0;
                string path = outputDir + "\\Thumbnails";
                DirectoryInfo di = new DirectoryInfo(path);
                pics += di.GetFiles("*.JPG", SearchOption.AllDirectories).Length;
                pics += di.GetFiles("*.PNG", SearchOption.AllDirectories).Length;
                pics += di.GetFiles("*.BMP", SearchOption.AllDirectories).Length;
                pics += di.GetFiles("*.GIF", SearchOption.AllDirectories).Length;

                return pics;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in getting pics number " + e.Message);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            if (config.OutputDirectory != "")
            {
                outputDir = config.OutputDirectory;
                TotalPics = GetNumOfPics();
                UpdateEvent?.Invoke();
            }
        }
    }
}