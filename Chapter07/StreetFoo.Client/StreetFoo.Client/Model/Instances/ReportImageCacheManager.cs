using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace StreetFoo.Client
{
    internal class ReportImageCacheManager
    {
        private SynchronizationContext SyncContext { get; set; }

        private const string LocalCacheFolderName = "ReportImages";

        internal ReportImageCacheManager()
        {
            this.SyncContext = SynchronizationContext.Current;
        }

        internal void EnqueueImageDownload(ReportViewItem viewItem)
        {
            Debug.WriteLine(string.Format("Enqueuing download for '{0}'...", viewItem.NativeId));

            // queue...
            var nativeId = viewItem.NativeId;
            Task.Factory.StartNew(async () =>
            {
                Debug.WriteLine(string.Format("Requesting image for '{0}'...", viewItem.NativeId));

                // load...
                var proxy = ServiceProxyFactory.Current.GetHandler<IGetReportImageServiceProxy>();
                var result = await proxy.GetReportImageAsync(nativeId);

                // check...
                result.AssertNoErrors();

                // write the image to disk...
                var cacheFolder = await this.GetCacheFolderAsync();

                // delete any existing file...
                var filename = GetCacheFilename(viewItem);
                var existing = (await cacheFolder.GetFilesAsync()).Where(v => string.Compare(v.Name, filename, StringComparison.CurrentCultureIgnoreCase)== 0).FirstOrDefault();
                if (existing != null)
                    await existing.DeleteAsync();

                // create...
                var cacheFile = await cacheFolder.CreateFileAsync(filename);
                using (var stream = await cacheFile.OpenStreamForWriteAsync())
                    stream.Write(result.ImageBytes, 0, result.ImageBytes.Length);

                // set...
                string url = this.CalculateLocalImageUrl(viewItem);

                // do we have a UI?
                if (this.SyncContext != null)
                    this.SyncContext.Post((args) => viewItem.SetLocalImageUrl(url), null);
                else
                    viewItem.SetLocalImageUrl(url);

                // ok...
                Debug.WriteLine(string.Format("Image load for '{0}' finished.", viewItem.NativeId));

            });
        }

        private async Task<StorageFolder> GetCacheFolderAsync()
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
                Debug.WriteLine(string.Format("Cache image for '{0}' was found locally...", viewItem.NativeId));
                return CalculateLocalImageUrl(viewItem);
            }
            else
            {
                Debug.WriteLine(string.Format("Cache image for '{0}' was not found locally...", viewItem.NativeId));
                return null;
            }
        }

        private string CalculateLocalImageUrl(ReportViewItem viewItem)
        {
            return string.Format("ms-appdata:///local/{0}/{1}.jpg", LocalCacheFolderName, viewItem.NativeId);
        }

        private string GetCacheFilename(ReportViewItem viewItem)
        {
            return viewItem.NativeId + ".jpg";
        }

        private void SinkWarning(FileNotFoundException ex)
        {
            // no-op - we're just getting rid of compiler warnings...
        }
    }
}
