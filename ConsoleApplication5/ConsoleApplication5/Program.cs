using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.UserInterface;

namespace ConsoleApplication5
{
    class Program
    {
        private const string path = @"D:\Git\Test\ConsoleApplication5\ConsoleApplication5\";
        
        //private const CvMat spades = new CvMat(path + "spades.png");
        //private const CvMat spades = new CvMat(path + "spades.png");
        static void Main(string[] args)
        {
            var spades = new CvMat(path + "spades.png");
            var clubs = new CvMat(path + "clubs.png");
            var src = new IplImage(path + "screen.png");

            int x = 506, y = 470, rW = 21, rH = 43; //1 = 445, 2=506
            double minVal, maxValSpades, maxValClubs;
            CvPoint maxLoc, minLoc;

            Cv.SetImageROI(src, new CvRect(x, y, rW, rH));
            Cv.SaveImage("temp.png", src);
            Cv.ResetImageROI(src);
            var image = new CvMat(path + @"bin/Debug/temp.png");

            var template = spades;
            var result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValSpades, out minLoc, out maxLoc);
            //image.DrawRect(new CvRect(maxLoc, template.GetSize()), CvColor.Red, 1);

            template = clubs;
            result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValClubs, out minLoc, out maxLoc);


            Console.WriteLine("maxValSpades: " + maxValSpades + "\n" + "maxValClubs: " + maxValClubs);

            //Console.WriteLine("{0} - {1}", maxLoc.X, maxLoc.Y);
            //Console.WriteLine("\n{0} - {1}", minLoc.X, minLoc.Y);
            //Console.WriteLine("\n{0} - {1}", minVal, maxVal);
            Cv.ShowImage("src", image);
            Cv.ShowImage("result", result);
            Cv.SaveImage("result.png", image);
            Cv.WaitKey();

        }
    }
}
