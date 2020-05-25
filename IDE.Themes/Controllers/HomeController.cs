using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IDE.Themes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace IDE.Themes.Controllers {

    public class HomeController : Controller {

        /*VIEWS*/

        [HttpGet]
        public IActionResult Index() {
            
            return View(new HomeModel());
        }


        [HttpPost]
        public async Task<IActionResult> Download(HomeModel homeModel, IFormFile file) {

            #region Check Field Validity

            //if a field is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view

            if (homeModel.IdeFrom == "Select IDE" || homeModel.IdeTo == "Select IDE") {

                return RedirectToAction("Index");
            }

            #endregion Check Field Validity

            #region Check File Type Validation

            //if the file type is invalid, do nothing and just return the view
            //error prompt for the user is handled by JQuery in the view "text/xml" 

            if (!(homeModel.IdeFrom == "Eclipse" && Path.GetExtension(file.FileName) == ".xml") && !(homeModel.IdeFrom == "Visual Studio" && Path.GetExtension(file.FileName) == ".vssettings")) {

                return RedirectToAction("Index");
            }

            #endregion Check File Type Validation

            #region Convert File

            int i = 2;






            #endregion

            //download file

            //await store file


            return RedirectToAction("Index");
        }


        public IActionResult Error() {

            return View();
        }


    }
}
