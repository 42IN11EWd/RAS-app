using dshow;
using dshow.Core;
using RunApproachStatistics.View;
using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using VideoSource;

namespace RunApproachStatistics.Services
{
    public class VideoCameraController
    {
        private FilterCollection filters;
        private string device;
        private View.CameraWindow cameraWindow = new View.CameraWindow();
        private CaptureBuffer captureBuffer = new CaptureBuffer();
        private bool save = false;
        private bool pausedCamera = false;
        private System.Timers.Timer timer;

        // fps
        private const int statLength = 15;
        private int statIndex = 0, statReady = 0;
        private int[] statCount = new int[statLength];
        private float fps;

        private String[] devices;
        public String[] Devices
        {
            get
            {
                return devices;
            }
        }

        public bool IsCapturing 
        { 
            get
            {
                return save;
            }
        }
        public List<Bitmap> RecordedVideo
        {
            get
            {
                if (captureBuffer != null)
                {
                    return captureBuffer.CapturedImagesBuffer;
                }
                else
                {
                    return null;
                }
            }
        }

        public CameraWindow CameraWindow
        {
            get
            {
                return cameraWindow;
            }
        }

        public VideoCameraController()
        {
            setDevices();

            timer = new System.Timers.Timer();
            timer.Interval = 1000D;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            timer.Start();
        }

        public void setDevices()
        {
            filters = new FilterCollection(FilterCategory.VideoInputDevice);

            if (filters.Count == 0)
            {
                MessageBox.Show("No camera's found", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            devices     = new String[filters.Count];
            int count   = 0;
            foreach (Filter filter in filters) 
            {
                devices[count++] = filter.Name;
            }
        }

        public CameraWindow OpenVideoSource(int deviceIndex)
        {
            try
            {
                this.device = filters[deviceIndex].MonikerString;
                CaptureDevice localSource = new CaptureDevice();
                localSource.VideoSource = device;

                //close previous file
                CloseFile();

                // create camera
                VideoCamera camera = new VideoCamera(localSource);

                // Start camera
                camera.Start();

                // attach camera to camera window
                cameraWindow.Camera = camera;

                // set event handlers
                camera.NewFrame += new EventHandler(camera_NewFrame);
            }
            catch (Exception e)
            {
                return null;
            }

            return cameraWindow;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            VideoCamera camera = cameraWindow.Camera;

            if (camera != null)
            {
                // get number of frames for the last second
                statCount[statIndex] = camera.FramesReceived;

                // increment indexes
                if (++statIndex >= statLength)
                {
                    statIndex = 0;
                }

                if (statReady < statLength)
                {
                    statReady++;
                }

                fps = 0;

                // calculate average value
                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }

                fps /= statReady;

                statCount[statIndex] = 0;

                captureBuffer.updateFPS(fps);
            }
        }

        private void CloseFile()
        {
            VideoCamera camera = cameraWindow.Camera;

            if (camera != null)
            {
                // detach camera from camera window
                cameraWindow.Camera = null;

                // signal camera to stop
                camera.SignalToStop();
                // wait for the camera
                camera.WaitForStop();

                camera = null;
            }
        }

        private void camera_NewFrame(object sender, System.EventArgs e)
        {            
            cameraWindow.Camera.Lock();

            if (save)
            {
                captureBuffer.AddCaptureBufferFrame((Bitmap)cameraWindow.Camera.LastFrame.Clone());
            }

            captureBuffer.AddDynamicBufferFrame((Bitmap)cameraWindow.Camera.LastFrame.Clone());

            cameraWindow.Camera.Unlock();            
        }

        public void Capture()
        {
            captureBuffer.Open(cameraWindow.Camera != null ? cameraWindow.Camera.Width : 640, cameraWindow.Camera != null ? cameraWindow.Camera.Height : 480, fps, false);
            save = true;
            Console.WriteLine("Camera started: " + DateTime.Now.ToString("dd_MM_yyyyTHH_mm_ss_fff"));
        }

        public void StopCapture()
        {
            save = false;
            Console.WriteLine("Camera stopped: " + DateTime.Now.ToString("dd_MM_yyyyTHH_mm_ss_fff"));
        }

        public void Close()
        {
            captureBuffer.Close();
        }

        public bool IsCameraOpen()
        {
            return cameraWindow.Camera != null;
        }
    }
}
