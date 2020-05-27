using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.Services {


    public class Factory {


        public static IThemeConverter CreateThemeConverter() {

            return new ThemeConverter();
        }







    }
}
