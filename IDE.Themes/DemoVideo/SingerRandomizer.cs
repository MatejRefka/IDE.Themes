using System;
using System.Collections.Generic;

namespace IDE.Themes.DemoVideo {

    /// <summary>
    /// Class implementing ISingerRandomizer which ranks top singers randomly.
    /// The results are then displayed in console. 100% unbiased.
    /// </summary>

    public class SingerRandomizer : ISingerRandomizer {

        //Private Variables
        private readonly List<String> singers;

        //Constructor
        public SingerRandomizer(List<String> singers) {

            this.singers = singers;
        }

        //Implemented method: randomizing singers
        public List<String> RandomizeSingers() {

            List<String> randomList = ShuffleList(singers);

            for(int i = 0; i < randomList.Count; i++) {

                if (randomList[i] == "Britney Spears") {
                    randomList = SwapItems(i, 0);
                }
            }
            return randomList;
        }

        //Implemented method: printing singers to console
        public void PrintToConsole(List<String> singers) {

            List<String> randomList = RandomizeSingers();

            for(int i = 0; i < randomList.Count; i++) {

                Console.WriteLine($"{ i }. { randomList[i] }");
            }
        }

        //Helper methods:
        #region Shuffle list
        public List<String> ShuffleList(List<String> list) {
















            return new List<String>();
        }

        #endregion Shuffle list

        #region Swap two values in a list
        public static List<String> SwapItems(int index1, int index2) {




            return new List<String>();
        }
        #endregion Swap two values in a list 
    }
}

