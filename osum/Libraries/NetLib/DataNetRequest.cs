using System;
using System.IO;
using System.Threading;
using System.Net;
using osum;
using System.Text;
#if iOS
using MonoTouch.Foundation;
using System.Runtime.InteropServices;
#endif

namespace osu_common.Libraries.NetLib
{
#if iOS
    public class NRDelegate : NSUrlConnectionDelegate
    {
        public byte[] result;
        public int written = 0;
        public bool finished;
        public Exception error;

        DataNetRequest nr;

        public NRDelegate(DataNetRequest nr) : base()
        {
            this.nr = nr;
        }

        public override void ReceivedResponse(NSUrlConnection connection, NSUrlResponse response)
        {
            long length = response.ExpectedContentLength;
            if (length != -1)
                result = new byte[length];
        }

        public override void ReceivedData(NSUrlConnection connection, NSData data)
        {
            if (written + data.Length > result.Length)
            {
                byte[] nb = new byte [result.Length + data.Length];
                result.CopyTo (nb, 0);
                Marshal.Copy (data.Bytes, nb, result.Length, (int) data.Length);
                result = nb;
            }
            else
                Marshal.Copy(data.Bytes, result, written, (int)data.Length);

            Console.WriteLine("received " + data.Length);

            written += (int)data.Length;

            if (nr.AbortRequested)
            {
                connection.Cancel();
                return;
            }

            nr.TriggerUpdate();
        }

        public override void FinishedLoading(NSUrlConnection connection)
        {
            finished = true;

            nr.TriggerUpdate();

            nr.data = result;
            nr.error = error;

            nr.processFinishedRequest();
        }

        public override void FailedWithError(NSUrlConnection connection, NSError err)
        {
            if (err != null)
            {
                error = new Exception(err.ToString());
                nr.error = error;
            }

            finished = true;

            nr.processFinishedRequest();
        }
    }
#endif

    /// <summary>
    /// Downloads a file from the internet to a specified location
    /// </summary>
    public class DataNetRequest : NetRequest
    {
        public DataNetRequest(string _url)
            : base(_url)
        {
        }

        public event RequestStartHandler onStart;
        public event RequestUpdateHandler onUpdate;
        public event RequestCompleteHandler onFinish;

        public byte[] data;
        public Exception error;

#if iOS
        NRDelegate del;

        public void TriggerUpdate()
        {
            if (onUpdate != null)
                onUpdate(this, del.written, del.result.Length);
        }
#endif

        public override void Perform()
        {
            try
            {
                //inform subscribers that we have started
                if (onStart != null)
                    onStart();

#if iOS
                del = new NRDelegate(this);

                NSUrlRequest req = new NSUrlRequest(new NSUrl(UrlEncode(m_url)), NSUrlRequestCachePolicy.ReloadIgnoringCacheData, 15);
                NSUrlConnection conn = new NSUrlConnection(req, del, true);

#if !DIST
                if (error != null)
                    Console.WriteLine("requst finished with error " + error);
#endif
#else
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadDataCompleted += wc_DownloadDataCompleted;
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadDataAsync(new Uri(m_url));

                    while (wc.IsBusy)
                        Thread.Sleep(500);
                }

                processFinishedRequest();
#endif
            }
            catch (ThreadAbortException)
            { }
        }

        private const string badChars = " \"%'\\";
        public static String UrlEncode(String s)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in s.ToCharArray())
            {
                ushort u = (ushort)c;
                if (u < 32 || badChars.IndexOf(c) >= 0)
                {
                    result.Append('%');
                    result.Append(u.ToString("X2"));
                }
                else result.Append(c);
            }

            return result.ToString();
        }

        public virtual void processFinishedRequest()
        {
            if (AbortRequested) return;

            GameBase.Scheduler.Add(delegate
            {
                if (onFinish != null)
                    onFinish(data, error);
            });
        }

        long totalBytesReceived = 0;

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            totalBytesReceived += e.BytesReceived;
            GameBase.Scheduler.Add(delegate
            {
                if (onUpdate != null)
                    onUpdate(this, totalBytesReceived, e.TotalBytesToReceive);
            });
        }

        void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            data = e.Result;
            error = e.Error;
        }

        public override bool Valid()
        {
            return true;
        }

        public override void OnException(Exception e)
        {
#if !DIST
            Console.WriteLine("net error:" + e);
#endif
            processFinishedRequest();
        }

        #region Nested type: RequestCompleteHandler

        public delegate void RequestCompleteHandler(byte[] data, Exception e);

        #endregion
    }
}