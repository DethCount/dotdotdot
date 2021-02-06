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
    }
}
