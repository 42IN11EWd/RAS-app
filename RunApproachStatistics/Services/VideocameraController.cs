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
            try {
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
                    statIndex = 0;
                if (statReady < statLength)
                    statReady++;

                fps = 0;

                // calculate average value
                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }

                fps /= statReady;

                statCount[statIndex] = 0;

                captureBuffer.updateFPS(fps);

                //mainWindow.setFps(fps);
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
            captureBuffer.Open(cameraWindow.Camera.Width, cameraWindow.Camera.Height, fps, false);
            save = true;
            Console.WriteLine("Camera started: " + DateTime.Now.ToString("dd_MM_yyyyTHH_mm_ss_fff"));
        }

        public void StopCapture()
        {
            save = false;
            Console.WriteLine("Camera stopped: " + DateTime.Now.ToString("dd_MM_yyyyTHH_mm_ss_fff"));
        }

        public void WriteFile()
        {
            captureBuffer.Close();
        }

        public bool IsCameraOpen()
        {
            return cameraWindow.Camera != null;
        }
    }    

    internal class DispatcherWinFormsCompatAdapter : ISynchronizeInvoke
    {
        #region IAsyncResult implementation
        private class DispatcherAsyncResultAdapter : IAsyncResult
        {
            private DispatcherOperation m_op;
            private object m_state;

            public DispatcherAsyncResultAdapter(DispatcherOperation operation)
            {
                m_op = operation;
            }

            public DispatcherAsyncResultAdapter(DispatcherOperation operation, object state)
                : this(operation)
            {
                m_state = state;
            }

            public DispatcherOperation Operation
            {
                get { return m_op; }
            }

            #region IAsyncResult Members

            public object AsyncState
            {
                get { return m_state; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return null; }
            }

            public bool CompletedSynchronously
            {
                get { return false; }
            }

            public bool IsCompleted
            {
                get { return m_op.Status == DispatcherOperationStatus.Completed; }
            }

            #endregion
        }
        #endregion
        private Dispatcher m_disp;
        public DispatcherWinFormsCompatAdapter(Dispatcher dispatcher)
        {
            m_disp = dispatcher;
        }
        #region ISynchronizeInvoke Members

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            if (args != null && args.Length > 1)
            {
                object[] argsSansFirst = GetArgsAfterFirst(args);
                DispatcherOperation op = m_disp.BeginInvoke(DispatcherPriority.Normal, method, args[0], argsSansFirst);
                return new DispatcherAsyncResultAdapter(op);
            }
            else
            {
                if (args != null)
                {
                    return new DispatcherAsyncResultAdapter(m_disp.BeginInvoke(DispatcherPriority.Normal, method, args[0]));
                }
                else
                {
                    return new DispatcherAsyncResultAdapter(m_disp.BeginInvoke(DispatcherPriority.Normal, method));
                }
            }
        }

        private static object[] GetArgsAfterFirst(object[] args)
        {
            object[] result = new object[args.Length - 1];
            Array.Copy(args, 1, result, 0, args.Length - 1);
            return result;
        }

        public object EndInvoke(IAsyncResult result)
        {
            DispatcherAsyncResultAdapter res = result as DispatcherAsyncResultAdapter;
            if (res == null)
                throw new InvalidCastException();

            while (res.Operation.Status != DispatcherOperationStatus.Completed || res.Operation.Status == DispatcherOperationStatus.Aborted)
            {
                Thread.Sleep(50);
            }

            return res.Operation.Result;
        }

        public object Invoke(Delegate method, object[] args)
        {
            if (args != null && args.Length > 1)
            {
                object[] argsSansFirst = GetArgsAfterFirst(args);
                return m_disp.Invoke(DispatcherPriority.Normal, method, args[0], argsSansFirst);
            }
            else
            {
                if (args != null)
                {
                    return m_disp.Invoke(DispatcherPriority.Normal, method, args[0]);
                }
                else
                {
                    return m_disp.Invoke(DispatcherPriority.Normal, method);
                }
            }
        }

        public bool InvokeRequired
        {
            get { return m_disp.Thread != Thread.CurrentThread; }
        }

        #endregion
    }
}
