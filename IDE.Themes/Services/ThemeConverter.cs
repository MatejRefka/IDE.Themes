using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Security.Policy;
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

        #endregion helper injection variables

        /*PROPERTIES*/

        #region string path properties 

        public string ThemeName { get; set; }

        public string ThemeDir { get; set; }

        #endregion paths

        /*CONSTRUCTOR*/

        public ThemeConverter(IWebHostEnvironment environment) {

            this.environment = environment;
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
