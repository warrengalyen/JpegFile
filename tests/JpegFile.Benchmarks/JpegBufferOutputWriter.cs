﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JpegFile.Benchmarks
{
    internal sealed class JpegBufferOutputWriter : JpegBlockOutputWriter
    {
        private int _width;
        private int _height;
        private int _componentCount;
        private Memory<byte> _output;

        public JpegBufferOutputWriter(int width, int height, int componentsCount, Memory<byte> output)
        {
            if (output.Length < (width * height * componentsCount))
            {
                throw new ArgumentException("Destination buffer is too small.");
            }

            _width = width;
            _height = height;
            _componentCount = componentsCount;
            _output = output;
        }

        public override void WriteBlock(ref short blockRef, int componentIndex, int x, int y)
        {
            int componentCount = _componentCount;
            int width = _width;
            int height = _height;

            if (x > width || y > _height)
            {
                return;
            }

            int writeWidth = Math.Min(width - x, 8);
            int writeHeight = Math.Min(height - y, 8);

            ref byte destinationRef = ref MemoryMarshal.GetReference(_output.Span);
            destinationRef = ref Unsafe.Add(ref destinationRef, y * width * componentCount + x * componentCount + componentIndex);

            for (int destY = 0; destY < writeHeight; destY++)
            {
                ref byte destinationRowRef = ref Unsafe.Add(ref destinationRef, destY * width * componentCount);
                for (int destX = 0; destX < writeWidth; destX++)
                {
                    Unsafe.Add(ref destinationRowRef, destX * componentCount) = ClampTo8Bit(Unsafe.Add(ref blockRef, destX));
                }
                blockRef = ref Unsafe.Add(ref blockRef, 8);
            }
        }

        private static byte ClampTo8Bit(short input)
        {
#if NO_MATH_CLAMP
            return (byte)Math.Min(Math.Max(input, (short)0), (short)255);
#else
            return (byte)Math.Clamp(input, (short)0, (short)255);
#endif
        }
    }
}
