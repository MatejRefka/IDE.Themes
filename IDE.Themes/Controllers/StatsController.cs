﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace IDE.Themes.Controllers {

    public class StatsController : Controller {

        /*VIEWS*/
        public IActionResult Index() {
            
            return View();
        }

        public IActionResult Error() {

            return View();
        }


    }
}
