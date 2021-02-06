using System;
using System.Collections.Generic;
using System.IO;
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

            return ReadNextSaveFileObjects(
                GetStreamFromFilepath(filepath),
                worldObjectsSrc,
                true
            );
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
            header.saveDate = new DateTime((long) ReadNextInt64(src));
            header.sessionVisibility = ReadNextByte(src);

            return header;
        }

        public void SkipHeader(Stream src)
        {
            src.Position += 12;
            src.Position = Math.Abs(ReadNextInt32(src)) + src.Position;
            src.Position = Math.Abs(ReadNextInt32(src)) + src.Position;
            src.Position = Math.Abs(ReadNextInt32(src)) + src.Position;
            src.Position += 13;
        }
        public void SkipObjects(Stream src, Stream worldObjectSrc)
        {
            UnzipSaveFileObjects(src, worldObjectSrc);
            src.Dispose();
            worldObjectSrc.Position = 0;

            Int32 objectsLength = ReadNextInt32(worldObjectSrc);
            src.Position += objectsLength;
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
            bool skipPreviousBlocks = true
        ) {
            SaveFileObjects objects = new SaveFileObjects();

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
                objects.objects.Add(ReadNextWorldObject(worldObjectSrc));
            }

            return objects;
        }

        public SaveFileProperties ReadNextSaveFileProperties(
            Stream src,
            bool skipPreviousBlocks = true
        ) {
            SaveFileProperties properties = new SaveFileProperties();
            properties.objectsCount = ReadNextInt32(src);
            
            properties.properties = new List<WorldObjectProperties>();
            for (int i = 0; i < properties.objectsCount; i++) {
                /* debug
                if (i == 7908) {
                    byte[] datablock = new byte[f.worldObjects[i].propertiesLength];
                    long pos = worldObjectSrc.Position;
                    //worldObjectSrc.Position = pos + l - 1024;
                    string block = ReadNextString(worldObjectSrc, l);
                    worldObjectSrc.Position = pos;
                    string str = f.worldObjects[i].id.instanceName;
                }
                //*/

                properties.properties.Add(ReadNextWorldObjectProperties(src));
            }

            return properties;
        }

        public SaveFile ReadNextSaveFile(Stream src)
        {
            SaveFile f = new SaveFile();

            f.header = ReadNextSaveFileHeader(src);
            MemoryStream worldObjectSrc = new MemoryStream();
            f.objects = ReadNextSaveFileObjects(src, worldObjectSrc, false);
            f.properties = ReadNextSaveFileProperties(worldObjectSrc, false);

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

        public WorldObject ReadNextWorldObject(Stream src)
        {
            WorldObject obj = new WorldObject();
            obj.type = ReadNextInt32(src);
            obj.typePath = ReadNextString(src);
            obj.id = ReadNextWorldObjectRef(src);

            switch (obj.type) {
                case 0:
                    WorldObjectData component = new WorldObjectData();
                    component.type = "Component";
                    component.parentEntityName = ReadNextString(src);
                    obj.value = component;
                    break;
                case 1:
                    WorldObjectData entity = new WorldObjectData();
                    entity.type = "Entity";
                    entity.needTransform = ReadNextInt32(src) == 1;
                    entity.rotation = (
                        ReadNextFloat32(src),
                        ReadNextFloat32(src),
                        ReadNextFloat32(src),
                        ReadNextFloat32(src)
                    );
                    entity.position = (
                        ReadNextFloat32(src),
                        ReadNextFloat32(src),
                        ReadNextFloat32(src)
                    );
                    entity.scale = (
                        ReadNextFloat32(src),
                        ReadNextFloat32(src),
                        ReadNextFloat32(src)
                    );
                    entity.wasPlacedInLevel = ReadNextInt32(src) == 1;
                    obj.value = entity;
                    break;
                default:
                    break;
            }

            return obj;
        }

        public WorldObjectProperties ReadNextWorldObjectProperties(Stream src)
        {
            WorldObjectProperties properties = new WorldObjectProperties();
            properties.properties = new List<WorldObjectProperty>();

            Int64 start = src.Position;
            
            properties.size = ReadNextInt32(src);

            try {
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
            } catch (Exception) {
                src.Position = start;
            }

            while (src.Position < start + properties.size) {
                WorldObjectProperty prop = ReadNextWorldObjectProperty(src);
                if (prop == null) {
                    break;
                }

                properties.properties.Add(prop);
                long notReaded = start + properties.size - src.Position;

                /*
                if (notReaded < 20) {
                    long pos = src.Position;
                    string str = ReadNextString(src, (int) notReaded);
                    src.Position = pos;
                }
                */
            }

            long missingBytes = start + properties.size - src.Position;
            if (missingBytes > 0) {
                src.Position += missingBytes; // skip unknown 
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

        public Byte ReadNextByte(Stream src)
        {
            byte[] bytes = ReadNext(src, 1);

            return (Byte) bytes[0];
        }

        public Int32 ReadNextInt32(Stream src)
        {
            return BitConverter.ToInt32(ReadNext(src, 4));
        }

        public Int64 ReadNextInt64(Stream src)
        {
            return BitConverter.ToInt64(ReadNext(src, 8));
        }

        public Single ReadNextFloat32(Stream src)
        {
            return BitConverter.ToSingle(ReadNext(src, 4));
        }

        public Vector2D ReadNextVector2D(Stream src)
        {
            Vector2D v = new Vector2D();
            v.x = ReadNextFloat32(src);
            v.y = ReadNextFloat32(src);
            
            return v;
        }

        public Rotator ReadNextRotator(Stream src)
        {
            Rotator r = new Rotator();
            r.x = ReadNextFloat32(src);
            r.y = ReadNextFloat32(src);
            r.z = ReadNextFloat32(src);

            return r;
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