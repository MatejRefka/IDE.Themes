using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.Services {

    /// <summary>
    /// ThemeConverter class providing functionility for IThemeConverter. Takes in a user-input file (theme) as a parameter. 
    /// For further description of the functionality, see IThemeConverter interface.
    /// </summary>

    public class ThemeConverter : IThemeConverter{

        private IFormFile file;

        public ThemeConverter(IFormFile file) {

            this.file = file;
        }

        public string ConvertEclipseToVisual() {

            return null;
        }
        

        public string ConvertVisualToEclipse() {

            return null;
        }



    }
}
