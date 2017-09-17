using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.SqlServer.Server;

namespace WarSQLKit
{
    class FileDownloader
    {
        private readonly string _url;
        private readonly string _fullPathWhereToSave;
        private bool _result = false;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
        public FileDownloader(string url, string fullPathWhereToSave)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
            if (string.IsNullOrEmpty(fullPathWhereToSave)) throw new ArgumentNullException("fullPathWhereToSave");

            _url = url;
            _fullPathWhereToSave = fullPathWhereToSave;
        }
        public bool StartDownload(int timeout)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_fullPathWhereToSave));

                if (File.Exists(_fullPathWhereToSave))
                {
                    File.Delete(_fullPathWhereToSave);
                }
                using (WebClient client = new WebClient())
                {
                    var ur = new Uri(_url);
                    client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                    client.DownloadFileCompleted += WebClientDownloadCompleted;
                    SqlContext.Pipe.Send(@"Downloading file:");
                    client.DownloadFileAsync(ur, _fullPathWhereToSave);
                    _semaphore.Wait(timeout);
                    return _result && File.Exists(_fullPathWhereToSave);
                }
            }
            catch (Exception e)
            {
                SqlContext.Pipe.Send("Was not able to download file!");
                SqlContext.Pipe.Send(e.Message);
                return false;
            }
            finally
            {
                _semaphore.Dispose();
            }
        }
        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            SqlContext.Pipe.Send("\r     -->    {0}%."+ e.ProgressPercentage);
        }
        private void WebClientDownloadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            SqlContext.Pipe.Send(Environment.NewLine + "Download finished!");
            _result = !args.Cancelled;
            if (!_result)
            {
                SqlContext.Pipe.Send(args.Error.ToString());
            }
            _semaphore.Release();
        }
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static bool DownloadFile(string url, string fullPathWhereToSave, int timeoutInMilliSec)
        {
            return new FileDownloader(url, fullPathWhereToSave).StartDownload(timeoutInMilliSec);
        }
    }
}
