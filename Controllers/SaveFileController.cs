using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using dotdotdot.Models;
using ViewModels = dotdotdot.Models.View.SaveFile;
using dotdotdot.Services;

namespace dotdotdot.Controllers
{
    [Produces("application/json")]
    public class SaveFileController : Controller
    {
        private readonly ILogger<SaveFileController> _logger;
        private ISaveFileReader _saveFileReader;
        public SaveFileController(
            ILogger<SaveFileController> logger,
            ISaveFileReader saveFileReader
        )
        {
            _logger = logger;
            _saveFileReader = saveFileReader;
        }

        public ViewModels.List List()
        {
            ViewModels.List list = new ViewModels.List();

            list.basepath = _saveFileReader.GetBasePath();
            list.files = new List<ViewModels.ListItem>();

            foreach (var dir in Directory.GetDirectories(list.basepath))
            {
                long playerId;
                string dirname = Path.GetFileName(dir);
                if (long.TryParse(dirname, out playerId)) {
                    list.files.AddRange(
                        ((new DirectoryInfo(dir)).GetFiles("*.sav"))
                            .OrderByDescending(f => f.LastWriteTime)
                            .Select((FileInfo f) => new ViewModels.ListItem(
                                f.FullName.Replace(list.basepath, ""), 
                                f.FullName, 
                                f.LastWriteTime
                            ))
                    );
                }
            }

            return list;
        }
        
        [Route("api/save-file/list")]
        [Produces("application/json")]
        public JsonResult ListJson()
        {
            return Json(List());
        }

        public ViewModels.Read Read(string filename)
        {
            ViewModels.Read read = new ViewModels.Read();
            read.basepath = _saveFileReader.GetBasePath();
            read.filename = filename;

            if (filename != null && filename.Trim().Length > 0) {
                read.file = _saveFileReader.Read(read.basepath + filename);
            }

            return read;
        }

        [Route("api/save-file/{filename}")]
        [Produces("application/json")]
        public JsonResult ListJson(string filename)
        {
            return Json(Read(filename));
        }

        public ViewModels.Header Header(string filename)
        {
            ViewModels.Header header = new ViewModels.Header();
            header.basepath = _saveFileReader.GetBasePath();
            header.filename = filename;

            if (filename != null && filename.Trim().Length > 0) {
                header.header = _saveFileReader.ReadHeader(header.basepath + filename);
            }

            return header;
        }

        [Route("api/save-file/{filename}/header")]
        [Produces("application/json")]
        public JsonResult HeaderJson(string filename)
        {
            return Json(Header(filename));
        }

        public ViewModels.Objects Objects(string filename)
        {
            ViewModels.Objects objects = new ViewModels.Objects();
            objects.basepath = _saveFileReader.GetBasePath();
            objects.filename = filename;

            if (filename != null && filename.Trim().Length > 0) {
                objects._objects = _saveFileReader.ReadObjects(
                    objects.basepath + filename
                );
            }

            return objects;
        }

        [Route("api/save-file/{filename}/objects")]
        [Produces("application/json")]
        public JsonResult ObjectsJson(string filename)
        {
            return Json(Objects(filename));
        }
        
        public ViewModels.Properties Properties(string filename)
        {
            ViewModels.Properties properties = new ViewModels.Properties();
            properties.basepath = _saveFileReader.GetBasePath();
            properties.filename = filename;

            if (filename != null && filename.Trim().Length > 0) {
                properties._properties = _saveFileReader.ReadProperties(
                    properties.basepath + filename
                );
            }

            return properties;
        }

        [Route("api/save-file/{filename}/properties")]
        [Produces("application/json")]
        public JsonResult PropertiesJson(string filename)
        {
            return Json(Properties(filename));
        }
    }
}
