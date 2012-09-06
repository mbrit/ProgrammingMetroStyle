using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace StreetFoo.Client
{
    internal class ReportImageCacheManager
    {
        private const string LocalCacheFolderName = "ReportImages";

        internal ReportImageCacheManager()
        {
        }

        internal void EnqueueImageDownload(ReportViewItem viewItem)
        {
            Debug.WriteLine(string.Format("Enqueuing download for '{0}'...", viewItem.NativeId));

            // create a new task...
            Task<Task<string>>.Factory.StartNew(async () =>
            {
                Debug.WriteLine(string.Format("Requesting image for '{0}'...", viewItem.NativeId));

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
                Debug.WriteLine(string.Format("Image load for '{0}' finished.", viewItem.NativeId));
                return url;

            }).ContinueWith(async (t) =>
            {
                // send it back...
                Debug.WriteLine(string.Format("Setting image for '{0}'...", viewItem.NativeId));
                viewItem.ImageUri = (await t).Result;

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

        internal async Task<string> GetLocalImageUriAsync(ReportViewItem viewItem)
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
                Debug.WriteLine(string.Format("Cache image for '{0}' was found locally...", viewItem.NativeId));
                return CalculateLocalImageUrl(viewItem);
            }
            else
            {
                Debug.WriteLine(string.Format("Cache image for '{0}' was not found locally...", viewItem.NativeId));
                return null;
            }
        }
    }
}
