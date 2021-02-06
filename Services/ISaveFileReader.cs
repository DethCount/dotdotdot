using System;
using System.Collections.Generic;
using System.IO;

using dotdotdot.Models;

namespace dotdotdot.Services
{
    public interface ISaveFileReader
    {
        string GetBasePath();
        Stream GetStreamFromFilepath(string filepath);
        SaveFile Read(string filepath);
        SaveFileHeader ReadHeader(string filepath);
        SaveFileObjects ReadObjects(string filepath);
        SaveFileHeader ReadNextSaveFileHeader(Stream src);
        void SkipHeader(Stream src);
        void SkipObjects(Stream src, Stream worldObjectSrc);
        void UnzipSaveFileObjects(Stream src, Stream dest);
        SaveFileObjects ReadNextSaveFileObjects(
            Stream src,
            Stream worldObjectSrc,
            bool skipPreviousBlocks = true
        );
        SaveFileProperties ReadNextSaveFileProperties(
            Stream src,
            bool skipPreviousBlocks = true
        );
        SaveFile ReadNextSaveFile(Stream src);
        SaveFileChunkHeader ReadNextSaveFileChunkHeader(Stream src);
        WorldObject ReadNextWorldObject(Stream src);
        WorldObjectProperties ReadNextWorldObjectProperties(Stream src);
        WorldObjectProperty ReadNextWorldObjectProperty(Stream src, long? count = null);
        object ReadNextWorldObjectPropertyValue(
            Stream src, 
            string type, 
            string subType = null, 
            bool firstValue = true,
            bool inArray = false,
            bool inMap = false
        );
        NamedWorldObjectProperty ReadNextNamedWorldObjectProperty(
            Stream src, 
            string name = null
        );
        WorldObjectStructProperty ReadNextWorldObjectDynamicStructProperty(
            Stream src, 
            string type = null
        );
        WorldObjectRef ReadNextWorldObjectRef(Stream src);
        WorldObjectObjectProperty ReadNextWorldObjectObjectProperty(Stream src);
        WorldObjectInterfaceProperty ReadNextWorldObjectInterfaceProperty(Stream src);
        Byte ReadNextByte(Stream src);
        Int32 ReadNextInt32(Stream src);
        Int64 ReadNextInt64(Stream src);
        Single ReadNextFloat32(Stream src);
        Vector2D ReadNextVector2D(Stream src);
        Rotator ReadNextRotator(Stream src);
        Vector ReadNextVector(Stream src);
        LinearColor ReadNextLinearColor(Stream src);
        Color ReadNextColor(Stream src);
        Box ReadNextBox(Stream src);
        Quat ReadNextQuat(Stream src);
        InventoryItem ReadNextInventoryItem(Stream src);
        RailroadTrackPosition ReadNextRailroadTrackPosition(Stream src);
        Guid ReadNextGuid(Stream src);
        FluidBox ReadNextFluidBox(Stream src);
        FINNetworkTrace ReadNextFINNetworkTrace(Stream src);
        WorldObjectArrayProperty ReadNextWorldObjectArrayProperty(Stream src, string type);
        object ReadNextWorldObjectStructProperty(Stream src, string type);
        Dictionary<object,object> ReadNextWorldObjectMapProperty(
            Stream src, 
            string keyType, 
            string valueType
        );
        WorldObjectTextProperty ReadNextWorldObjectTextProperty(Stream src);
        string ReadNextString(Stream src, int? length = null);
        byte[] ReadNext(Stream src, int dataLen);
    }
}