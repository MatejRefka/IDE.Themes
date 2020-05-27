using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.Services {

    /// <summary>
    /// ThemeConverter class providing functionility for IThemeConverter. Takes in a user-input file (theme) as a parameter. 
    /// For further description of the functionality, see IThemeConverter interface.
    /// </summary>

    public class ThemeConverter : IThemeConverter{

        private readonly IWebHostEnvironment environment;

        public string ThemeName { get; set; }

        public string ThemeDir { get; set; }


        public ThemeConverter() {

        }


        public void CreateTempFile(IFormFile file) {

            if (Path.GetExtension(file.FileName) == ".xml") {

                IWebHostEnvironment env = new IWebHostEnvironment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                string templateDirVS = environment.WebRootPath + @"\Files\templateTheme.vssettings";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = Directory.GetCurrentDirectory()+ @"\Files\" + ThemeName + @".vssetting";

                File.Copy(templateDirVS, ThemeDir);
            }

            else if (Path.GetExtension(file.FileName) == ".vssettings") {

                string templateDirEclipse = environment.WebRootPath + @"~\Files\templateTheme.xml";

                ThemeName = Path.GetFileNameWithoutExtension(file.FileName);
                ThemeDir = @"~\Files\" + ThemeName + @".xml";

                File.Copy(templateDirEclipse, ThemeDir);
            }
        }



        public async Task<string> ConvertEclipseToVisualAsync(IFormFile file) {

            string themeContent;
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))

                themeContent = await reader.ReadToEndAsync();


            return null;


        }


        public string ConvertVisualToEclipse() {

            return null;
        }



    }
}
