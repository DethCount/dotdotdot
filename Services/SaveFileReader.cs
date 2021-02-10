using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using dotdotdot.Models;

namespace dotdotdot.Services
{
    public class SaveFileReader : ISaveFileReader
    {
        private readonly ILogger<SaveFileReader> _logger;

        public SaveFileReader(ILogger<SaveFileReader> logger) {
            _logger = logger;
        }

        public string GetBasePath()
        {
            return Path.GetFullPath(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "\\FactoryGame\\Saved\\SaveGames\\"
            );
        }

        public Stream GetStreamFromFilepath(string filepath)
        {
            filepath = Path.GetFullPath(filepath);
            if (!filepath.StartsWith(GetBasePath())) {
                throw new IOException("File not found");
            }

            return new FileStream(
                filepath,
                FileMode.Open,
                FileAccess.Read
            );
        }

        public SaveFile Read(string filepath)
        {
            return ReadNextSaveFile(
                GetStreamFromFilepath(filepath)
            );
        }

        public SaveFileHeader ReadHeader(string filepath)
        {
            return ReadNextSaveFileHeader(
                GetStreamFromFilepath(filepath)
            );
        }

        public SaveFileObjects ReadObjects(string filepath)
        {
            MemoryStream worldObjectsSrc = new MemoryStream();
            List<Int32> objectTypes;

            return ReadNextSaveFileObjects(
                GetStreamFromFilepath(filepath),
                worldObjectsSrc,
                out objectTypes,
                true
            );
        }

        public SaveFileProperties ReadProperties(string filepath)
        {
            MemoryStream worldObjectsSrc = new MemoryStream();

            return ReadNextSaveFileProperties(
                GetStreamFromFilepath(filepath),
                worldObjectsSrc,
                null,
                true
            );
        }

        public Models.Diff.SaveFile ReadDiff(string filepath2, string filepath1)
        {
            Models.Diff.SaveFile diff = new Models.Diff.SaveFile();

            Stream src2 = GetStreamFromFilepath(filepath2);
            Stream src1 = GetStreamFromFilepath(filepath1);

            MemoryStream worldObjectSrc2 = new MemoryStream();
            MemoryStream worldObjectSrc1 = new MemoryStream();
            List<int> objectTypes2;
            List<int> objectTypes1;

            diff.header = DiffNextSaveFileHeader(src2, src1);

            diff.objects = DiffNextSaveFileObjects(
                src2,
                worldObjectSrc2,
                src1,
                worldObjectSrc1,
                out objectTypes2,
                out objectTypes1,
                false
            );

            diff.objects.objects.RemoveAll(item => item.status == Models.Diff.Status.UNCHANGED);

            return diff;
        }

        public SaveFileHeader ReadNextSaveFileHeader(Stream src)
        {
            SaveFileHeader header = new SaveFileHeader();

            header.saveHeaderVersion = ReadNextInt32(src);
            header.saveVersion = ReadNextInt32(src);
            header.buildVersion = ReadNextInt32(src);
            header.worldType = ReadNextString(src);
            header.worldProperties = ReadNextString(src);
            header.sessionName = ReadNextString(src);
            header.playTime = ReadNextInt32(src);
            header.saveDate = ReadNextDateTime(src);
            header.sessionVisibility = ReadNextByte(src);

            return header;
        }

        public void SkipHeader(Stream src)
        {
            SkipNextInt32(src);
            SkipNextInt32(src);
            SkipNextInt32(src);
            SkipNextString(src);
            SkipNextString(src);
            SkipNextString(src);
            SkipNextInt32(src);
            SkipNextDateTime(src);
            SkipNextByte(src);
        }

        public Models.Diff.SaveFileHeader DiffNextSaveFileHeader(Stream src2, Stream src1)
        {
            Models.Diff.SaveFileHeader header = new Models.Diff.SaveFileHeader();

            header.saveHeaderVersion = DiffNextInt32(src2, src1);
            header.saveVersion = DiffNextInt32(src2, src1);
            header.buildVersion = DiffNextInt32(src2, src1);
            header.worldType = DiffNextString(src2, src1);
            header.worldProperties = DiffNextString(src2, src1);
            header.sessionName = DiffNextString(src2, src1);
            header.playTime = DiffNextInt32(src2, src1);
            header.saveDate = DiffNextDateTime(src2, src1);
            header.sessionVisibility = DiffNextByte(src2, src1);

            return header;
        }

        public void SkipObjects(
            Stream src,
            Stream worldObjectSrc,
            out List<Int32> objectTypes
        ) {
            objectTypes = new List<Int32>();

            UnzipSaveFileObjects(src, worldObjectSrc);
            src.Dispose();
            worldObjectSrc.Position = 0;

            SkipNextInt32(worldObjectSrc);
            Int32 count = ReadNextInt32(worldObjectSrc);
            for (int i = 0; i < count; i++) {
                Int32 objectType;
                SkipNextWorldObject(worldObjectSrc, out objectType);
                objectTypes.Add(objectType);
            }
        }

        public void UnzipSaveFileObjects(Stream src, Stream dest)
        {
            while (src.Position < src.Length)
            {
                SaveFileChunkHeader header = ReadNextSaveFileChunkHeader(src);
                long start = src.Position;
                unzip(src, dest);
                src.Position = start + header.currentChunkCompressedLength;
            }
        }

        public SaveFileObjects ReadNextSaveFileObjects(
            Stream src,
            Stream worldObjectSrc,
            out List<Int32> objectTypes,
            bool skipPreviousBlocks = false
        ) {
            SaveFileObjects objects = new SaveFileObjects();
            objectTypes = new List<Int32>();

            if (skipPreviousBlocks) {
                SkipHeader(src);
            }

            UnzipSaveFileObjects(src, worldObjectSrc);

            src.Dispose();
            worldObjectSrc.Position = 0;

            objects.size = ReadNextInt32(worldObjectSrc);
            objects.count = ReadNextInt32(worldObjectSrc);
            objects.objects = new List<WorldObject>();

            for (int i = 0; i < objects.count; i++) {
                WorldObject obj = ReadNextWorldObject(worldObjectSrc);
                objectTypes.Add(obj.type);
                objects.objects.Add(obj);
            }

            return objects;
        }

        public Models.Diff.SaveFileObjects DiffNextSaveFileObjects(
            Stream src2,
            Stream worldObjectSrc2,
            Stream src1,
            Stream worldObjectSrc1,
            out List<Int32> objectTypes2,
            out List<Int32> objectTypes1,
            bool skipPreviousBlocks = false
        ) {
            Models.Diff.SaveFileObjects objects = new Models.Diff.SaveFileObjects();
            objectTypes2 = new List<Int32>();
            objectTypes1 = new List<Int32>();

            if (skipPreviousBlocks) {
                SkipHeader(src2);
                SkipHeader(src1);
            }

            UnzipSaveFileObjects(src2, worldObjectSrc2);
            UnzipSaveFileObjects(src1, worldObjectSrc1);

            src2.Dispose();
            src1.Dispose();
            worldObjectSrc2.Position = 0;
            worldObjectSrc1.Position = 0;

            objects.size = DiffNextInt32(worldObjectSrc2, worldObjectSrc1);
            objects.count = DiffNextInt32(worldObjectSrc2, worldObjectSrc1);

            Dictionary<Int32,WorldObject> objects1 = new Dictionary<int, WorldObject>();

            objects.objects = new Models.Diff.DiffList<Models.Diff.WorldObject, WorldObject>();
            for (int i = 0; i < objects.count.to; i++) {
                WorldObject obj2 = ReadNextWorldObject(worldObjectSrc2);
                objectTypes2.Add(obj2.type);

                WorldObject obj1;
                int j = 0;
                bool found = false;

                while (j < objects.count.from && objects1.ContainsKey(j)) {
                    obj1 = objects1.GetValueOrDefault(j);
                    if (obj2.id == obj1.id) {
                        objects.objects.Add(new Models.Diff.WorldObject(obj2, obj1));
                        objects1.Remove(j);
                        found = true;
                        break;
                    }

                    j++;
                }

                if (found) {
                    continue;
                }

                for (; j < objects.count.to; j++) {
                    obj1 = ReadNextWorldObject(worldObjectSrc1);
                    objectTypes2.Add(obj1.type);

                    if (obj2.id.Equals(obj1.id)) {
                        objects.AddObject(new Models.Diff.WorldObject(obj2, obj1));
                        found = true;
                        break;
                    }

                    objects1.Add(j, obj1);
                }

                if (found) {
                    continue;
                }

                objects.AddObject(new Models.Diff.WorldObject(obj2, null));
            }

            foreach (KeyValuePair<Int32,WorldObject> entry1 in objects1) {
                objects.AddObject(new Models.Diff.WorldObject(null, entry1.Value));
            }

            return objects;
        }

        public SaveFileProperties ReadNextSaveFileProperties(
            Stream src,
            Stream worldObjectSrc,
            List<Int32> objectTypes = null,
            bool skipPreviousBlocks = true
        ) {
            SaveFileProperties properties = new SaveFileProperties();

            if (skipPreviousBlocks) {
                SkipHeader(src);
                objectTypes = new List<Int32>();
                SkipObjects(src, worldObjectSrc, out objectTypes);
            }

            properties.count = ReadNextInt32(worldObjectSrc);
            if (objectTypes.Count != properties.count) {
                throw new Exception("Incoherence between object list and properties list");
            }

            properties.properties = new List<WorldObjectProperties>();

            for (int i = 0; i < properties.count; i++) {
                try {
                    if (i == 0) {
                        long pos = worldObjectSrc.Position;
                        string debug = ReadNextString(worldObjectSrc);
                        worldObjectSrc.Position = pos;
                    }
                    properties.properties.Add(ReadNextWorldObjectProperties(worldObjectSrc, objectTypes[i]));
                } catch (Exception e) {
                    throw e;
                }
            }

            return properties;
        }

        public SaveFile ReadNextSaveFile(Stream src)
        {
            SaveFile f = new SaveFile();

            f.header = ReadNextSaveFileHeader(src);
            MemoryStream worldObjectSrc = new MemoryStream();
            List<Int32> objectTypes;
            f.objects = ReadNextSaveFileObjects(src, worldObjectSrc, out objectTypes, false);
            f.properties = ReadNextSaveFileProperties(src, worldObjectSrc, objectTypes, false);

            return f;
        }

        public SaveFileChunkHeader ReadNextSaveFileChunkHeader(Stream src)
        {
            SaveFileChunkHeader header = new SaveFileChunkHeader();

            header.packageFileTag = ReadNextInt64(src);
            header.maxChunkSize = ReadNextInt64(src);
            header.currentChunkCompressedLength = ReadNextInt64(src);
            header.currentChunkUncompressedLength = ReadNextInt64(src);
            header.currentChunkCompressedLength2 = ReadNextInt64(src);
            header.currentChunkUncompressedLength2 = ReadNextInt64(src);

            return header;
        }

        public void SkipNextWorldObject(Stream src, out Int32 objectType)
        {
            objectType = ReadNextInt32(src);
            SkipNextString(src);
            SkipNextWorldObjectRef(src);
            SkipNextWorldObjectData(src, objectType);
        }

        public WorldObject ReadNextWorldObject(Stream src)
        {
            WorldObject obj = new WorldObject();
            obj.type = ReadNextInt32(src);
            obj.typePath = ReadNextString(src);
            obj.id = ReadNextWorldObjectRef(src);
            obj.value = ReadNextWorldObjectData(src, obj.type);

            return obj;
        }

        public void SkipNextWorldObjectData(Stream src, Int32 objectType)
        {
            switch (objectType) {
                case 0:
                    SkipNextString(src);
                    break;
                case 1:
                    SkipNextInt32(src);
                    SkipNextQuat(src);
                    SkipNextVector(src);
                    SkipNextVector(src);
                    SkipNextInt32(src);
                    break;
                default:
                    break;
            }
        }

        public WorldObjectData ReadNextWorldObjectData(Stream src, Int32 objectType)
        {
            WorldObjectData objData = new WorldObjectData();

            switch (objectType) {
                case 0:
                    objData.type = "Component";
                    objData.parentEntityName = ReadNextString(src);
                    break;
                case 1:
                    objData.type = "Entity";
                    objData.needTransform = ReadNextInt32(src) == 1;
                    objData.rotation = ReadNextQuat(src);
                    objData.position = ReadNextVector(src);
                    objData.scale = ReadNextVector(src);
                    objData.wasPlacedInLevel = ReadNextInt32(src) == 1;
                    break;
                default:
                    break;
            }

            return objData;
        }

        public WorldObjectProperties ReadNextWorldObjectProperties(Stream src, Int32 objectType)
        {
            WorldObjectProperties properties = new WorldObjectProperties();
            properties.properties = new List<WorldObjectProperty>();
            properties.size = ReadNextInt32(src);
            Int64 start = src.Position;

            // entity
            if (objectType == 1) {
                properties.parentId = ReadNextWorldObjectRef(src);
                properties.childrenCount = ReadNextInt32(src);

                for (int i = 0; i < properties.childrenCount; i++) {
                    WorldObjectRef child = ReadNextWorldObjectRef(src);
                    if (child != null) {
                        if (properties.children == null) {
                            properties.children = new List<WorldObjectRef>();
                        }

                        properties.children.Add(child);
                    }
                }
            }

            long notReaded = start + properties.size - src.Position;
            while (notReaded > 0) {
                WorldObjectProperty prop = ReadNextWorldObjectProperty(src);
                notReaded = start + properties.size - src.Position;
                if (prop == null) {
                    break;
                }

                properties.properties.Add(prop);
            }

            if (notReaded > 0) {
                src.Position += notReaded; // skip unknown
            }

            return properties;
        }

        public WorldObjectProperty ReadNextWorldObjectProperty(Stream src, long? count = null)
        {
            WorldObjectProperty prop = new WorldObjectProperty();

            prop.name = ReadNextString(src);
            if (prop.name == "None") {
                return null;
            }

            prop.type = ReadNextString(src);
            prop.size = ReadNextInt32(src);
            prop.index = ReadNextInt32(src);

            if (!count.HasValue) {
                prop.value = ReadNextWorldObjectPropertyValue(src, prop.type);
            } else {
                prop.isMultiple = true;
                prop.values = new List<object>();

                string subType = prop.type == "StructProperty" || prop.type == "ByteProperty" ? ReadNextString(src) : null;

                for (long i = 0; i < count; i++) {
                    prop.values.Add(ReadNextWorldObjectPropertyValue(src, prop.type, subType, i == 0, true));
                }
            }

            return prop;
        }

        public object ReadNextWorldObjectPropertyValue(
            Stream src,
            string type,
            string subType = null,
            bool firstValue = true,
            bool inArray = false,
            bool inMap = false
        ) {
            object value;
            string firstSubType = subType;
            string secondSubType = null;

            if (firstValue) {
                if (subType == null
                    && (
                        type == "StructProperty"
                        || type == "ArrayProperty"
                        || type == "MapProperty"
                        || (!inArray && type == "ByteProperty")
                        || (!inMap && type == "EnumProperty")
                    )
                ) {
                    firstSubType = ReadNextString(src);
                }

                if (type == "MapProperty") {
                    secondSubType = ReadNextString(src);
                }

                if (!inMap && type == "StructProperty") {
                    Guid guid = ReadNextGuid(src); // skip guid
                }

                if (!inMap
                    && (
                        !inArray
                        || (
                            type != "InterfaceProperty"
                            && type != "ObjectProperty"
                            && type != "ByteProperty"
                            && type != "IntProperty"
                        )
                    )
                ) {
                    src.Position++; // unk
                }
            }

            switch (type) {
                case "FloatProperty":
                    value = ReadNextFloat32(src);
                    break;
                case "IntProperty":
                    value = ReadNextInt32(src);
                    break;
                case "Int8Property":
                    value = ReadNextByte(src);
                    break;
                case "Int64Property":
                    value = ReadNextInt64(src);
                    break;
                case "ByteProperty":
                    value = (firstSubType == "None" || firstSubType == null)
                        ? ReadNextByte(src)
                        : ReadNext(src, ReadNextInt32(src));
                    break;
                case "BoolProperty":
                    value = ReadNextByte(src) > 0;
                    break;
                case "StrProperty":
                case "NameProperty":
                case "EnumProperty":
                    value = ReadNextString(src);
                    break;
                case "TextProperty":
                    value = ReadNextWorldObjectTextProperty(src);
                    break;
                case "ArrayProperty":
                    value = ReadNextWorldObjectArrayProperty(src, firstSubType);
                    break;
                case "StructProperty":
                    value = ReadNextWorldObjectStructProperty(src, firstSubType);
                    break;
                case "MapProperty":
                    value = ReadNextWorldObjectMapProperty(src, firstSubType, secondSubType);
                    break;
                case "SetProperty":
                    throw new NotImplementedException(type);
                case "ObjectProperty":
                case "InterfaceProperty":
                    value = ReadNextWorldObjectRef(src);
                    break;
                default:
                    throw new NotImplementedException(type);
            }

            return value;
        }
        public NamedWorldObjectProperty ReadNextNamedWorldObjectProperty(
            Stream src,
            string name = null
        ) {
            NamedWorldObjectProperty prop = new NamedWorldObjectProperty();
            prop.name = name != null ? name :  ReadNextString(src);
            prop.value = ReadNextWorldObjectProperty(src);

            return prop;
        }

        public WorldObjectStructProperty ReadNextWorldObjectDynamicStructProperty(
            Stream src,
            string type = null
        ) {
            WorldObjectStructProperty structProp = new WorldObjectStructProperty();
            structProp.type = type;
            structProp.properties = new List<WorldObjectProperty>();

            while (true) {
                WorldObjectProperty prop = ReadNextWorldObjectProperty(src);
                if (prop == null) {
                    break;
                }

                structProp.properties.Add(prop);
            }

            return structProp;
        }

        public void SkipNextWorldObjectRef(Stream src)
        {
            SkipNextString(src);
            SkipNextString(src);
        }

        public WorldObjectRef ReadNextWorldObjectRef(Stream src)
        {
            WorldObjectRef objRef = new WorldObjectRef();
            objRef.rootObject = ReadNextString(src);
            objRef.instanceName = ReadNextString(src);

            if (objRef.rootObject == string.Empty && objRef.instanceName == string.Empty) {
                return null;
            }

            return objRef;
        }

        public WorldObjectObjectProperty ReadNextWorldObjectObjectProperty(Stream src)
        {
            WorldObjectObjectProperty objRef = new WorldObjectObjectProperty();

            objRef.rootObject = ReadNextString(src);
            objRef.instanceName = ReadNextString(src);

            if (objRef.rootObject == string.Empty && objRef.instanceName == string.Empty) {
                return null;
            }

            return objRef;
        }

        public WorldObjectInterfaceProperty ReadNextWorldObjectInterfaceProperty(Stream src)
        {
            WorldObjectInterfaceProperty objRef = new WorldObjectInterfaceProperty();

            objRef.rootObject = ReadNextString(src);
            objRef.instanceName = ReadNextString(src);

            if (objRef.rootObject == string.Empty && objRef.instanceName == string.Empty) {
                return null;
            }

            return objRef;
        }

        public void SkipNextByte(Stream src)
        {
            src.Position += 1;
        }

        public Byte ReadNextByte(Stream src)
        {
            byte[] bytes = ReadNext(src, 1);

            return (Byte) bytes[0];
        }
        public Models.Diff.Property<Byte> DiffNextByte(Stream to, Stream from)
        {
            Models.Diff.Property<Byte> val = new Models.Diff.Property<Byte>();
            val.to = ReadNextByte(to);
            val.from = ReadNextByte(from);

            return val;
        }

        public void SkipNextInt32(Stream src)
        {
            src.Position += 4;
        }

        public Int32 ReadNextInt32(Stream src)
        {
            return BitConverter.ToInt32(ReadNext(src, 4));
        }

        public Models.Diff.Property<Int32> DiffNextInt32(Stream to, Stream from)
        {
            Models.Diff.Property<Int32> val = new Models.Diff.Property<Int32>();
            val.to = ReadNextInt32(to);
            val.from = ReadNextInt32(from);

            return val;
        }
        public void SkipNextDateTime(Stream src)
        {
            SkipNextInt64(src);
        }
        public DateTime ReadNextDateTime(Stream src)
        {
            return new DateTime(ReadNextInt64(src));
        }
        public Models.Diff.Property<DateTime> DiffNextDateTime(Stream to, Stream from)
        {
            Models.Diff.Property<DateTime> val = new Models.Diff.Property<DateTime>();
            val.to = ReadNextDateTime(to);
            val.from = ReadNextDateTime(from);

            return val;
        }

        public void SkipNextInt64(Stream src)
        {
            src.Position += 8;
        }

        public Int64 ReadNextInt64(Stream src)
        {
            return BitConverter.ToInt64(ReadNext(src, 8));
        }
        public Models.Diff.Property<Int64> DiffNextInt64(Stream to, Stream from)
        {
            Models.Diff.Property<Int64> val = new Models.Diff.Property<Int64>();
            val.to = ReadNextInt64(to);
            val.from = ReadNextInt64(from);

            return val;
        }

        public void SkipNextFloat32(Stream src)
        {
            src.Position += 4;
        }

        public Single ReadNextFloat32(Stream src)
        {
            return BitConverter.ToSingle(ReadNext(src, 4));
        }
        public Models.Diff.Property<Single> DiffNextFloat32(Stream to, Stream from)
        {
            Models.Diff.Property<Single> val = new Models.Diff.Property<Single>();
            val.to = ReadNextFloat32(to);
            val.from = ReadNextFloat32(from);

            return val;
        }

        public Vector2D ReadNextVector2D(Stream src)
        {
            Vector2D v = new Vector2D();
            v.x = ReadNextFloat32(src);
            v.y = ReadNextFloat32(src);

            return v;
        }

        public Models.Diff.Property<Vector2D> DiffNextVector2D(Stream to, Stream from)
        {
            Models.Diff.Property<Vector2D> val = new Models.Diff.Property<Vector2D>();
            val.to = ReadNextVector2D(to);
            val.from = ReadNextVector2D(from);

            return val;
        }

        public Rotator ReadNextRotator(Stream src)
        {
            Rotator r = new Rotator();
            r.x = ReadNextFloat32(src);
            r.y = ReadNextFloat32(src);
            r.z = ReadNextFloat32(src);

            return r;
        }

        public void SkipNextVector(Stream src)
        {
            SkipNextFloat32(src);
            SkipNextFloat32(src);
            SkipNextFloat32(src);
        }

        public Vector ReadNextVector(Stream src)
        {
            Vector r = new Vector();
            r.x = ReadNextFloat32(src);
            r.y = ReadNextFloat32(src);
            r.z = ReadNextFloat32(src);

            return r;
        }
        public LinearColor ReadNextLinearColor(Stream src)
        {
            LinearColor c = new LinearColor();
            c.r = ReadNextFloat32(src);
            c.g = ReadNextFloat32(src);
            c.b = ReadNextFloat32(src);
            c.a = ReadNextFloat32(src);

            return c;
        }

        public Color ReadNextColor(Stream src)
        {
            Color c = new Color();
            c.r = ReadNextByte(src);
            c.g = ReadNextByte(src);
            c.b = ReadNextByte(src);
            c.a = ReadNextByte(src);

            return c;
        }

        public Box ReadNextBox(Stream src)
        {
            Box b = new Box();
            b.min = ReadNextVector(src);
            b.max = ReadNextVector(src);
            b.isValid = ReadNextByte(src) > 0;

            return b;
        }

        public void SkipNextQuat(Stream src)
        {
            SkipNextFloat32(src);
            SkipNextFloat32(src);
            SkipNextFloat32(src);
            SkipNextFloat32(src);
        }

        public Quat ReadNextQuat(Stream src)
        {
            Quat q = new Quat();
            q.x = ReadNextFloat32(src);
            q.y = ReadNextFloat32(src);
            q.z = ReadNextFloat32(src);
            q.w = ReadNextFloat32(src);

            return q;
        }

        public InventoryItem ReadNextInventoryItem(Stream src)
        {
            InventoryItem item = new InventoryItem();
            item.objectType = ReadNextInt32(src);
            item.name = ReadNextString(src);
            item.objectRef = ReadNextWorldObjectRef(src);
            item.prop = ReadNextWorldObjectProperty(src);

            return item;
        }

        public RailroadTrackPosition ReadNextRailroadTrackPosition(Stream src)
        {
            RailroadTrackPosition ratp = new RailroadTrackPosition();

            ratp.objectRef = ReadNextWorldObjectRef(src);
            ratp.offset = ReadNextFloat32(src);
            ratp.forward = ReadNextFloat32(src);

            return ratp;
        }

        public Guid ReadNextGuid(Stream src)
        {
            return new Guid(ReadNext(src, 16));
        }

        public FluidBox ReadNextFluidBox(Stream src)
        {
            FluidBox fbox = new FluidBox();

            fbox.value = ReadNextFloat32(src);

            return fbox;
        }

        public FINNetworkTrace ReadNextFINNetworkTrace(Stream src)
        {
            FINNetworkTrace t = new FINNetworkTrace();

            t.isValid = ReadNextInt32(src) > 0;
            if (t.isValid) {
                t.objectRef = ReadNextWorldObjectRef(src);
                t.hasPrev = ReadNextInt32(src) > 0;

                if (t.hasPrev) {
                    t.prev = ReadNextFINNetworkTrace(src);
                }

                t.hasStep = ReadNextInt32(src) > 0;

                if (t.hasStep) {
                    t.step = ReadNextString(src);
                }
            }

            return t;
        }

        public WorldObjectArrayProperty ReadNextWorldObjectArrayProperty(Stream src, string type)
        {
            WorldObjectArrayProperty arr = new WorldObjectArrayProperty();

            arr.type = type;
            arr.length = ReadNextInt32(src);
            if (arr.type == "StructProperty") {
                arr.property = ReadNextWorldObjectProperty(src, arr.length);
            } else {
                arr.values = new List<object>();
                for (int i = 0; i < arr.length; i++) {
                    arr.values.Add(ReadNextWorldObjectPropertyValue(src, arr.type, null, i == 0, true));
                }
            }

            return arr;
        }

        public object ReadNextWorldObjectStructProperty(Stream src, string type)
        {
            object value;

            switch (type) {
                case "Vector2D":
                    value = ReadNextVector2D(src);
                    break;
                case "Rotator":
                    value = ReadNextRotator(src);
                    break;
                case "Vector":
                    value = ReadNextVector(src);
                    break;
                case "LinearColor":
                    value = ReadNextLinearColor(src);
                    break;
                case "Color":
                    value = ReadNextColor(src);
                    break;
                case "Box":
                    value = ReadNextBox(src);
                    break;
                case "Quat":
                    value = ReadNextQuat(src);
                    break;
                case "InventoryItem":
                    value = ReadNextInventoryItem(src);
                    break;
                case "RailroadTrackPosition":
                    value = ReadNextRailroadTrackPosition(src);
                    break;
                case "Guid":
                    value = ReadNextGuid(src);
                    break;
                case "FluidBox":
                    value = ReadNextFluidBox(src);
                    break;
                case "FINNetworkTrace":
                    value = ReadNextFINNetworkTrace(src);
                    break;
                default:
                    value = ReadNextWorldObjectDynamicStructProperty(src, type);
                    break;
            }

            return value;
        }

        public Dictionary<object,object> ReadNextWorldObjectMapProperty(
            Stream src,
            string keyType,
            string valueType
        ) {
            Int32 unk1 = ReadNextInt32(src);
            Int32 length = ReadNextInt32(src);
            Dictionary<object,object> values = new Dictionary<object,object>();
            for (int i = 0; i < length; i++) {
                object key = ReadNextWorldObjectPropertyValue(src, keyType, null, i==0, true, true);
                object value = null;

                if (valueType == "StructProperty") {
                    value = ReadNextWorldObjectDynamicStructProperty(src);
                } else {
                    value = ReadNextWorldObjectPropertyValue(src, valueType, null, i==0, true, true);
                }

                values.Add(key, value);
            }

            return values;
        }

        public WorldObjectTextProperty ReadNextWorldObjectTextProperty(Stream src)
        {
            WorldObjectTextProperty prop = new WorldObjectTextProperty();
            prop.flags = ReadNextInt32(src);
            prop.historyType = ReadNextByte(src);

            switch (prop.historyType) {
                case 0:
                    prop.ns = ReadNextString(src);
                    prop.key = ReadNextString(src);
                    prop.value = ReadNextString(src);
                    break;
                case 3:
                    prop.sourceFmt = ReadNextWorldObjectTextProperty(src);
                    prop.argsCount = ReadNextInt32(src);
                    prop.args = new List<WorldObjectTextPropertyArgument>();

                    for (int i = 0; i < prop.argsCount; i++) {
                        WorldObjectTextPropertyArgument arg = new WorldObjectTextPropertyArgument();
                        arg.name = ReadNextString(src);
                        arg.type = ReadNextByte(src);

                        if (arg.type == 4) {
                            arg.value = ReadNextWorldObjectTextProperty(src);
                        } else {
                            throw new NotImplementedException("Unknown argument type: " + arg.type);
                        }

                        prop.args.Add(arg);
                    }

                    break;
                default:
                    throw new NotImplementedException("Unknown history type: " + prop.historyType);
            }

            return prop;
        }

        public void SkipNextString(Stream src)
        {
            src.Position = Math.Abs(ReadNextInt32(src)) + src.Position;
        }

        public string ReadNextString(Stream src, int? length = null)
        {
            Int32 len = length.HasValue ? length.Value : ReadNextInt32(src);
            // bool isUTF16 = len < 0;

            int effectiveLength = Math.Abs(len) - 1;

            if (effectiveLength > 2048*1024) { // 2Gb
                throw new NotImplementedException("too big string skipped");
            }

            string val = "";
            if (effectiveLength > 0) {
                val = Encoding.UTF8.GetString(
                    ReadNext(src, effectiveLength)
                );
                src.Position++; // avoid \0 char
            }

            return val;
        }
        public Models.Diff.Property<string> DiffNextString(Stream to, Stream from)
        {
            Models.Diff.Property<string> val = new Models.Diff.Property<string>();
            val.to = ReadNextString(to);
            val.from = ReadNextString(from);

            return val;
        }

        public byte[] ReadNext(Stream src, int dataLen)
        {
            byte[] data = new byte[dataLen];

            src.Read(data, 0, dataLen);

            return data;
        }
        protected void unzip(Stream src, Stream dest)
        {
            InflaterInputStream inputStream = new InflaterInputStream(
                src,
                new Inflater(false)
            );

            inputStream.CopyTo(dest);
        }
    }
}