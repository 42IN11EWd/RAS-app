﻿using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Modules
{
    /// <summary>
    /// This module is responsible for managing the settings for the video and laser camera
    /// 
    /// This module will be changed or deleted because the settings for the cameras
    /// will be saved in the camera itself.
    /// </summary>
    class SettingsModule : IVideoCameraSettingsModule, ILaserCameraSettingsModule
    {
        private Settings settings;

        public SettingsModule()
        {
            settings = Settings.Default;
        }

        public void saveVideocameraIndex(int index)
        {
            int currentIndex = settings.VideocameraIndex;
            if (index != currentIndex)
            {
                settings.VideocameraIndex = index;
                settings.Save();
            }
        }

        public int getVideocameraIndex()
        {
            return settings.VideocameraIndex;
        }
    }
}
