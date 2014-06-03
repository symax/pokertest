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
        private const string path = @"C:\Users\Максим\Documents\Visual Studio 2012\Projects\ConsoleApplication5\ConsoleApplication5\";
        static void Main(string[] args)
        {
            using (var image = new CvMat(path + "screen.png"))
            using (var template = new CvMat(path + "spades.png"))
            using (var result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1))
            {
                Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
                double minVal, maxVal;
                
                CvPoint maxLoc, minLoc;
                Cv.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                image.DrawRect(new CvRect(maxLoc, template.GetSize()), CvColor.Red, 1);
                
                Console.WriteLine("{0} - {1}", maxLoc.X, maxLoc.Y);
                Cv.ShowImage("src", image);
                Cv.ShowImage("result", result);
                Cv.SaveImage("result.png", image);
                Cv.WaitKey();
            }
           
        }
    }
}
