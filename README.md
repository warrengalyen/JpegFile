# JpegLibrary

A native C# implementation of JPEG decoder and encoder.

## Supported Runtimes

* .NET Framework 4.5+
* .NET Core 2.0+
* .NET Standard 2.0+

## Currently Supported Features

### Decode
* Decode Huffman-coding baseline DCT-based JPEG (SOF0)
* Decode Huffman-coding extended sequential DCT-based JPEG (SOF1)
* Decode Huffman-coding progressive DCT-based JPEG (SOF2)
* Decode Huffman-coding lossless JPEG (SOF3)
* Decode arithmetic-coding sequential DCT-based JPEG (SOF9)
* Decode arithmetic-coding progressive DCT-based JPEG (SOF10)

### Encode
* Encode Huffman-coding baseline DCT-based JPEG (SOF0) with optimized coding.

### Optimize
* Optimize an existing SOF0 image to use optimized Huffman coding.