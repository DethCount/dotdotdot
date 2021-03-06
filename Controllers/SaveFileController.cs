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

        public ViewModels.Diff.Read Diff(string filename1, string filename2)
        {
            ViewModels.Diff.Read diff = new ViewModels.Diff.Read();
            diff.basepath = _saveFileReader.GetBasePath();
            diff.filename1 = filename1;
            diff.filename2 = filename2;

            if (filename1 != null
                && filename1.Trim().Length > 0
                && filename2 != null
                && filename2.Trim().Length > 0
            ) {
                diff.file = _saveFileReader.ReadDiff(
                    diff.basepath + filename2,
                    diff.basepath + filename1
                );
            }

            return diff;
        }

        [Route("api/save-file/{filename2}/diff/{filename1}")]
        [Produces("application/json")]
        public JsonResult DiffJson(string filename1, string filename2)
        {
            return Json(Diff(filename1, filename2));
        }

        public ViewModels.Diff.Header HeaderDiff(string filename1, string filename2)
        {
            ViewModels.Diff.Header diff = new ViewModels.Diff.Header();
            diff.basepath = _saveFileReader.GetBasePath();
            diff.filename1 = filename1;
            diff.filename2 = filename2;

            if (filename1 != null
                && filename1.Trim().Length > 0
                && filename2 != null
                && filename2.Trim().Length > 0
            ) {
                diff.header = _saveFileReader.ReadHeaderDiff(
                    diff.basepath + filename2,
                    diff.basepath + filename1
                );
            }

            return diff;
        }

        [Route("api/save-file/{filename2}/diff/{filename1}/header")]
        [Produces("application/json")]
        public JsonResult HeaderDiffJson(
            string filename1,
            string filename2
        ) {
            return Json(HeaderDiff(filename1, filename2));
        }

        public ViewModels.Diff.Objects ObjectsDiff(string filename1, string filename2)
        {
            ViewModels.Diff.Objects diff = new ViewModels.Diff.Objects();
            diff.basepath = _saveFileReader.GetBasePath();
            diff.filename1 = filename1;
            diff.filename2 = filename2;

            if (filename1 != null
                && filename1.Trim().Length > 0
                && filename2 != null
                && filename2.Trim().Length > 0
            ) {
                diff.objects = _saveFileReader.ReadObjectsDiff(
                    diff.basepath + filename2,
                    diff.basepath + filename1
                );
            }

            return diff;
        }

        [Route("api/save-file/{filename2}/diff/{filename1}/objects")]
        [Produces("application/json")]
        public JsonResult ObjectsDiffJson(
            string filename1,
            string filename2
        ) {
            return Json(ObjectsDiff(filename1, filename2));
        }


        public ViewModels.Diff.Properties PropertiesDiff(string filename1, string filename2)
        {
            ViewModels.Diff.Properties diff = new ViewModels.Diff.Properties();
            diff.basepath = _saveFileReader.GetBasePath();
            diff.filename1 = filename1;
            diff.filename2 = filename2;

            if (filename1 != null
                && filename1.Trim().Length > 0
                && filename2 != null
                && filename2.Trim().Length > 0
            ) {
                diff.properties = _saveFileReader.ReadPropertiesDiff(
                    diff.basepath + filename2,
                    diff.basepath + filename1
                );
            }

            return diff;
        }

        [Route("api/save-file/{filename2}/diff/{filename1}/properties")]
        [Produces("application/json")]
        public JsonResult PropertiesDiffJson(
            string filename1,
            string filename2
        ) {
            return Json(PropertiesDiff(filename1, filename2));
        }
    }
}
