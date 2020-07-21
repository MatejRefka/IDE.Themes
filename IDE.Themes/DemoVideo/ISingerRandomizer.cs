using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDE.Themes.DemoVideo {

    public interface ISingerRandomizer {

        public List<String> RandomizeSingers();

        public void PrintToConsole(List<String> singers);

    }
}
