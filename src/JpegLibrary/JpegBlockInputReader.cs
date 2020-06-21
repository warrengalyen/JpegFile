﻿#nullable enable

namespace JpegLibrary
{
    public abstract class JpegBlockInputReader
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract void ReadBlock(ref short blockRef, int componentIndex, int x, int y);
    }
}
