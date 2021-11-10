using IDE.Themes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDE.Themes.Services {

    /// <summary>
    /// ThemeConverter class providing functionility for IThemeConverter. Takes in a user-input file (theme) as a parameter. 
    /// For further description of the functionality, see IThemeConverter interface.
    /// </summary>

    public class ThemeConverter {

        /*PRIVATE VARIABLES*/

        #region Helper algorithm variables
        string plainTextBgColor = "";
        string currentLineColor = "";
        #endregion Helper algorithm variables

        #region helper injection variables

        private readonly IWebHostEnvironment environment;
        private readonly ThemeDictionary dictionary;
        private readonly ColorStringConverter colorConverter;

        #endregion helper injection variables

        /*PROPERTIES*/

        #region string path properties 

        public string ThemeName { get; set; }

        public string ThemeDir { get; set; }

        #endregion paths

        /*CONSTRUCTOR*/

        #region constructor
        public ThemeConverter(IWebHostEnvironment environment, ThemeDictionary dictionary, ColorStringConverter colorConverter) {

            this.environment = environment;
            this.dictionary = dictionary;
            this.colorConverter = colorConverter;
        }
        #endregion constructor


        /*METHODS*/

        #region Copy a template theme, serving as a new template file
        public void CreateTempFile(IFormFile file) {

            //Eclipse to VS
            if (Path.GetExtension(file.FileName) == ".xml"|| Path.GetExtension(file.FileName) == ".epf") {

                string templateDirVS = environment.ContentRootPath + @"/Files/templateTheme.vssettings";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = environment.ContentRootPath + @"/Files/TempTheme/" + ThemeName + @".vssettings";

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

                string templateDirEclipse = environment.ContentRootPath + @"/Files/templateTheme.xml";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = environment.ContentRootPath + @"/Files/TempTheme/" + ThemeName + @".xml";

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

            string rootPath = environment.ContentRootPath + @"/Files/TempTheme/";
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
            using (var reader = new StreamReader(file.OpenReadStream())) {

                return fileText = await reader.ReadToEndAsync();
            }
        }
        #endregion IFormFile to string conversion (async)

        #region IFormFile to string list conversion (async)
        public List<String> FileToStringList(IFormFile file) {

            List<String> result = new List<String>();

            using (var reader = new StreamReader(file.OpenReadStream())) {

                while (reader.Peek() >= 0) {
                    result.Add(reader.ReadLine());
                }
            }

            return result;
        }
        #endregion IFormFile to string list conversion (async)

        #region Extract theme items from .epf file into .xml

        public string EpfToXml(IFormFile file, ThemeDictionary dictionary) {

            //setup an artificial xml file (string to be returned)
            string templateDirEclipse = environment.ContentRootPath + @"/Files/templateTheme.xml";
            string xmlFile = File.ReadAllText(templateDirEclipse);
            string[] xmlLines = File.ReadAllLines(templateDirEclipse);
            IList<String> test = new List<string>();

            List<String> epfLines = FileToStringList(file);

            foreach(string line in epfLines) {

                foreach(KeyValuePair<String,String> epfEntry in dictionary.EpfValues){

                    if (line.Contains(epfEntry.Value)) {

                        //grab the color
                        int startRIndex = line.IndexOf("=")+1;
                        string epfRGB = line.Substring(startRIndex);

                        //convert the color to hex
                        string hex = colorConverter.EpfRGBToHex(epfRGB);

                        //add this color to an xml entry
                        foreach(string xmlLine in xmlLines) {

                            if (xmlLine.Contains(epfEntry.Key)) {

                                int startI = xmlLine.IndexOf("color=\"") + 7;
                                string color = hex;
                                if (color != "") {
                                    //adjust new color to Background or Foreground and replace it to the final file
                                    string tempColor = xmlLine.Substring(startI, 7);
                                    var newItem = xmlLine.Replace(tempColor, color);
                                    xmlFile = xmlFile.Replace(xmlLine, newItem);
                                    test.Add(xmlLine);
                                    break;
                                }
                            }
                        }
                    }
                } 
            }

            IList<String> testt = test;

            return xmlFile;

        }
        #endregion Extract theme items from .epf file into .xml

        #region Convert Eclipse to VS, create and populate a .vssettings theme file
        public void ConvertEclipseToVisual(IFormFile file, ThemeDictionary dictionary) {

            IList<String> newTheme = new List<String>() { };
            int counter = 0;

            string eclipseFileText = "";

            if (file.FileName.Contains(".xml")) {

                eclipseFileText = FileToStringAsync(file).Result;
            }
            else if (file.FileName.Contains(".epf")) {

                eclipseFileText = EpfToXml(file, dictionary);
            }

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] eclipseItems = Regex.Split(eclipseFileText, @"(?=<)");

            //delete all the comments as they can interfere with the algorithm
            List<String> items = eclipseItems.ToList<String>();
            for (int i = 0; i < items.Count; i++) {
                string item = items[i];
                if (item.Contains("<!")) {
                    items.RemoveAt(i);
                }
            }

            eclipseItems = items.ToArray();

            //create xyz.vssettings file in ~\Files\TempTheme\xyz.vssettings
            CreateTempFile(file);

            //path of the created file = ThemeDir
            string vsFileText = File.ReadAllText(ThemeDir);

            //split the single string into <Item.../> strings, preserving the delimeters
            string[] vsItems = Regex.Split(vsFileText, @"(?=<)");
            newTheme = vsItems.ToList<String>();

            foreach (KeyValuePair<String, VsSettingsModel> entry in dictionary.Mapping) {

                string currentKey = entry.Key;

                //remove quotes from the key
                currentKey = currentKey.Remove(0,1);
                currentKey = currentKey.Remove(currentKey.Length-1,1);

                //extract the color from an Eclipse key
                foreach (string eclipseItem in eclipseItems) {

                    string color="";

                    if (eclipseItem.Contains(currentKey)) {
                        counter++;

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
                        if (currentKey == "foreground") {
                            valuesToChange.Add("\"Plain Text\"");
                            valuesToChange.Add("\"Line Number\"");
                        }

                        //remove empty values
                        valuesToChange = valuesToChange.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                        if (valuesToChange.Any()) {

                            foreach(string value in valuesToChange) {

                                foreach(string vsItem in vsItems) {

                                    if (vsItem.Contains(value) && value != "") {
                                        
                                        if(counter == 28) {
                                            string s = "";
                                        }

                                        string oldItem = vsItem;
                                        int startI;

                                        //outliers that change Background instead of Foreground
                                        if ((value == "\"CurrentLineActiveFormat\"" || value == "\"Selected Text\"" || value == "\"HTML Server-Side Script\"" || value == "\"Breakpoint (Enabled)\"" || value == "\"Current Statement\"" || value == "\"Plain Text\"" || value == "\"Line Number\"" || value == "\"MarkerFormatDefinition/HighlightedReference\"" || value == "\"MarkerFormatDefinition/HighlightedWrittenReference\"" || value == "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\"" || value== "\"Line Numbers\"" || value == "\"MarkerFormatDefinition/FindHighlight\"")&& currentKey != "foreground") {
                                            startI = vsItem.IndexOf("Background=\"") + 12;
                                            string tempColor = vsItem.Substring(startI, 10);
                                            var newItem = vsItem.Replace(tempColor, color);
                                            vsFileText = vsFileText.Replace(oldItem, newItem);
                                            newTheme.Add(newItem);

                                            //two outliers where Foreground AND Background colors need to be changed
                                            if (value == "\"Plain Text\"") {
                                                plainTextBgColor = color;
                                            }
                                            if (value == "\"Line Number\"") {
                                                currentLineColor = color;
                                            }
                                        }

                                        //else change the foreground
                                        if(!(value == "\"CurrentLineActiveFormat\"" || value == "\"Selected Text\"" || value == "\"HTML Server-Side Script\"" || value == "\"Breakpoint (Enabled)\"" || value == "\"Current Statement\"" ||  value == "\"MarkerFormatDefinition/HighlightedReference\"" || value == "\"MarkerFormatDefinition/HighlightedWrittenReference\"" || value == "\"MarkerFormatDefinition/HighlightParameterFormatDefinition\"" || value == "\"MarkerFormatDefinition/FindHighlight\"")) {
                                            startI = vsItem.IndexOf("Foreground=\"") + 12;
                                            string tempColor = vsItem.Substring(startI, 10);
                                            var newItem = vsItem.Replace(tempColor, color);
                                            vsFileText = vsFileText.Replace(oldItem, newItem);
                                            newTheme.Add(newItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            //upadate background for PlainText and CurrentLine
            string[] vsItemsUpdated = Regex.Split(vsFileText, @"(?=<)");

            foreach (string vsItem in vsItemsUpdated) {

                if (vsItem.Contains("\"Plain Text\"")){
                    int startI = vsItem.IndexOf("Background=\"") + 12;
                    string tColor = vsItem.Substring(startI, 10);
                    var newItem = vsItem.Replace(tColor, plainTextBgColor);
                    vsFileText = vsFileText.Replace(vsItem, newItem);
                }
                if(vsItem.Contains("\"Line Number\"")) {
                    int startI = vsItem.IndexOf("Background=\"") + 12;
                    string tColor = vsItem.Substring(startI, 10);
                    var newItem = vsItem.Replace(tColor, plainTextBgColor);
                    startI = vsItem.IndexOf("Foreground=\"") + 12;
                    string tempColor = vsItem.Substring(startI, 10);
                    var newItem2 = newItem.Replace(tempColor, currentLineColor);
                    vsFileText = vsFileText.Replace(vsItem, newItem2);
                }
            }

            string[] convertedItems = Regex.Split(vsFileText, @"(?=<)");

            //grab all the VS values
            List<String> allValues = new List<String>();
            foreach(KeyValuePair<String,VsSettingsModel> entry in dictionary.Mapping){
                allValues.AddRange(entry.Value.CCsharp);
                allValues.AddRange(entry.Value.Cpp);
                allValues.AddRange(entry.Value.CssScss);
                allValues.AddRange(entry.Value.Html);
                allValues.AddRange(entry.Value.Xaml);
                allValues.AddRange(entry.Value.Xml);
            }

            //remove empty values
            allValues = allValues.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            //update convertedItems values with new values from newTheme list
            foreach (string value in allValues) {
                for (int i = 0; i < convertedItems.Length; i++) {
                    foreach (string nItem in newTheme) {
                        if (convertedItems[i].Contains(value) && nItem.Contains(value) && value!= "\"Plain Text\"" && value != "\"Line Number\"") {
                            convertedItems[i] = nItem;
                        }
                    }
                }
            }

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

                            if (valueToCompare == "Plain Text" || valueToCompare == "Selected Text" || valueToCompare == "CurrentLineActiveFormat" || valueToCompare == "HTML Server-Side Script" || valueToCompare == "Breakpoint (Enabled)" || valueToCompare == "Current Statement" || valueToCompare == "MarkerFormatDefinition/FindHighlight" || valueToCompare == "MarkerFormatDefinition/HighlightedReference" || valueToCompare == "MarkerFormatDefinition/HighlightedWrittenReference" || valueToCompare == "MarkerFormatDefinition/HighlightParameterFormatDefinition") {

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
