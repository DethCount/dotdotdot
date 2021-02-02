using System;
using System.IO;

using dotdotdot.Models;

namespace dotdotdot.Services
{
    public interface ISaveFileReader
    {
        string GetBasePath();
        SaveFile Read(string filepath);
        SaveFile ReadNextSaveFile(Stream src);
        SaveFileChunkHeader ReadNextSaveFileChunkHeader(Stream src);
        WorldObject ReadNextWorldObject(Stream src);
        Byte ReadNextByte(Stream src);
        Int32 ReadNextInt32(Stream src);
        Int64 ReadNextInt64(Stream src);
        Single ReadNextFloat32(Stream src);
        string ReadNextString(Stream src, int? length = null);
        byte[] ReadNext(Stream src, int dataLen);
    }
}