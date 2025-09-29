using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CryptoMonitor.Data
{
    public class LineChart
    {
        private const int SizeX = 60;
        private const int SizeY = 60;
        private const int BorderY = 1;

        public static MemoryStream GenerateImageIntoMemoryStream(List<decimal> data)
        {
            Bitmap bmp = new Bitmap(SizeX, SizeY);
            Graphics g = Graphics.FromImage(bmp);

            Pen penGreen = new Pen(Color.LightGreen);

            decimal maxValue = 0;
            decimal minValue = 999999;

            foreach (decimal d in data)
            {
                if (d > maxValue) maxValue = d;
                if (d < minValue) minValue = d;
            }

            decimal heightCoef = 1;
            if ((maxValue - minValue) > 0)
            {
                heightCoef = (SizeY - (2 * BorderY)) / (maxValue - minValue);
            }
            decimal prevValue = 0;
            int index = 0;

            foreach (decimal value in data)
            {
                index++;

                if (prevValue > 0) 
                {
                    int h1 = SizeY - BorderY - Convert.ToInt32((prevValue - minValue) * heightCoef);
                    int h2 = SizeY - BorderY - Convert.ToInt32((value - minValue) * heightCoef);

                    g.DrawLine(penGreen, new Point(index - 1, h1), new Point(index, h2));
                }

                prevValue = value;
                
            }

            MemoryStream stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Png);

            return stream;
        }
    }
}