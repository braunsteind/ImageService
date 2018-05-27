using ImageService.Server;

namespace ImageService.Controller
{
    public interface IImageController
    {
        /// <summary>
        /// Executing the command
        /// </summary>
        /// <param name="commandID"> ID of the command</param>
        /// <param name="args"> relevant arguments </param>
        /// <param name="result"> inidication whether or not command succeeded </param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
        ImageServer Server { get; set; }
    }
}