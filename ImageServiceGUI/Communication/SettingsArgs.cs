using System;
using System.Collections.Generic;

namespace ImageServiceGUI.Communication
{
    class SettingsArgs
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<string> Handlers { get; set; }

        public SettingsArgs(string outputDir, string sourceName, string logName, string thumbnailSize, List<string> handlers)
        {
            this.OutputDir = outputDir;
            this.SourceName = sourceName;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
            this.Handlers = handlers;
        }
    }
}