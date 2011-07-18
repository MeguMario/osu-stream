using System;
using System.IO;

namespace osu_common.Libraries.NetLib
{
    /// <summary>
    /// Downloads a file from the internet to a specified location
    /// </summary>
    public class FileNetRequest : DataNetRequest
    {
        private string path;

        public FileNetRequest(string path, string url) : base(url)
        {
            this.path = path;
        }

        public override void processFinishedRequest()
        {
            if (data != null && error == null)
                File.WriteAllBytes(path, data);
            base.processFinishedRequest();
        }

    }
}