﻿#nullable enable

namespace JpegFile
{
    internal static class JpegMarkerHelper
    {
        public static bool IsRestartMarker(this JpegMarker marker)
        {
            return JpegMarker.DefineRestart0 <= marker && marker <= JpegMarker.DefineRestart7;
        }
    }
}
