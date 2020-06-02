using IDE.Themes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDE.Themes.Services {

    /// <summary>
    /// ThemeConverter class providing functionility for IThemeConverter. Takes in a user-input file (theme) as a parameter. 
    /// For further description of the functionality, see IThemeConverter interface.
    /// </summary>

    public class ThemeConverter {

        /*PRIVATE VARIABLES*/

        #region helper injection variables

        private readonly IWebHostEnvironment environment;
        private readonly ThemeDictionary dictionary;

        #endregion helper injection variables

        /*PROPERTIES*/

        #region string path properties 

        public string ThemeName { get; set; }

        public string ThemeDir { get; set; }

        #endregion paths

        /*CONSTRUCTOR*/

        public ThemeConverter(IWebHostEnvironment environment, ThemeDictionary dictionary) {

            this.environment = environment;
            this.dictionary = dictionary;
        }

        /*METHODS*/

        #region Copy a template theme, serving as a new template file
        public void CreateTempFile(IFormFile file) {

            //Eclipse to VS
            if (Path.GetExtension(file.FileName) == ".xml") {

                string templateDirVS = environment.ContentRootPath + @"\Files\templateTheme.vssettings";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = environment.ContentRootPath + @"\Files\TempTheme\" + ThemeName + @".vssettings";

                try {
                    File.Copy(templateDirVS, ThemeDir);
                }
                catch {
                    DeleteThemes();
                    File.Copy(templateDirVS, ThemeDir);
                }
            }

            //VS to Eclipse
            else if (Path.GetExtension(file.FileName) == ".vssettings") {

                string templateDirEclipse = environment.ContentRootPath + @"\Files\templateTheme.xml";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = environment.ContentRootPath + @"\Files\TempTheme\" + ThemeName + @".xml";

                try {
                    File.Copy(templateDirEclipse, ThemeDir);
                }
                catch {
                    DeleteThemes();
                    File.Copy(templateDirEclipse, ThemeDir);
                }
            }
        }
        #endregion Copy a template theme, serving as a new template file

        #region Delete the contructed theme (after download)
        public void DeleteThemes() {

            string rootPath = environment.ContentRootPath + @"\Files\TempTheme\";
            DirectoryInfo directory = new DirectoryInfo(rootPath);

            foreach (FileInfo file in directory.GetFiles()) {
                file.Delete();

            }
        }
        #endregion Delete the contructed theme (after download)

        public async Task<String> FileToStringAsync(IFormFile file) {

            string fileText;

            //read IFormFile to a string
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream)) {

                return fileText = await reader.ReadToEndAsync();
            }
        }

        public String ConvertEclipseToVisual(IFormFile file, ThemeDictionary dictionary) {

            string eclipseFileText=FileToStringAsync(file).Result;

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] eclipseItems = Regex.Split(eclipseFileText, @"(?=<)");
            

            //create xyz.vssettings file in ~\Files\TempTheme\xyz.vssettings
            CreateTempFile(file);

            //path of the created file = ThemeDir
            string vsFileText = File.ReadAllText(ThemeDir);

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] vsItems = Regex.Split(vsFileText, @"(?=<)");

            List<String> updatedVsItems = new List<string>();

            foreach (KeyValuePair<String, VsSettingsModel> entry in dictionary.Mapping) {

                string currentKey = entry.Key;

                //saving the color for these 2 keys
                string plainTextForegroundColor = "";
                string lineNumberForegroundColor = "";

                //find the colors for the 2 keys
                foreach(string row in eclipseItems) {
                    if (row.Contains("foreground")) {
                        int startIndex = row.IndexOf("color=\"") + 7;
                        plainTextForegroundColor = row.Substring(startIndex, row.IndexOf("\"") - startIndex + 8);
                        plainTextForegroundColor = ConvertColor(plainTextForegroundColor);
                    }
                    if (row.Contains("background")) {
                        int startIndex = row.IndexOf("color=\"") + 7;
                        lineNumberForegroundColor = row.Substring(startIndex, row.IndexOf("\"") - startIndex + 8);
                        lineNumberForegroundColor = ConvertColor(lineNumberForegroundColor);
                    }
                }

                //extract the color from an Eclipse key
                foreach (string eclipseItem in eclipseItems) {

                    string color;

                    if (eclipseItem.Contains(currentKey)) {

                        int startIndex = eclipseItem.IndexOf("color=\"") + 7;
                        color = eclipseItem.Substring(startIndex, eclipseItem.IndexOf("\"") - startIndex + 8);
                        color = ConvertColor(color);

                        //list of all VS values for an Eclipse key 
                        List<String> valuesToChange = entry.Value.CCsharp;
                        valuesToChange.AddRange(entry.Value.Cpp);
                        valuesToChange.AddRange(entry.Value.CssScss);
                        valuesToChange.AddRange(entry.Value.Html);
                        valuesToChange.AddRange(entry.Value.Xaml);
                        valuesToChange.AddRange(entry.Value.Xml);

                        //remove empty values
                        valuesToChange = valuesToChange.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                        if (valuesToChange.Any()) {

                            foreach(string value in valuesToChange) {

                                foreach(string vsItem in vsItems) {

                                    if (vsItem.Contains(value) && value != "") {

                                        string oldItem = vsItem;
                                        int startI;

                                        //outliers that change Background instead of Foreground
                                        if (value == "\"CurrentLineActiveFormat\"" || value == "\"Selected Text\"" || value == "\"HTML Server-Side Script\"" || value == "\"Breakpoint (Enabled)\"" || value == "\"Current Statement\"" || value == "\"Plain Text\"" || value == "\"Line Number\"") {
                                            startI = vsItem.IndexOf("Background=\"") + 12;

                                            //two instances where a single VS item changes foreground and background
                                            if(value == "\"Plain Text\"") {
                                                int startF = vsItem.IndexOf("Foreground=\"") + 12;
                                                string tempCol = vsItem.Substring(startF, 10);
                                                var newI = vsItem.Replace(tempCol, plainTextForegroundColor);
                                                string tempC = vsItem.Substring(startI, 10);
                                                var newIt = newI.Replace(tempC, color);
                                                vsFileText = vsFileText.Replace(oldItem, newIt);
                                                break;
                                            }
                                            if(value == "\"Line Number\"") {
                                                int startF = vsItem.IndexOf("Foreground=\"") + 12;
                                                string tempCol = vsItem.Substring(startF, 10);
                                                var newI = vsItem.Replace(tempCol, color);
                                                string tempC = vsItem.Substring(startI, 10);
                                                var newIt = newI.Replace(tempC, lineNumberForegroundColor);
                                                vsFileText = vsFileText.Replace(oldItem, newIt);
                                                break;
                                            }
                                        }

                                        //else change the foreground
                                        else {
                                            startI = vsItem.IndexOf("Foreground=\"") + 12;
                                        }

                                        //adjust new color to Background or Foreground and replace it to the final file
                                        string tempColor = vsItem.Substring(startI, 10);
                                        var newItem = vsItem.Replace(tempColor, color);
                                        vsFileText = vsFileText.Replace(oldItem, newItem);

                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] convertedItems = Regex.Split(vsFileText, @"(?=<)");

            //write the new theme to TempTheme folder
            File.WriteAllLines(ThemeDir, convertedItems);




            //for each key in dictionary
            //find correct entry in eclipseItems
            //extract string color
            //convert to BIOS color
            //for each value 
            //find correct entry in vsItems
            //string replace

            //write and format vsItems to temp



            return null;

        }



        public string ConvertVisualToEclipse() {

            return null;
        }

        public string ConvertColor(String color) {

            //eclipse to VS
            if(color[0]=='#') {

                //#FF79C6 = 0x00C679FF
                string biosColor = "0x00" + color[5] + color[6] + color[3] + color[4] + color[1] + color[2];
                return biosColor;
            }
            //VS to Eclipse
            if (color[0]==0) {

                //0x00C679FF = #FF79C6
                string hexColor = "#" + color[8] + color[9] + color[6] + color[7] + color[4] + color[5];
                return hexColor;
            }

            return null;
        }


    }
}
