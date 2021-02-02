using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using dotdotdot.Models;

namespace dotdotdot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger
        )
        {
            _logger = logger;
        }

        protected string GetBasePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                + "\\FactoryGame\\Saved\\SaveGames\\";
        }

        public IActionResult Index(string filename = null)
        {
            //string basePath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("\\bin"));
            // "country_autosave_0.sav"
            string basepath = GetBasePath();

            ViewData["basepath"] = basepath;

            List<ValueTuple<string,DateTime>> files = new List<ValueTuple<string,DateTime>>();

            foreach (var dir in Directory.GetDirectories(basepath))
            {
                long playerId;
                string dirname = Path.GetFileName(dir);
                if (long.TryParse(dirname, out playerId)) {
                    files.AddRange(
                        ((new DirectoryInfo(dir)).GetFiles("*.sav"))
                            .OrderByDescending(f => f.LastWriteTime)
                            .Select((FileInfo f) => (f.FullName, f.LastWriteTime))
                            .ToList()
                    );
                }
            }
            
            ViewData["files"] = files;

            return View();
        }

        [Route("file/{filename}", Name = "File")]
        public IActionResult File(string filename)
        {
            string basepath = GetBasePath();

            ViewData["basepath"] = basepath;
            ViewData["filename"] = filename;

            if (filename != null && filename.Trim().Length > 0) {
                string filepath = basepath + filename;
                ViewData["filepath"] = filepath;

                FileStream src = new FileStream(
                    filepath,
                    FileMode.Open, 
                    FileAccess.Read
                );
                
                SaveFile f = readNextSaveFile(src);
                f.filepath = filepath;

                ViewData["saveFile"] = f;
            }

            return View();
        }

        protected SaveFile readNextSaveFile(Stream src)
        {
            SaveFile f = new SaveFile();

            f.saveHeaderVersion = readNextInt32(src);
            f.saveVersion = readNextInt32(src);
            f.buildVersion = readNextInt32(src);
            f.worldType = readNextString(src);
            f.worldProperties = readNextString(src);
            f.sessionName = readNextString(src);
            f.playTime = readNextInt32(src);
            f.saveDate = new DateTime((long) readNextInt64(src));
            f.sessionVisibility = readNextByte(src);
            
            MemoryStream worldObjectSrc = new MemoryStream();

            long uncompressedSize = 0;
            while (src.Position < src.Length)
            {
                SaveFileChunkHeader header = readNextSaveFileChunkHeader(src);
                long start = src.Position;
                unzip(src, worldObjectSrc);
                src.Position = start + header.currentChunkCompressedLength;
                uncompressedSize += header.currentChunkUncompressedLength;
            }

            src.Dispose();
            worldObjectSrc.Position = 0;

            f.worldObjectLength = readNextInt32(worldObjectSrc);
            f.worldObjectCount = readNextInt32(worldObjectSrc);
            f.worldObjects = new WorldObject[f.worldObjectCount];

            for (int i = 0; i < f.worldObjectCount; i++) {
                f.worldObjects[i] = readNextWorldObject(worldObjectSrc);
            }

            return f;
        }

        protected SaveFileChunkHeader readNextSaveFileChunkHeader(Stream src)
        {
            SaveFileChunkHeader header = new SaveFileChunkHeader();

            header.packageFileTag = readNextInt64(src);
            header.maxChunkSize = readNextInt64(src);
            header.currentChunkCompressedLength = readNextInt64(src);
            header.currentChunkUncompressedLength = readNextInt64(src);
            header.currentChunkCompressedLength2 = readNextInt64(src);
            header.currentChunkUncompressedLength2 = readNextInt64(src);

            return header;
        }

        protected WorldObject readNextWorldObject(Stream src)
        {
            WorldObject obj = new WorldObject();
            obj.type = readNextInt32(src);
            obj.typePath = readNextString(src);
            obj.rootObject = readNextString(src);
            obj.instanceName = readNextString(src);

            switch (obj.type) {
                case 0:
                    SaveComponent component = new SaveComponent();
                    component.parentEntityName = readNextString(src);
                    obj.value = component;
                    break;
                case 1:
                    SaveEntity entity = new SaveEntity();
                    entity.needTransform = readNextInt32(src) == 1;
                    entity.rotation = (
                        readNextFloat32(src),
                        readNextFloat32(src),
                        readNextFloat32(src),
                        readNextFloat32(src)
                    );
                    entity.position = (
                        readNextFloat32(src),
                        readNextFloat32(src),
                        readNextFloat32(src)
                    );
                    entity.scale = (
                        readNextFloat32(src),
                        readNextFloat32(src),
                        readNextFloat32(src)
                    );
                    entity.wasPlacedInLevel = readNextInt32(src) == 1;
                    obj.value = entity;
                    break;
                default:
                    break;
            }

            return obj;
        }

        public void unzip(Stream src, Stream dest)
        {
            InflaterInputStream inputStream = new InflaterInputStream(
                src, 
                new Inflater(false)
            );
            
            inputStream.CopyTo(dest);
        }

        protected Byte readNextByte(Stream src)
        {
            byte[] bytes = readNext(src, 1);

            return (Byte) bytes[0];
        }

        protected Int32 readNextInt32(Stream src)
        {
            return BitConverter.ToInt32(readNext(src, 4));
        }

        protected Int64 readNextInt64(Stream src)
        {
            return BitConverter.ToInt64(readNext(src, 8));
        }

        protected Single readNextFloat32(Stream src)
        {
            return BitConverter.ToSingle(readNext(src, 4));
        }

        protected string readNextString(Stream src, int? length = null)
        {
            Int32 len = length.HasValue ? length.Value : readNextInt32(src);
            // bool isUTF16 = len < 0;
            
            int effectiveLength = Math.Abs(len) - 1;

            string val = effectiveLength > 0 
                ? Encoding.UTF8.GetString(
                    readNext(src, effectiveLength)
                )
                : "";

            src.Position++; // avoid \0 char
            
            return val;
        }

        protected byte[] readNext(Stream src, int dataLen)
        {
            byte[] data = new byte[dataLen];

            src.Read(data, 0, dataLen);

            return data;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
