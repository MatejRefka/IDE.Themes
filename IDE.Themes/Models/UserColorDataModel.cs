using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Data Model representing the User Color Table. Stores all the theme color variables.
/// </summary>

namespace IDE.Themes.Models {

    public class UserColorDataModel {

        public UserColorDataModel() {

        }

        //PROPERTIES: TABLE COLUMNS
        [Key]
        [MaxLength(50)]
        public string ColorId { get; set; }

        public DateTimeOffset Date { get; set; }

        //foregrounds

        public bool RedOccurance { get; set; }

        public bool PinkOccurance { get; set; }

        public bool PurpleOccurance { get; set; }

        public bool BlueOccurance { get; set; }

        public bool CyanOccurance { get; set; }

        public bool TealOccurance { get; set; }

        public bool GreenOccurance { get; set; }

        public bool YellowOccurance { get; set; }

        public bool OrangeOccurance { get; set; }

        public bool BrownOccurance { get; set; }

        public bool WhiteOccurance { get; set; }

        public bool GrayOccurance { get; set; }

        public bool BlackOccurance { get; set; }

        //backgrounds

        public bool BRedOccurance { get; set; }

        public bool BPinkOccurance { get; set; }

        public bool BPurpleOccurance { get; set; }

        public bool BBlueOccurance { get; set; }

        public bool BCyanOccurance { get; set; }

        public bool BTealOccurance { get; set; }

        public bool BGreenOccurance { get; set; }

        public bool BYellowOccurance { get; set; }

        public bool BOrangeOccurance { get; set; }

        public bool BBrownOccurance { get; set; }

        public bool BWhiteOccurance { get; set; }

        public bool BGrayOccurance { get; set; }

        public bool BBlackOccurance { get; set; }

    }
}
