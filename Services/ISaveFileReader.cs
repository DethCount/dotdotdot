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
        Models.Diff.SaveFile ReadDiff(
            string filepath2,
            string filepath1
        );
        Models.Diff.SaveFileHeader ReadHeaderDiff(
            string filepath2,
            string filepath1
        );
        Models.Diff.SaveFileObjects ReadObjectsDiff(
            string filepath2,
            string filepath1
        );
        Models.Diff.SaveFileProperties ReadPropertiesDiff(
            string filepath2,
            string filepath1
        );
        SaveFileHeader ReadNextSaveFileHeader(Stream src);
        void SkipHeader(Stream src);
        Models.Diff.SaveFileHeader DiffNextSaveFileHeader(Stream src2, Stream src1);
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
            bool skipPreviousBlocks = false
        );
        Models.Diff.SaveFileObjects DiffNextSaveFileObjects(
            Stream src2,
            Stream worldObjectSrc2,
            Stream src1,
            Stream worldObjectSrc1,
            out List<Int32> objectTypes2,
            out List<Int32> objectTypes1,
            bool skipPreviousBlocks = false
        );
        SaveFileProperties ReadNextSaveFileProperties(
            Stream src,
            Stream worldObjectSrc,
            List<Int32> objectTypes = null,
            bool skipPreviousBlocks = true
        );
        Models.Diff.SaveFileProperties DiffNextSaveFileProperties(
            Stream src2,
            Stream worldObjectSrc2,
            Stream src1,
            Stream worldObjectSrc1,
            List<Int32> objectTypes2 = null,
            List<Int32> objectTypes1 = null,
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
        Models.Diff.Property<Byte> DiffNextByte(Stream to, Stream from);
        void SkipNextInt32(Stream src);
        Int32 ReadNextInt32(Stream src);
        Models.Diff.Property<Int32> DiffNextInt32(Stream to, Stream from);
        void SkipNextDateTime(Stream src);
        DateTime ReadNextDateTime(Stream src);
        Models.Diff.Property<DateTime> DiffNextDateTime(Stream to, Stream from);
        void SkipNextInt64(Stream src);
        Int64 ReadNextInt64(Stream src);
        Models.Diff.Property<Int64> DiffNextInt64(Stream to, Stream from);
        void SkipNextFloat32(Stream src);
        Single ReadNextFloat32(Stream src);
        Models.Diff.Property<Single> DiffNextFloat32(Stream to, Stream from);
        Vector2D ReadNextVector2D(Stream src);
        Models.Diff.Property<Vector2D> DiffNextVector2D(Stream to, Stream from);
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
        Models.Diff.Property<string> DiffNextString(Stream to, Stream from);
        byte[] ReadNext(Stream src, int dataLen);
    }
}