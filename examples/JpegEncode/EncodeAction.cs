﻿using JpegFile;
using JpegFile.ColorConverters;
using JpegFile.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JpegEncode
{
    public class EncodeAction
    {
        public static Task<int> Encode(FileInfo source, FileInfo output, int quality, bool optimizeCoding)
        {
            if (quality <= 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(quality));
            }

            Image<Rgb24> image;
            using (FileStream stream = source.OpenRead())
            {
                image = Image.Load<Rgb24>(stream);
            }

            // Convert RGB to YCbCr
            byte[] ycbcr = new byte[image.Width * image.Height * 3];

            image.ProcessPixelRows(pixelAccessor =>
            {
                for (int i = 0; i < image.Height; i++)
                {
                    JpegRgbToYCbCrConverter.Shared.ConvertRgb24ToYCbCr8(MemoryMarshal.AsBytes(pixelAccessor.GetRowSpan(i)), ycbcr.AsSpan(3 * image.Width * i, 3 * image.Width), image.Width);
                }
            });

            var encoder = new JpegEncoder();
            encoder.SetQuantizationTable(JpegStandardQuantizationTable.ScaleByQuality(JpegStandardQuantizationTable.GetLuminanceTable(JpegElementPrecision.Precision8Bit, 0), quality));
            encoder.SetQuantizationTable(JpegStandardQuantizationTable.ScaleByQuality(JpegStandardQuantizationTable.GetChrominanceTable(JpegElementPrecision.Precision8Bit, 1), quality));
            if (optimizeCoding)
            {
                encoder.SetHuffmanTable(true, 0);
                encoder.SetHuffmanTable(false, 0);
                encoder.SetHuffmanTable(true, 1);
                encoder.SetHuffmanTable(false, 1);
            }
            else
            {
                encoder.SetHuffmanTable(true, 0, JpegStandardHuffmanEncodingTable.GetLuminanceDCTable());
                encoder.SetHuffmanTable(false, 0, JpegStandardHuffmanEncodingTable.GetLuminanceACTable());
                encoder.SetHuffmanTable(true, 1, JpegStandardHuffmanEncodingTable.GetChrominanceDCTable());
                encoder.SetHuffmanTable(false, 1, JpegStandardHuffmanEncodingTable.GetChrominanceACTable());
            }
            encoder.AddComponent(1, 0, 0, 0, 2, 2); // Y component
            encoder.AddComponent(2, 1, 1, 1, 1, 1); // Cb component
            encoder.AddComponent(3, 1, 1, 1, 1, 1); // Cr component

            encoder.SetInputReader(new JpegBufferInputReader(image.Width, image.Height, 3, ycbcr));

            var writer = new ArrayBufferWriter<byte>();
            encoder.SetOutput(writer);

            encoder.Encode();

            using (FileStream stream = output.OpenWrite())
            {
                stream.Write(writer.WrittenSpan);
            }

            return Task.FromResult(0);
        }
    }
}
