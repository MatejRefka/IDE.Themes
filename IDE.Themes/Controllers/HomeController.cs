using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IDE.Themes.Models;
using IDE.Themes.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace IDE.Themes.Controllers {

    public class HomeController : Controller {

        private ThemeConverter converter;
        private ThemeDictionary dictionary;
        private IWebHostEnvironment environment;
        private HomeModel model;

        public HomeController(ThemeConverter converter, ThemeDictionary dictionary, IWebHostEnvironment environment) {

            this.environment = environment;
            this.converter = converter;
            this.dictionary = dictionary;
        }



        /*VIEWS*/

        [HttpGet]
        public IActionResult Index(HomeModel model) {
            
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Convert(HomeModel homeModel, IFormFile file, ThemeDictionary dictionary) {

            #region Check Field Validity

            //if a field is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view

            if (homeModel.IdeFrom == "Select IDE" || homeModel.IdeTo == "Select IDE") {

                return RedirectToAction("Index");
            }

            #endregion Check Field Validity

            #region Check File Type Validation

            //if the file type is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view "text/xml" 

            if (!(homeModel.IdeFrom == "Eclipse" && Path.GetExtension(file.FileName) == ".xml") && !(homeModel.IdeFrom == "Visual Studio" && Path.GetExtension(file.FileName) == ".vssettings")) {

                return RedirectToAction("Index");
            }

            #endregion Check File Type Validation

            #region Convert File

            if (homeModel.IdeFrom == "Eclipse" && homeModel.IdeTo == "Visual Studio") {
                converter.ConvertEclipseToVisual(file, dictionary);
            }

            else if (homeModel.IdeFrom == "Visual Studio" && homeModel.IdeTo == "Eclipse") {
                converter.ConvertVisualToEclipse(file, dictionary, homeModel);
            }
            #endregion

            //await store file




            return RedirectToAction("Index",homeModel);
        }

        [HttpGet]
        public IActionResult Download() {

            #region Download File
            string filePath = converter.ThemeDir;
            string fileName = "";

            if (filePath.Contains("xml")) {

                fileName = converter.ThemeName + ".xml";
            }
            else if (filePath.Contains("vssettings")) {

                fileName = converter.ThemeName + ".vssettings";
            }


            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);


            #endregion Download File

            //return RedirectToAction("Index");

            return File(fileBytes, "application/force-download", fileName);
        }


        public IActionResult Error() {

            return View();
        }


    }
}
