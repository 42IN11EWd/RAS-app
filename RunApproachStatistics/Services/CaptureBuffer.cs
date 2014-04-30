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
        private Bitmap[] captureBuffer;
        private Bitmap[] dynamicBuffer;
        private int captureBufferSize;
        private int dynamicBufferSize;
        private int framesCounter;
        private int dynamicBufferCounter;
        public static int fps { get; set; }
        public static int width { get; set; }
        public static int height { get; set; }
        private VideoFileWriter writer;
        private bool modifyingBuffer;

        public CaptureBuffer()
        {
            captureBufferSize = 10000;
            dynamicBufferSize = 1000;
            framesCounter = 0;
            dynamicBufferCounter = 0;
            fps = 20;
            captureBuffer = new Bitmap[captureBufferSize];
            dynamicBuffer = new Bitmap[dynamicBufferSize];
            modifyingBuffer = false;
        }

        public void updateFPS(float fps)
        {
            CaptureBuffer.fps = (int)Math.Round(fps);
        }

        public void AddCaptureBufferFrame(Bitmap bmp)
        {
            captureBuffer[framesCounter++] = bmp;
        }

        public void AddDynamicBufferFrame(Bitmap bmp)
        {
            if (!modifyingBuffer)
            {
                dynamicBuffer[dynamicBufferCounter++] = bmp;
                resetBuffer();
            }
        }

        private void resetBuffer()
        {
            modifyingBuffer = true;

            Bitmap[] tempBuffer = new Bitmap[dynamicBufferSize];

            if (dynamicBufferCounter > (fps * 3))
            {
                int counter = 0;

                while (dynamicBuffer[counter + 1] != null)
                {
                    tempBuffer[counter] = (Bitmap)dynamicBuffer[counter + 1].Clone();
                    counter++;
                }

                dynamicBuffer = tempBuffer;
                dynamicBufferCounter--;
            }

            modifyingBuffer = false;
        }

        public void Open(int width, int height, float fps, bool automaticDetection)
        {
            writer = new VideoFileWriter();
            captureBuffer = new Bitmap[captureBufferSize];

            if (automaticDetection)
            {
                Bitmap[] tempBuffer = dynamicBuffer;
                int counter = 0;

                while (tempBuffer[counter] != null)
                {
                    captureBuffer[counter] = (Bitmap)tempBuffer[counter].Clone();
                    counter++;
                }
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
                // Get the path for Desktop, to easily find the CSV
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
                writer = null;
                captureBuffer = null;
                framesCounter = 0;
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }
    }
}
