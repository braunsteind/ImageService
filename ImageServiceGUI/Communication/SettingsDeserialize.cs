﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    class SettingsDeserialize
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public List<string> Handlers { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="outputDir"></param>
        /// <param name="sourceName"></param>
        /// <param name="logName"></param>
        /// <param name="thumbnailSize"></param>
        /// <param name="handlers"></param>
        public SettingsDeserialize(string outputDir, string sourceName, string logName, string thumbnailSize, List<string> handlers)
        {
            this.OutputDir = outputDir;
            this.SourceName = sourceName;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
            this.Handlers = handlers;
        }
    }
}