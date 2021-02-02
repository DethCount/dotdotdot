using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using dotdotdot.Models;

namespace dotdotdot.Services
{
    public class SaveFileReader : ISaveFileReader
    {
        public string GetBasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "\\FactoryGame\\Saved\\SaveGames\\";
        }

        public SaveFile Read(string filepath)
        {
            FileStream src = new FileStream(
                filepath,
                FileMode.Open, 
                FileAccess.Read
            );
                
            SaveFile f = ReadNextSaveFile(src);
            f.filepath = filepath;

            return f;
        }

        public SaveFile ReadNextSaveFile(Stream src)
        {
            SaveFile f = new SaveFile();

            f.saveHeaderVersion = ReadNextInt32(src);
            f.saveVersion = ReadNextInt32(src);
            f.buildVersion = ReadNextInt32(src);
            f.worldType = ReadNextString(src);
            f.worldProperties = ReadNextString(src);
            f.sessionName = ReadNextString(src);
            f.playTime = ReadNextInt32(src);
            f.saveDate = new DateTime((long) ReadNextInt64(src));
            f.sessionVisibility = ReadNextByte(src);
            
            MemoryStream worldObjectSrc = new MemoryStream();

            long uncompressedSize = 0;
            while (src.Position < src.Length)
            {
                SaveFileChunkHeader header = ReadNextSaveFileChunkHeader(src);
                long start = src.Position;
                unzip(src, worldObjectSrc);
                src.Position = start + header.currentChunkCompressedLength;
                uncompressedSize += header.currentChunkUncompressedLength;
            }

            src.Dispose();
            worldObjectSrc.Position = 0;

            f.worldObjectLength = ReadNextInt32(worldObjectSrc);
            f.worldObjectCount = ReadNextInt32(worldObjectSrc);
            f.worldObjects = new WorldObject[f.worldObjectCount];

            for (int i = 0; i < f.worldObjectCount; i++) {
                f.worldObjects[i] = ReadNextWorldObject(worldObjectSrc);
            }

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
            obj.rootObject = ReadNextString(src);
            obj.instanceName = ReadNextString(src);

            switch (obj.type) {
                case 0:
                    SaveComponent component = new SaveComponent();
                    component.parentEntityName = ReadNextString(src);
                    obj.value = component;
                    break;
                case 1:
                    SaveEntity entity = new SaveEntity();
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

        public string ReadNextString(Stream src, int? length = null)
        {
            Int32 len = length.HasValue ? length.Value : ReadNextInt32(src);
            // bool isUTF16 = len < 0;
            
            int effectiveLength = Math.Abs(len) - 1;

            string val = effectiveLength > 0 
                ? Encoding.UTF8.GetString(
                    ReadNext(src, effectiveLength)
                )
                : "";

            src.Position++; // avoid \0 char
            
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