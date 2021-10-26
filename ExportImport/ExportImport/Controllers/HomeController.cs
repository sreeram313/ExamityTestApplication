using ExportImport.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExportImport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View(GetFiles());
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

        public List<FileNames> GetFiles()
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "Files");

            List<FileNames> lstFiles = new List<FileNames>();
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            int i = 0;

            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new FileNames()
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });

                i++;
            }

            return lstFiles;
        }

        public FileResult Download(string id)
        {
            int fid = Convert.ToInt32(id);
            var files = GetFiles();
            string filename = (from f in files
                               where f.FileId == fid
                               select f.FilePath).First();
           
            
            FileInfo fileInfo = new FileInfo(filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileInfo.FullName);
            string contentType = "application/pdf";

            return File(fileBytes, contentType, fileInfo.Name);
        }
    }
}
