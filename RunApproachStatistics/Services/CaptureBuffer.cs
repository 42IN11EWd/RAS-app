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
            captureBuffer.Add((Bitmap)bmp.Clone());
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
                dynamicBuffer[0].Dispose();
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
            foreach (Bitmap bmp in captureBuffer)
            {
                bmp.Dispose();
            }

            captureBuffer = null;
        }
    }
}
