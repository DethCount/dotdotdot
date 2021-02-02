using System;

namespace dotdotdot.Models
{
    public class SaveFileChunkHeader
    {
        public Int64 packageFileTag;
        public Int64 maxChunkSize;
        public Int64 currentChunkCompressedLength;
        public Int64 currentChunkUncompressedLength;
        public Int64 currentChunkCompressedLength2;
        public Int64 currentChunkUncompressedLength2;
    }
}