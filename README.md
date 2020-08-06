# JpegLibrary

JPEG decoder, encoder and optimizer implemented in C#.

## Supported Runtimes

* .NET Framework 4.5+
* .NET Core 2.0+
* .NET Standard 2.0+

## Supported Features

### Decode
* Decode Huffman-coding baseline DCT-based JPEG (SOF0)
* Decode Huffman-coding extended sequential DCT-based JPEG (SOF1)
* Decode Huffman-coding progressive DCT-based JPEG (SOF2)
* Decode Huffman-coding lossless JPEG (SOF3)
* Decode arithmetic-coding sequential DCT-based JPEG (SOF9)
* Decode arithmetic-coding progressive DCT-based JPEG (SOF10)

See [JpegDecode](https://github.com/warrengalyen/JpegLibrary/blob/master/apps/JpegDecode/DecodeAction.cs) program for example.

### Encode
* Encode Huffman-coding baseline DCT-based JPEG (SOF0) with optimized coding.

See [JpegEncode](https://github.com/warrengalyen/JpegLibrary/blob/master/apps/JpegEncode/EncodeAction.cs) program for example.

### Optimize
* Optimize an existing SOF0 image to use optimized Huffman coding.

See [JpegOptimize](https://github.com/warrengalyen/JpegLibrary/blob/master/apps/JpegOptimize/OptimizeAction.cs) program for example.