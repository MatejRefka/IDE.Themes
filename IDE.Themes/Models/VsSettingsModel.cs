using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.Models {

    /// <summary>
    /// Model for VS .vssettings theme variables. Used as a single value for the theme
    /// conversion mapping, resulting in a 1 to 1. In the background however it will be 
    /// a one to many mapping.
    /// </summary>

    public class VsSettingsModel {

        public List<String> CCsharp { get; set; }
        public List<String> Cpp { get; set; }
        public List<String> CssScss { get; set; }
        public List<String> Html { get; set; }
        public List<String> Xml { get; set; }
        public List<String> Xaml { get; set; }

    }
}
