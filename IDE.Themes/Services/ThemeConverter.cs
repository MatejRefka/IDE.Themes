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

        #region constructor
        public ThemeConverter(IWebHostEnvironment environment, ThemeDictionary dictionary) {

            this.environment = environment;
            this.dictionary = dictionary;
        }
        #endregion constructor


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

        #region IFormFile to string conversion (async)
        public async Task<String> FileToStringAsync(IFormFile file) {

            string fileText;

            //read IFormFile to a string
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream)) {

                return fileText = await reader.ReadToEndAsync();
            }
        }
        #endregion IFormFile to string conversion (async)


        #region Convert Eclipse to VS, create and populate a .vssettings theme file
        public void ConvertEclipseToVisual(IFormFile file, ThemeDictionary dictionary) {

            string eclipseFileText=FileToStringAsync(file).Result;

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] eclipseItems = Regex.Split(eclipseFileText, @"(?=<)");
            

            //create xyz.vssettings file in ~\Files\TempTheme\xyz.vssettings
            CreateTempFile(file);

            //path of the created file = ThemeDir
            string vsFileText = File.ReadAllText(ThemeDir);

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] vsItems = Regex.Split(vsFileText, @"(?=<)");

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

        }
        #endregion Convert Eclipse to VS, create and populate a .vssettings theme file

        #region Convert VS to Eclipse, create and populate an .xml theme file
        public void ConvertVisualToEclipse(IFormFile file, ThemeDictionary dictionary, HomeModel model) {

            string vsFileText = FileToStringAsync(file).Result;

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] vsItems = Regex.Split(vsFileText, @"(?=<)");


            //create xyz.xml file in ~\Files\TempTheme\xyz.xml
            CreateTempFile(file);

            //path of the created file = ThemeDir
            string eclipseFileText = File.ReadAllText(ThemeDir);

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] eclipseItems = Regex.Split(eclipseFileText, @"(?=<)");

            int counter = 0;

            foreach (KeyValuePair<String, VsSettingsModel> entry in dictionary.Mapping) { //current key = javadoc

                counter++;

                bool doubleCheckVal = false;

                string currentKey = entry.Key;
                string correctValue = "";

                //find the correct value for this key based on preference
                if (model.CCsharp) {
                    if (entry.Value.CCsharp[0] != "") {
                        foreach (string value in entry.Value.CCsharp) {
                            foreach (string vsItem in vsItems) {
                                if (vsItem.Contains(value)) {
                                    correctValue = value;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (model.Cpp) {
                    if (entry.Value.Cpp[0] != "") {
                        correctValue = entry.Value.Cpp[0];
                    }
                    else {

                        if (dictionary.EmptyValues.ContainsKey(currentKey)) {
                            string backupKey = dictionary.EmptyValues[currentKey];

                            if (dictionary.Mapping[backupKey].Cpp[0] != "") {
                                correctValue = dictionary.Mapping[backupKey].Cpp[0];
                            }
                        }
                    }
                }
                else if (model.CssScss) {
                    if (entry.Value.CssScss[0] != "") {
                        correctValue = entry.Value.CssScss[0];
                    }
                    else {

                        if (dictionary.EmptyValues.ContainsKey(currentKey)) {
                            string backupKey = dictionary.EmptyValues[currentKey];

                            if (dictionary.Mapping[backupKey].CssScss[0] != "") {
                                correctValue = dictionary.Mapping[backupKey].CssScss[0];
                            }
                        }
                    }
                }
                else if (model.Html) {
                    if (entry.Value.Html[0] != "") {
                        correctValue = entry.Value.Html[0];
                    }
                    else {

                        if (dictionary.EmptyValues.ContainsKey(currentKey)) {
                            string backupKey = dictionary.EmptyValues[currentKey];

                            if (dictionary.Mapping[backupKey].Html[0] != "") {
                                correctValue = dictionary.Mapping[backupKey].Html[0];
                            }
                        }
                    }
                }
                else if (model.Xaml) {
                    if (entry.Value.Xaml[0] != "") {
                        correctValue = entry.Value.Xaml[0];
                    }
                    else {

                        if (dictionary.EmptyValues.ContainsKey(currentKey)) {
                            string backupKey = dictionary.EmptyValues[currentKey];

                            if (dictionary.Mapping[backupKey].Xaml[0] != "") {
                                correctValue = dictionary.Mapping[backupKey].Xaml[0];
                            }
                        }
                    }
                }
                else if (model.Xml) {
                    if (entry.Value.Xml[0] != "") {
                        correctValue = entry.Value.Xml[0];
                    }
                    else {

                        if (dictionary.EmptyValues.ContainsKey(currentKey)) {
                            string backupKey = dictionary.EmptyValues[currentKey];

                            if (dictionary.Mapping[backupKey].Xml[0] != "") {
                                correctValue = dictionary.Mapping[backupKey].Xml[0];
                            }
                        }
                    }
                }

                //if there's no preference and the mapping doesn't have a default CCsharp value, then atrificially get a replacement default value
                if (entry.Value.CCsharp[0] == "" && correctValue == "") {
                    foreach (KeyValuePair<String, String> defaultEntry in dictionary.DefaultValues) {

                        if (defaultEntry.Key == currentKey) {
                            correctValue = defaultEntry.Value;
                            doubleCheckVal = true;
                            break;
                        }
                    }
                }

                //else use the default value for CCsharp.
                if (correctValue == "" || correctValue ==null) {

                    foreach (string value in entry.Value.CCsharp) {
                        foreach (string vsItem in vsItems) {
                            if (vsItem.Contains(value)) {
                                correctValue = value;
                                break;
                            }
                        }
                    }
                }

                //if a default value was used then check for other values in the same list if they match an original key instead.
                if (doubleCheckVal == true) {

                    string correctkey = dictionary.Mapping.FirstOrDefault(x => x.Value.CCsharp[0] == correctValue).Key;
                    foreach (string vsItem in vsItems) {
                        foreach(string value in dictionary.Mapping[correctkey].CCsharp) {
                            if (vsItem.Contains(value)) {
                                correctValue = value;
                                break;
                            }
                        }
                    }
                }

                string color = "";

                //extract and convert the color from correct value

                if (correctValue != "") {
                    foreach (string vsItem in vsItems) {


                        string valueToCompare = correctValue;
                        valueToCompare = valueToCompare.Remove(0, 1);
                        valueToCompare = valueToCompare.Remove(valueToCompare.Length - 1);

                        if (vsItem.Contains(valueToCompare)) {

                            if (valueToCompare == "Plain Text" || valueToCompare == "Selected Text" || valueToCompare == "CurrentLineActiveFormat" || valueToCompare == "HTML Server-Side Script" || valueToCompare == "Breakpoint (Enabled)" || valueToCompare == "Current Statement") {

                                int startIndex = vsItem.IndexOf("Background=\"") + 12;
                                color = vsItem.Substring(startIndex, 10);
                                color = ConvertColor(color);
                                break;

                            }
                            else {
                                int startIndex = vsItem.IndexOf("Foreground=\"") + 12;
                                color = vsItem.Substring(startIndex, 10);
                                color = ConvertColor(color);
                                break;
                            }
                        }
                    }

                    //replace the colors for each new eclipse item

                    if (correctValue != "") {

                        foreach (string eclipseItem in eclipseItems) {

                            string keyToCompare = currentKey;
                            keyToCompare = keyToCompare.Remove(0, 1);
                            keyToCompare = keyToCompare.Remove(keyToCompare.Length - 1);
                            if (eclipseItem.Contains(keyToCompare)) {

                                int startI = eclipseItem.IndexOf("color=\"") + 7;

                                if (color != "") {
                                    //adjust new color to Background or Foreground and replace it to the final file
                                    string tempColor = eclipseItem.Substring(startI, 7);
                                    var newItem = eclipseItem.Replace(tempColor, color);
                                    eclipseFileText = eclipseFileText.Replace(eclipseItem, newItem);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            string[] convertedItems = Regex.Split(eclipseFileText, @"(?=<)");

            //write the new theme to TempTheme folder
            File.WriteAllLines(ThemeDir, convertedItems);

            //remove empty lines
            var lines = File.ReadAllLines(ThemeDir).Where(arg => !string.IsNullOrWhiteSpace(arg));
            File.WriteAllLines(ThemeDir, lines);

        }
        #endregion Convert VS to Eclipse, create and populate a .vssettings theme file


        #region Hex color <=> BIOS color code converter
        public string ConvertColor(String color) {

            //eclipse to VS
            if(color[0]=='#') {

                //#FF79C6 = 0x00C679FF
                string biosColor = "0x00" + color[5] + color[6] + color[3] + color[4] + color[1] + color[2];
                return biosColor;
            }
            //VS to Eclipse
            if (color[0]=='0') {

                //0x00C679FF = #FF79C6
                string hexColor = "#" + color[8] + color[9] + color[6] + color[7] + color[4] + color[5];
                return hexColor;
            }

            return null;
        }
        #endregion Hex color <=> BIOS color code converter

    }
}
