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
        SaveFileProperties ReadProperties(string filepath);
        SaveFileHeader ReadNextSaveFileHeader(Stream src);
        void SkipHeader(Stream src);
        void SkipObjects(
            Stream src,
            Stream worldObjectSrc,
            out List<Int32> objectTypes
        );
        void UnzipSaveFileObjects(Stream src, Stream dest);
        SaveFileObjects ReadNextSaveFileObjects(
            Stream src,
            Stream worldObjectSrc,
            out List<Int32> objectTypes,
            bool skipPreviousBlocks = true
        );
        SaveFileProperties ReadNextSaveFileProperties(
            Stream src,
            Stream worldObjectSrc,
            List<Int32> objectTypes = null,
            bool skipPreviousBlocks = true
        );
        SaveFile ReadNextSaveFile(Stream src);
        SaveFileChunkHeader ReadNextSaveFileChunkHeader(Stream src);
        void SkipNextWorldObject(Stream src, out Int32 objectType);
        WorldObject ReadNextWorldObject(Stream src);
        void SkipNextWorldObjectData(Stream src, Int32 objectType);
        WorldObjectData ReadNextWorldObjectData(Stream src, Int32 objectType);
        WorldObjectProperties ReadNextWorldObjectProperties(Stream src, Int32 objectType);
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
        void SkipNextWorldObjectRef(Stream src);
        WorldObjectRef ReadNextWorldObjectRef(Stream src);
        WorldObjectObjectProperty ReadNextWorldObjectObjectProperty(Stream src);
        WorldObjectInterfaceProperty ReadNextWorldObjectInterfaceProperty(Stream src);
        void SkipNextByte(Stream src);
        Byte ReadNextByte(Stream src);
        void SkipNextInt32(Stream src);
        Int32 ReadNextInt32(Stream src);
        void SkipNextInt64(Stream src);
        Int64 ReadNextInt64(Stream src);
        void SkipNextFloat32(Stream src);
        Single ReadNextFloat32(Stream src);
        Vector2D ReadNextVector2D(Stream src);
        Rotator ReadNextRotator(Stream src);
        void SkipNextVector(Stream src);
        Vector ReadNextVector(Stream src);
        LinearColor ReadNextLinearColor(Stream src);
        Color ReadNextColor(Stream src);
        Box ReadNextBox(Stream src);
        void SkipNextQuat(Stream src);
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
        void SkipNextString(Stream src);
        string ReadNextString(Stream src, int? length = null);
        byte[] ReadNext(Stream src, int dataLen);
    }
}