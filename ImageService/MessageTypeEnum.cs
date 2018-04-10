using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// Enum for representing type of logs
    /// INFO - 0
    /// WARNING - 1
    /// FAIL - 2
    /// </summary>
    public enum MessageTypeEnum : int
    {
        INFO,
        WARNING,
        FAIL
    }
}