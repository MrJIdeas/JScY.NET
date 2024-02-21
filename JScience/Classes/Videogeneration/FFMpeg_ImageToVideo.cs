using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JScience.Classes.Videogeneration
{
    public class FFMpeg_ImageToVideo : IDisposable
    {
        private MediaOutput _mediaBuilder;

        public FFMpeg_ImageToVideo(string FFmpegPath, string OutPutPath, int width, int height, int framerate)
        {
            FFmpegLoader.FFmpegPath = FFmpegPath;

            var settings = new VideoEncoderSettings(width: width, height: height, framerate: framerate, codec: VideoCodec.H264);
            settings.EncoderPreset = EncoderPreset.Fast;
            settings.Framerate = 1;
            settings.CRF = 17;
            _mediaBuilder = MediaBuilder.CreateContainer(OutPutPath).WithVideo(settings).Create();
            Process.GetCurrentProcess().Exited += (s, e) => _mediaBuilder.Dispose();
        }

        public void AddNextImage(Image img)
        {
            using var ms = new MemoryStream();
            img.Save(ms, img.RawFormat);

            Bitmap pic = (Bitmap)Bitmap.FromStream(ms);
            var rect = new Rectangle(Point.Empty, pic.Size);
            var bitLock = pic.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, pic.Size);
            _mediaBuilder.Video.AddFrame(bitmapData); // Encode the frame
            pic.UnlockBits(bitLock);
        }

        public void Dispose()
        {
            _mediaBuilder.Dispose();
        }
    }
}