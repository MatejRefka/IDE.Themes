using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IDE.Themes.Data;
using IDE.Themes.Models;
using IDE.Themes.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace IDE.Themes.Controllers {

    public class HomeController : Controller {

        /*PRIVATE FIELDS*/

        #region Helper fields set by the constructor
        private ThemeConverter tConverter;
        private ThemeDictionary dictionary;
        private IWebHostEnvironment environment;
        private UserColorDataModel dataModel;
        private IConfiguration config;
        private ColorStringConverter cConverter;
        private HelperModel hModel;
        #endregion Helper fields set by the constructor

        #region Helper fields holding temp values or some state
        private int[] fgroundOccurences = new int[13];
        private int[] bgroundOccurences = new int[13];
        private List<HashSet<string>> colorRecords = new List<HashSet<string>>();
        private HashSet<string> recordSet = new HashSet<string>();
        private List<string> graphColors = new List<string> { "Red", "Pink", "Purple", "Blue", "Cyan", "Teal", "Green", "Yellow", "Orange", "Brown", "White", "Black" };
        private static int extraCounter = 0; 
        #endregion Helper fields holding temp values or some state

        /*CONSTRUCTOR*/

        #region Default constructor
        public HomeController(ThemeConverter tConverter, ThemeDictionary dictionary, IWebHostEnvironment environment, UserColorDataModel dataModel, IConfiguration config, ColorStringConverter cConverter, HelperModel hModel) {

            this.environment = environment;
            this.tConverter = tConverter;
            this.dictionary = dictionary;
            this.dataModel = dataModel;
            this.config = config;
            this.cConverter = cConverter;
            this.hModel = hModel;

        }
        #endregion Default constructor

        /*VIEWS*/

        #region Default Index View [GET]

        [HttpGet]
        public IActionResult Index(HomeModel hModel) {

            //control of the Download button disabled false/true (not the optimal solution but works) 
            if (extraCounter == 0) {
                ViewData["DownloadCounter"] = 0;
            }
            else {
                extraCounter = 0;
                ViewData["DownloadCounter"] = 1;
            }
            
            return View(hModel);
        }
        #endregion Default Index View [GET]

        #region Convert the given file based in user input [POST]

        [HttpPost]
        public async Task<IActionResult> Convert(HomeModel homeModel, IFormFile file) {

            //delete any previously converted themes in TempTheme
            tConverter.DeleteThemes();

            //if a field is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view
            if (homeModel.IdeFrom == "Select IDE" || homeModel.IdeTo == "Select IDE") {

                return RedirectToAction("Index");
            }

            //if the file type is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view "text/xml" 
            if (!(homeModel.IdeFrom == "Eclipse" && (Path.GetExtension(file.FileName) == ".xml"|| Path.GetExtension(file.FileName) == ".epf")) && !(homeModel.IdeFrom == "Visual Studio" && Path.GetExtension(file.FileName) == ".vssettings")) {

                return RedirectToAction("Index");
            }

            //convert the file based on user input
            if (homeModel.IdeFrom == "Eclipse" && homeModel.IdeTo == "Visual Studio") {
                tConverter.ConvertEclipseToVisual(file, dictionary);
            }

            else if (homeModel.IdeFrom == "Visual Studio" && homeModel.IdeTo == "Eclipse") {
                tConverter.ConvertVisualToEclipse(file, dictionary, homeModel);
            }

            //Populate the dataModel with data from the Eclipse theme and update the DB
            await AddNewRecordAsync(homeModel.IdeFrom, file);

            //controls the download button disabled/enabled
            ViewData["DownloadCounter"] = 1;
            extraCounter = 1;

            return RedirectToAction("Index", homeModel);

        }
        #endregion Convert the given file based in user input

        #region Download the new File [GET]

        [HttpGet]
        public IActionResult Download() {

            string filePath = tConverter.ThemeDir;
            string fileName = "";

            if (filePath.Contains("xml")) {

                fileName = tConverter.ThemeName + ".xml";
            }
            else if (filePath.Contains("vssettings")) {

                fileName = tConverter.ThemeName + ".vssettings";
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);


            return File(fileBytes, "application/force-download", fileName);

        }
        #endregion Download the new File [GET]

        #region Error IActionResult [GET]

        [HttpGet]
        public IActionResult Error() {

            return View();
        }
        #endregion Error IActionResult

        /*METHODS*/

        #region Given RGB values, determine what color it is
        public string IdentifyRGB(int r, int g, int b) {

            string filePath = environment.ContentRootPath + "\\Files\\satfaces.txt";

            IEnumerable<string> lines = System.IO.File.ReadLines(filePath);

            //format of the satfaces colors eg. [0, 230, 22]
            string rgbFace = "[" + r + ", " + g + ", " + b + "]";

            foreach (string s in lines) {

                if (s.Contains(rgbFace)) {

                    int startIndex = s.IndexOf(rgbFace)+rgbFace.Length+1;

                    string color = s.Substring(startIndex, s.Length-rgbFace.Length-1);

                    switch (color) {

                        case "red":
                        case "maroon":
                            return "red";

                        case "pink":
                        case "magenta":
                            return "pink";

                        case "purple":
                        case "dark purple":
                            return "purple";

                        case "blue":
                        case "dark blue":
                        case "navy blue":
                        case "sky blue":
                        case "light blue":
                            return "blue";

                        case "cyan":
                            return "cyan";

                        case "teal":
                        case "dark teal":
                            return "teal";

                        case "green":
                        case "dark green":
                        case "lime green":
                        case "olive":
                            return "green";

                        case "yellow":
                        case "mustard":
                        case "gold":
                            return "yellow";

                        case "orange":
                            return "orange";

                        case "brown":
                        case "dark brown":
                            return "brown";

                        case "black":
                            return "black";

                    }
                }
            }
            return null;
        }
        #endregion Given RGB values, determine what color it is

        #region Add a new record to the database from a theme file

        public async Task AddNewRecordAsync(string themeFrom, IFormFile file) {

            //Id
            Guid id = Guid.NewGuid();
            dataModel.ColorId = id.ToString();

            //Date
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            dataModel.Date = currentTime;

            string[] eclipseItems;

            //Add the theme colors from .xml theme
            if (themeFrom == "Eclipse") {

                string fileTextString = await tConverter.FileToStringAsync(file);

                //split the single string into <Item.../> strings, preserving the delimeters
                eclipseItems = Regex.Split(fileTextString, @"(?=<)");

            }
            else {

                //get the converted Eclipse theme
                string themeName = Path.GetFileNameWithoutExtension(file.FileName);
                string themeDir = environment.ContentRootPath + @"\Files\TempTheme\" + themeName + @".xml";
                string fileTextString = System.IO.File.ReadAllText(themeDir);

                //split the single string into <Item.../> strings, preserving the delimeters
                eclipseItems = Regex.Split(fileTextString, @"(?=<)");
            }

            Type dataModelType = dataModel.GetType();

            //compare dataModel Property names with eclipse items to access and set the correct color to the dataModel
            foreach (string item in eclipseItems) {

                string trimmedItem = Regex.Replace(item, @"\s+", "");

                //if it is a valid eclipse entry
                if (trimmedItem.Contains("color=\"") && !trimmedItem.Contains("color=\"\"")) { 


                    int startIndex = item.IndexOf("color=\"") + 7;
                    string hexColor = item.Substring(startIndex, item.IndexOf("\"") - startIndex + 8);

                    string convertedColor = "";

                    try {
                        Color checkRgbValid = cConverter.HexToRGB(hexColor);
                    }
                    catch (Exception) {
                        break;
                    }

                    Color rgb = cConverter.HexToRGB(hexColor);

                    if (hexColor == "#FFFFFF" || hexColor == "#ffffff") {

                        convertedColor = "white";
                    }

                    //if the difference between rgb values is less than 5, eg. RGB(245,243,246) = valid, RGB(35,35,45) = invalid. They also must be light enough for it to not be classed as white
                    if (Math.Abs(rgb.R - rgb.G) <= 8 && Math.Abs(rgb.G - rgb.B) <= 8 && Math.Abs(rgb.B - rgb.R) <= 8 && rgb.R >= 30 && rgb.G >=30 && rgb.B >= 30 && convertedColor!="white") {    

                        convertedColor = "gray";
                    }
                    else if (convertedColor == "") { 
                        
                        double[] hsv = cConverter.RGBToHSV(rgb);
                        double[] maxHsv = cConverter.MaxSaturationHSV(hsv);
                        Color convertedRgb = cConverter.HSVToRGB(maxHsv);

                        convertedColor = IdentifyRGB(convertedRgb.R, convertedRgb.G, convertedRgb.B);
                    }

                    //compare the converted color with DataModel property name, if they match then update the property to true
                    foreach (PropertyInfo propertyInfo in dataModelType.GetProperties()) {

                        string propertyName = propertyInfo.Name;

                        string propertyNameLower = propertyName.ToLowerInvariant();

                        if (trimmedItem.Contains("<currentLine") || trimmedItem.Contains("<background") || trimmedItem.Contains("<selectionBackground") || trimmedItem.Contains("<searchResultIndication") || trimmedItem.Contains("<filteredSearchResultIndication") || trimmedItem.Contains("<occurrenceIndication") || trimmedItem.Contains("<writeOccurrenceIndication") || trimmedItem.Contains("<findScope")) {

                            if (propertyNameLower.Contains("b" + convertedColor)) {

                                dataModel.GetType().GetProperty(propertyName).SetValue(dataModel, true);
                            }
                        }
                        else {

                            if (propertyNameLower.Contains(convertedColor)&&!propertyNameLower.Contains("b"+convertedColor)) {

                                dataModel.GetType().GetProperty(propertyName).SetValue(dataModel, true);
                            }
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
        }
        #endregion Add a new record to the database from a theme file

    }
}
