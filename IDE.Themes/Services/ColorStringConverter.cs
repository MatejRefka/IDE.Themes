using System;
using System.Drawing;

/// <summary>
/// Helper class that converts color codings (hex, RGB and HSV) 
/// </summary>

namespace IDE.Themes.Services {


    public class ColorStringConverter {


        public ColorStringConverter() {


        }

        //Convert hex to RGB, returns RGB color
        public Color HexToRGB(string hex) {

            return ColorTranslator.FromHtml(hex);
        }

        //Convert RGB to HSV, returns h s v doubles
        public double[] RGBToHSV(Color colorRGB) {

            int max = Math.Max(colorRGB.R, Math.Max(colorRGB.G, colorRGB.B));
            int min = Math.Min(colorRGB.R, Math.Min(colorRGB.G, colorRGB.B));

            double[] hsv = new double[3];
            hsv[0] = colorRGB.GetHue();
            hsv[1] = (max == 0) ? 0 : 1d - (1d * min / max);
            hsv[2] = max / 255d;

            return hsv;
        }

        //Increase saturation to 100% to so that one of the RGB values is 0 (valid mapping condition)
        public double[] MaxSaturationHSV(double[] hsv) {

            double[] maxSatHSV = new double[3];
            maxSatHSV[0] = hsv[0];
            maxSatHSV[1] = 1d;
            maxSatHSV[2] = hsv[2];

            return maxSatHSV;
        }

        //Convert HSV back to RGB
        public Color HSVToRGB(double[] hsv) {

            int hi = Convert.ToInt32(Math.Floor(hsv[0] / 60)) % 6;
            double f = hsv[0] / 60 - Math.Floor(hsv[0] / 60);

            hsv[2] = hsv[2] * 255;
            int v = Convert.ToInt32(hsv[2]);
            int p = Convert.ToInt32(hsv[2] * (1 - hsv[1]));
            int q = Convert.ToInt32(hsv[2] * (1 - f * hsv[1]));
            int t = Convert.ToInt32(hsv[2] * (1 - (1 - f) * hsv[1]));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        //Convert epf RGB string to hex eg. 222,12,23 => #de0c17
        public string EpfRGBToHex(string epfRGB) {

            string[] rgb = epfRGB.Split(",");

            Color rgbColor = Color.FromArgb(Int32.Parse(rgb[0]), Int32.Parse(rgb[1]), Int32.Parse(rgb[2]));
            string hex = rgbColor.R.ToString("X2") + rgbColor.G.ToString("X2") + rgbColor.B.ToString("X2");
            hex = "#" + hex;

            return hex;
        }


    }
}
