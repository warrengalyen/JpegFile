﻿using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace JpegFile.Tests.Decoder
{
    public class ArithmeticProgressiveDecodeTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            string currentDir = Directory.GetCurrentDirectory();
            yield return new object[] {
                Path.Join(currentDir, @"Assets\arithmetic_progressive\yellowcat_progressive_arith.jpg")
            };
            yield return new object[] {
                Path.Join(currentDir, @"Assets\arithmetic_progressive\yellowcat_progressive_arith_restart.jpg")
            };
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void TestDecode(string path)
        {
            byte[] jpegBytes = File.ReadAllBytes(path);

            var decoder = new JpegDecoder();
            decoder.SetInput(jpegBytes);
            decoder.Identify();

            ushort[] buffer = new ushort[decoder.Width * decoder.Height * 4];
            var outputWriter = new JpegExtendingOutputWriter(decoder.Width, decoder.Height, 4, decoder.Precision, buffer);

            decoder.SetOutputWriter(outputWriter);
            decoder.Decode();

            ushort[] reference = ImageHelper.LoadBuffer(path, decoder.Width, decoder.Height, decoder.NumberOfComponents);

            Assert.True(reference.AsSpan().SequenceEqual(buffer));
        }
    }
}
