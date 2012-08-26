using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace StreetFoo.Client
{
    public static class ImageHelper
    {
        internal static async Task ResizeAndSaveAs(IStorageFile source, IStorageFile destination, 
            int targetDimension)
        {
            // open the file...
            using(var sourceStream = await source.OpenReadAsync())
            {
                // step one, get a decoder...
                var decoder = await BitmapDecoder.CreateAsync(sourceStream);

                // step two, create somewhere to put it...
                using(var tempStream = new InMemoryRandomAccessStream())
                {
                    // copy...
                    await RandomAccessStream.CopyAsync(sourceStream, tempStream);

                    // step three, create an encoder...
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(tempStream, decoder);

                    // how big is it?
                    uint width = decoder.PixelWidth;
                    uint height = decoder.PixelHeight;
                    decimal ratio = (decimal)width / (decimal)height;

                    // orientation?
                    bool portrait = width < height;

                    // create the new size...
                    if (portrait)
                    {
                        encoder.BitmapTransform.ScaledHeight = (uint)targetDimension;
                        encoder.BitmapTransform.ScaledWidth = (uint)((decimal)targetDimension * ratio);
                    }
                    else
                    {
                        encoder.BitmapTransform.ScaledWidth = (uint)targetDimension;
                        encoder.BitmapTransform.ScaledHeight = (uint)((decimal)targetDimension / ratio);
                    }

                    // write it...
                    await encoder.FlushAsync();

                    // copy it...
                    tempStream.Seek(0);
                    using (var destinationWriter = await destination.OpenTransactedWriteAsync())
                    {
                        // reload the image from the stream and copy over the data...
                        decoder = await BitmapDecoder.CreateAsync(tempStream);
                        encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, destinationWriter.Stream);

                        // set...
                        var pixels = await decoder.GetPixelDataAsync();
                        var bs = pixels.DetachPixelData();
                        encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode,
                            decoder.PixelWidth, decoder.PixelHeight, decoder.DpiX, decoder.DpiY, bs);

                        await encoder.FlushAsync();
                    }
                }
            }
        }
    }
}