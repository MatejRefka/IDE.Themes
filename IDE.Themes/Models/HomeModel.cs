using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace IDE.Themes.Models {

    /// <summary>
    /// HomeInput model capturing all the user input from the home page.
    /// </summary>
    public class HomeModel {

        //dropdowns:
        [BindProperty] 
        public string IdeFrom { get; set; } = "Select IDE";

        [BindProperty]
        public string IdeTo { get; set; } = "Select IDE";

        //checkboxes:
        [BindProperty]
        public bool CCsharp { get; set; } = false;

        [BindProperty]
        public bool Cpp { get; set; } = false;

        [BindProperty]
        public bool CssScss { get; set; } = false;

        [BindProperty]
        public bool Html { get; set; } = false;

        [BindProperty]
        public bool Xml { get; set; } = false;

        [BindProperty]
        public bool Xaml { get; set; } = false;


        public static string testc() {
            return ".htm";
        }
    }
}
