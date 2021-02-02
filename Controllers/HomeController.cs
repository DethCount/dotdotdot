using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using dotdotdot.Models;
using dotdotdot.Services;

namespace dotdotdot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISaveFileReader _saveFileReader;
        public HomeController(
            ILogger<HomeController> logger,
            ISaveFileReader saveFileReader
        )
        {
            _logger = logger;
            _saveFileReader = saveFileReader;
        }

        public IActionResult Index(string filename = null)
        {
            //string basePath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("\\bin"));
            // "country_autosave_0.sav"
            string basepath = _saveFileReader.GetBasePath();

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
            string basepath = _saveFileReader.GetBasePath();

            ViewData["basepath"] = basepath;
            ViewData["filename"] = filename;

            if (filename != null && filename.Trim().Length > 0) {
                string filepath = basepath + filename;
                ViewData["filepath"] = filepath;

                SaveFile f = _saveFileReader.Read(filepath);
                ViewData["saveFile"] = f;
            }

            return View();
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
