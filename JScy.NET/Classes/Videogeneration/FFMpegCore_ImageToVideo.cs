using System;
using System.Collections.Generic;
using System.Drawing;
using FFMpegCore;
using FFMpegCore.Extensions.System.Drawing.Common;
using FFMpegCore.Pipes;

namespace JScy.NET.Classes.Videogeneration
{
    public class FFMpegCore_ImageToVideo(string outputPath, int width, int height, int framerate) : IDisposable
    {
        private readonly List<Image> Images = [];
        private readonly int width = width;
        private readonly int height = height;
        private readonly int framerate = framerate;
        private readonly string outputPath = outputPath;

        public void AddNextImage(Image img)
        {
            Images.Add(img);
        }

        private IEnumerable<BitmapVideoFrameWrapper> CreateFramesSD()
        {
            for (int i = 0; i < Images.Count; i++)
            {
                using Bitmap bmp = new(Images[i], new Size(width, height));

                using BitmapVideoFrameWrapper wrappedBitmap = new(bmp);
                yield return wrappedBitmap;
            }
        }

        public void Finalize_()
        {
            var frames = CreateFramesSD();
            RawVideoPipeSource source = new(frames) { FrameRate = framerate };

            bool success = FFMpegArguments
                .FromPipeInput(source)
                .OutputToFile(outputPath, overwrite: true, options => options.WithVideoCodec("libvpx-vp9"))
                .ProcessSynchronously();
        }

        public void Dispose()
        {
            Finalize_();
            GC.SuppressFinalize(this);
        }
    }
}