using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.Services {

    /// <summary>
    /// Description here
    /// </summary>

    public interface IThemeConverter {

        public void CreateTempFile(IFormFile file);

        public Task<string> ConvertEclipseToVisualAsync(IFormFile file);

    }
}
