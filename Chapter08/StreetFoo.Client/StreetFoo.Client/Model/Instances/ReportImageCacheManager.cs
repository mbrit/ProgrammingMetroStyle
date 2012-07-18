using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroLog;
using Windows.Storage;
using Windows.Storage.Streams;

namespace StreetFoo.Client
{
    internal class ReportImageCacheManager : ILoggable
    {
        private const string LocalCacheFolderName = "ReportImages";

        internal ReportImageCacheManager()
        {
        }

        internal void EnqueueImageDownload(ReportViewItem viewItem)
        {
            this.Info("Enqueuing download for '{0}'...", viewItem.NativeId);

            // create a new task...
            Task<Task<string>>.Factory.StartNew(async () =>
            {
                this.Info("Requesting image for '{0}'...", viewItem.NativeId);

                // load...
                var proxy = ServiceProxyFactory.Current.GetHandler<IGetReportImageServiceProxy>();
                var result = await proxy.GetReportImageAsync(viewItem.NativeId);

                // check...
                result.AssertNoErrors();

                // create the new file...
                var filename = GetCacheFilename(viewItem);
                var cacheFolder = await this.GetCacheFolderAsync();
                var cacheFile = await cacheFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (var stream = await cacheFile.OpenStreamForWriteAsync())
                    stream.Write(result.ImageBytes, 0, result.ImageBytes.Length);

                // get the URL...
                string url = this.CalculateLocalImageUrl(viewItem);
                this.Info("Image load for '{0}' finished.", viewItem.NativeId);
                return url;

            }).ContinueWith(async (t) =>
            {
                // send it back...
                this.Info("Setting image for '{0}'...", viewItem.NativeId);
                viewItem.ImageUrl = (await t).Result;

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        internal async Task<StorageFolder> GetCacheFolderAsync()
        {
            // find...
            StorageFolder cacheFolder = null;
            try
            {
                cacheFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(LocalCacheFolderName);
            }
            catch (FileNotFoundException ex)
            {
                SinkWarning(ex);
            }

            // did we get one?
            if(cacheFolder == null)
                cacheFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(LocalCacheFolderName);

            // return...
            return cacheFolder;
        }

        private void SinkWarning(FileNotFoundException ex)
        {
            // no-op - we're just getting rid of compiler warnings...
        }

        private string GetCacheFilename(ReportViewItem viewItem)
        {
            return viewItem.NativeId + ".jpg";
        }

        private string CalculateLocalImageUrl(ReportViewItem viewItem)
        {
            return string.Format("ms-appdata:///local/{0}/{1}.jpg", LocalCacheFolderName, viewItem.NativeId);
        }

        internal async Task<string> GetLocalImageUrlAsync(ReportViewItem viewItem)
        {
            var cacheFolder = await this.GetCacheFolderAsync();

            // build a path based on the native id...
            var filename = GetCacheFilename(viewItem);
            StorageFile cacheFile = null;
            try
            {
                cacheFile = await cacheFolder.GetFileAsync(filename);
            }
            catch (FileNotFoundException ex)
            {
                SinkWarning(ex);
            }

            // did we get one?
            if (cacheFile != null)
            {
                this.Info("Cache image for '{0}' was found locally...", viewItem.NativeId);
                return CalculateLocalImageUrl(viewItem);
            }
            else
            {
                this.Info("Cache image for '{0}' was not found locally...", viewItem.NativeId);
                return null;
            }
        }
    }
}
