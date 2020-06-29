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

    public class AboutController : Controller {

        /*PRIVATE FIELDS*/

        /*CONSTRUCTOR*/

        #region Default constructor
        public AboutController() {


        }
        #endregion Default constructor

        /*VIEWS*/
        public IActionResult Index() {

            return View();
        }

        public IActionResult Error() {

            return View();
        }


        /*METHODS*/


    }
}
