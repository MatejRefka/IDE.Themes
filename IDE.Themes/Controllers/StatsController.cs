using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IDE.Themes.Data;
using IDE.Themes.Models;
using IDE.Themes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace IDE.Themes.Controllers {

    public class StatsController : Controller {

        /*PRIVATE FIELDS*/

        #region Helper fields for the constructor
        private ColorStringConverter cConverter;
        private IConfiguration config;
        private UserColorDataModel dataModel;
        #endregion Helper fields for the constructor


        /*CONSTRUCTOR*/

        #region Default Constructor
        public StatsController(ColorStringConverter cConverter, IConfiguration config, UserColorDataModel dataModel) {

            this.cConverter = cConverter;
            this.config = config;
            this.dataModel = dataModel;

        }
        #endregion Default Constructor

        /*VIEWS*/

        #region Default Index IActionResult [GET]

        [HttpGet]
        public IActionResult Index() {

            //Items to be displayed in the view (passed with ViewData)
            string totalConversions = ""; 
            DateTimeOffset currentFirstDate = new DateTimeOffset(2112, 02, 29, 12, 43, 0, TimeSpan.FromSeconds(0));
            DateTimeOffset currentLastDate = new DateTimeOffset(2012, 02, 29, 12, 43, 0, TimeSpan.FromSeconds(0));

            int[] foregroundOccurences = new int[14];
            foregroundOccurences[13] = 0;
            int[] backgroundOccurences = new int[14];
            backgroundOccurences[13] = 0;

            List<UserColorDataModel> allModels = RetrieveRecords();

            foreach (UserColorDataModel model in allModels) {

                Type type = model.GetType();

                IList<PropertyInfo> properties = new List<PropertyInfo>(type.GetProperties());

                //iterate through the properties (reflection)
                foreach (PropertyInfo property in properties) {

                    if (!(property.Name == "ColorId" || property.Name == "Date")) {

                        string propertyName = property.Name;
                        string propertyValue = property.GetValue(model, null).ToString();

                        switch (propertyName) {

                            case "RedOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[0]++;
                                }
                                break;
                            case "PinkOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[1]++;
                                }
                                break;
                            case "PurpleOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[2]++;
                                }
                                break;
                            case "BlueOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[3]++;
                                }
                                break;
                            case "CyanOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[4]++;
                                }
                                break;
                            case "TealOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[5]++;
                                }
                                break;
                            case "GreenOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[6]++;
                                }
                                break;
                            case "YellowOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[7]++;
                                }
                                break;
                            case "OrangeOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[8]++;
                                }
                                break;
                            case "BrownOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[9]++;
                                }
                                break;
                            case "WhiteOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[10]++;
                                }
                                break;
                            case "GrayOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[11]++;
                                }
                                break;
                            case "BlackOccurance":
                                if (propertyValue == "True") {
                                    foregroundOccurences[12]++;
                                }
                                break;
                            case "BRedOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[0]++;
                                }
                                break;
                            case "BPinkOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[1]++;
                                }
                                break;
                            case "BPurpleOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[2]++;
                                }
                                break;
                            case "BBlueOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[3]++;
                                }
                                break;
                            case "BCyanOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[4]++;
                                }
                                break;
                            case "BTealOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[5]++;
                                }
                                break;
                            case "BGreenOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[6]++;
                                }
                                break;
                            case "BYellowOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[7]++;
                                }
                                break;
                            case "BOrangeOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[8]++;
                                }
                                break;
                            case "BBrownOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[9]++;
                                }
                                break;
                            case "BWhiteOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[10]++;
                                }
                                break;
                            case "BGrayOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[11]++;
                                }
                                break;
                            case "BBlackOccurance":
                                if (propertyValue == "True") {
                                    backgroundOccurences[12]++;
                                }
                                break;
                        }
                    }
                }

                DateTimeOffset date = model.Date;

                if (date < currentFirstDate) {

                     currentFirstDate = date;
                }
                if (date > currentLastDate) {

                    currentLastDate = date;
                }
            }

            ViewData["ForegroundOccurences"] = foregroundOccurences;
            ViewData["BackgroundOccurences"] = backgroundOccurences;

            totalConversions = allModels.Count().ToString();
            ViewData["TotalConverstions"] = totalConversions;

            string firstDate = currentFirstDate.ToString("dd/MM/yyyy, H:mm tt");
            string lastDate = currentLastDate.ToString("dd/MM/yyyy, H:mm tt");
            ViewData["FirstDate"] = firstDate;
            ViewData["LastDate"] = lastDate;


            return View();

        }
        #endregion Default Index IActionResult

        #region Error IActionResult [GET]

        [HttpGet]
        public IActionResult Error() {

            return View();
        }
        #endregion Error IActionResult


        /*METHODS*/

        #region Pull all DB records into List<UserColorDataModel>
        public List<UserColorDataModel> RetrieveRecords() {

            //stores the results
            List<UserColorDataModel> colorModels = new List<UserColorDataModel>();

            //pull the records into a list of sets
            using (var context = new ApplicationDbContext(config)) {

                var colorRecord = from record in context.UserColorTable
                                  select record;

                foreach (var model in colorRecord) {
                    colorModels.Add(model);
                }

                context.SaveChanges();
            }
            return colorModels;

        }
        #endregion Pull all new records and update their values (Updated = 1)


    }
}
