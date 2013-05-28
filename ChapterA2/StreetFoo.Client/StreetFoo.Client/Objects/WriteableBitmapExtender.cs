using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace StreetFoo.Client
{
    public static class WriteableBitmapExtender
    {
        public static Task SaveAsJpgAsync(this WriteableBitmap image, IStorageFile file)
        {
            return image.SaveAsAsync(BitmapEncoder.JpegEncoderId, file);
        }

        public static Task SaveAsPngAsync(this WriteableBitmap image, IStorageFile file)
        {
            return image.SaveAsAsync(BitmapEncoder.PngEncoderId, file);
        }

        public static async Task SaveAsAsync(this WriteableBitmap image, Guid format, IStorageFile file)
        {
            // bytes...
            var bytes = await image.GetPixelBytesAsync();

            // save...
            using (var stream = await file.OpenTransactedWriteAsync())
            {
                // encoder...
                var encoder = await BitmapEncoder.CreateAsync(format, stream.Stream);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Straight, (uint)image.PixelWidth, (uint)image.PixelHeight,
                    96, 96, bytes);
                await encoder.FlushAsync();
            }
        }

        public static async Task<byte[]> GetPixelBytesAsync(this WriteableBitmap image)
        {
            using (var stream = new MemoryStream())
            {
                // needs --> using System.Runtime.InteropServices.WindowsRuntime;
                await image.PixelBuffer.AsStream().CopyToAsync(stream);

                // return...
                return stream.ToArray();
            }
        }
    }
}
