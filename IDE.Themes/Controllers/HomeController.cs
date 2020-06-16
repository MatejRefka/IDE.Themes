using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IDE.Themes.Data;
using IDE.Themes.Models;
using IDE.Themes.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace IDE.Themes.Controllers {

    public class HomeController : Controller {

        /*PRIVATE FIELDS*/

        #region Helper fields set by the constructor
        private ThemeConverter converter;
        private ThemeDictionary dictionary;
        private IWebHostEnvironment environment;
        private UserColorDataModel dataModel;
        private IConfiguration config;
        #endregion Helper fields set by the constructor

        /*CONSTRUCTOR*/

        #region Default constructor
        public HomeController(ThemeConverter converter, ThemeDictionary dictionary, IWebHostEnvironment environment, UserColorDataModel dataModel, IConfiguration config) {

            this.environment = environment;
            this.converter = converter;
            this.dictionary = dictionary;
            this.dataModel = dataModel;
            this.config = config;
        }
        #endregion Default constructor

        /*VIEWS*/

        #region Default Index View [GET]

        [HttpGet]
        public IActionResult Index(HomeModel model) {
            
            return View(model);
        }
        #endregion Default Index View [GET]

        #region Convert the given file based in user input [POST]
        [HttpPost]
        public async Task<IActionResult> Convert(HomeModel homeModel, IFormFile file) {

            //if a field is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view

            if (homeModel.IdeFrom == "Select IDE" || homeModel.IdeTo == "Select IDE") {

                return RedirectToAction("Index");
            }


            //if the file type is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view "text/xml" 

            if (!(homeModel.IdeFrom == "Eclipse" && Path.GetExtension(file.FileName) == ".xml") && !(homeModel.IdeFrom == "Visual Studio" && Path.GetExtension(file.FileName) == ".vssettings")) {

                return RedirectToAction("Index");
            }


            //convert the file based on user input

            if (homeModel.IdeFrom == "Eclipse" && homeModel.IdeTo == "Visual Studio") {
                converter.ConvertEclipseToVisual(file, dictionary);
            }

            else if (homeModel.IdeFrom == "Visual Studio" && homeModel.IdeTo == "Eclipse") {
                converter.ConvertVisualToEclipse(file, dictionary, homeModel);
            }
            

            //Populate the dataModel with data from the Eclipse theme

            //Id
            Guid id = Guid.NewGuid();
            dataModel.ColorId = id.ToString();

            //Date
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            dataModel.Date = currentTime;

            //Add the theme colors from .xml theme

            if (homeModel.IdeFrom == "Eclipse") {

                string fileTextString = await converter.FileToStringAsync(file);

                //split the single string into <Item.../> strings, preserving the delimeters
                string[] eclipseItems = Regex.Split(fileTextString, @"(?=<)");

                Type dataModelType = dataModel.GetType();

                //compare dataModel Property names with eclipse items to access and set the correct color to the dataModel
                foreach(string item in eclipseItems) {

                    foreach (PropertyInfo propertyInfo in dataModelType.GetProperties()) {

                        string propertyName = propertyInfo.Name;

                        string propertyNameLower = Char.ToLower(propertyName[0]) + propertyName.Substring(1);

                        string color = "";

                        if (item.Contains("<"+propertyNameLower)) {

                            int startIndex = item.IndexOf("color=\"") + 7;
                            color = item.Substring(startIndex, item.IndexOf("\"") - startIndex + 8);

                            dataModel.GetType().GetProperty(propertyName).SetValue(dataModel,color);
                        }
                    }
                }
            }

            else if (homeModel.IdeFrom=="Visual Studio") { 

                //get the converted Eclipse theme
                string themeName = Path.GetFileNameWithoutExtension(file.FileName);
                string themeDir = environment.ContentRootPath + @"\Files\TempTheme\" + themeName + @".xml";
                string fileTextString = System.IO.File.ReadAllText(themeDir);

                //split the single string into <Item.../> strings, preserving the delimeters
                string[] eclipseItems = Regex.Split(fileTextString, @"(?=<)");

                Type dataModelType = dataModel.GetType();

                //compare dataModel Property names with eclipse items to access and set the correct color to the dataModel
                foreach (string item in eclipseItems) {

                    foreach (PropertyInfo propertyInfo in dataModelType.GetProperties()) {

                        string propertyName = propertyInfo.Name;

                        string propertyNameLower = Char.ToLower(propertyName[0]) + propertyName.Substring(1);

                        string color = "";

                        if (item.Contains("<"+propertyNameLower)) {

                            int startIndex = item.IndexOf("color=\"") + 7;
                            color = item.Substring(startIndex, item.IndexOf("\"") - startIndex + 8);

                            dataModel.GetType().GetProperty(propertyName).SetValue(dataModel, color);
                        }
                    }
                }
            }
    

            //Add the populated dataModel to the DB context & SaveChanges to the DB itself
            //context of the DB, shortest possible connection
            using (var context = new ApplicationDbContext(config)) {

                context.Database.EnsureCreated();

                context.UserColorTable.Add(dataModel);

                context.SaveChanges();
            }

            return RedirectToAction("Index", homeModel);

        }
        #endregion Convert the given file based in user input

        #region Download the new File [GET]
        [HttpGet]
        public IActionResult Download() {

            string filePath = converter.ThemeDir;
            string fileName = "";

            if (filePath.Contains("xml")) {

                fileName = converter.ThemeName + ".xml";
            }
            else if (filePath.Contains("vssettings")) {

                fileName = converter.ThemeName + ".vssettings";
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", fileName);
        }
        #endregion Download the new File [GET]

        #region Error
        public IActionResult Error() {

            return View();
        }
        #endregion Error

    }
}
