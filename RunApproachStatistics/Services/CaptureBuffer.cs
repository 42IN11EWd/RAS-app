using AForge.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Services
{
    class CaptureBuffer
    {
        private List<Bitmap> captureBuffer;
        private List<Bitmap> dynamicBuffer;
        public static int fps { get; set; }
        public static int width { get; set; }
        public static int height { get; set; }
        private bool modifyingBuffer;

        public List<Bitmap> CapturedImagesBuffer 
        { 
            get
            {
                return captureBuffer;
            }
        }

        public CaptureBuffer()
        {
            fps = 20;
            captureBuffer = new List<Bitmap>();
            dynamicBuffer = new List<Bitmap>();
            modifyingBuffer = false;
        }

        public void updateFPS(float fps)
        {
            CaptureBuffer.fps = (int)Math.Round(fps);
        }

        public void AddCaptureBufferFrame(Bitmap bmp)
        {
            captureBuffer.Add(bmp);
        }

        public void AddDynamicBufferFrame(Bitmap bmp)
        {
            if (!modifyingBuffer)
            {
                dynamicBuffer.Add(bmp);
                resetBuffer();
            }
        }

        private void resetBuffer()
        {
            modifyingBuffer = true;

            if (dynamicBuffer.Count > (fps * 3))
            {
                dynamicBuffer.RemoveAt(0);
            }

            modifyingBuffer = false;
        }

        public void Open(int width, int height, float fps, bool automaticDetection)
        {
            captureBuffer = new List<Bitmap>();

            if (automaticDetection)
            {
                captureBuffer = dynamicBuffer;
            }

            CaptureBuffer.width = width;
            CaptureBuffer.height = height;

            if (fps > 17)
            {
                CaptureBuffer.fps = (int)Math.Round(fps) - 2;
            }
            else
            {
                CaptureBuffer.fps = (int)Math.Round(fps);
            }
        }

        public void Close()
        {
            try
            {
               /* // Get the path for Desktop, to easily find the CSV
                String path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                String dateStamp = DateTime.Now.ToString("yyyy_MM_dd_HH-mm-ss");

                // Create the filepath, add date stamp to filename
                String filePath = Path.Combine(path, "LC_Video_" + dateStamp + ".avi");
                writer.Open(filePath, width, height, fps, VideoCodec.MPEG4, 2000000);

                int count = 0;
                while (captureBuffer[count] != null)
                {
                    writer.WriteVideoFrame(captureBuffer[count]);
                    count++;
                }

                writer.Close();
                writer = null;*/
                captureBuffer = null;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
    }
}
