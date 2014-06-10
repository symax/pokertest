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
        
        static void Main(string[] args)
        {
            Console.WriteLine("В руке:\n");
            WhatSuit(445);
            WhatSuit(506);

            Console.ReadKey(true);
        }

        static void WhatSuit(int x)
        {
            var spades = new CvMat(path + "tempspades.png");
            var clubs = new CvMat(path + "tempclubs.png");
            var diamonds = new CvMat(path + "tempdiamonds.png");
            var hearts = new CvMat(path + "temphearts.png");
            var src = new IplImage(path + "screen1.png");

            int y = 470, rW = 21, rH = 43; //1 = 445, 2=506
            double minVal, maxValSpades, maxValClubs, maxValHearts, maxValDiamonds;
            CvPoint maxLoc, minLoc;

            Cv.SetImageROI(src, new CvRect(x, y, rW, rH));
            Cv.SaveImage("temp.png", src);
            Cv.ResetImageROI(src);
            var image = new CvMat(path + @"bin/Debug/temp.png");
            double maxVal;

            
            var template = hearts;
            var result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValHearts, out minLoc, out maxLoc);
            maxVal = maxValHearts;

            
            template = diamonds;
            result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValDiamonds, out minLoc, out maxLoc);
            if (maxValDiamonds > maxVal)
                maxVal = maxValDiamonds;

            template = spades;
            result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValSpades, out minLoc, out maxLoc);
            if (maxValSpades > maxVal)
                maxVal = maxValSpades;
            
            template = clubs;
            result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
            Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
            Cv.MinMaxLoc(result, out minVal, out maxValClubs, out minLoc, out maxLoc);
            if (maxValClubs > maxVal)
                maxVal = maxValClubs;
            //image.DrawRect(new CvRect(maxLoc, template.GetSize()), CvColor.Red, 1);

            if (maxVal == maxValHearts)
                Console.WriteLine(" черви\n");
            else
                if (maxVal == maxValDiamonds)
                    Console.WriteLine(" буби\n");
                else
                    if(maxVal == maxValClubs)
                        Console.WriteLine(" треф\n");
                    else
                        Console.WriteLine(" пики\n");

            //Console.WriteLine("{0} - {1}", maxLoc.X, maxLoc.Y);
            //Console.WriteLine("\n{0} - {1}", minLoc.X, minLoc.Y);
            /*Console.WriteLine("\nПики: \t" + maxValSpades);
            Console.WriteLine("\nТрефы: \t" + maxValClubs);
            Console.WriteLine("\nЧерви: \t" + maxValHearts);
            Console.WriteLine("\nБуби: \t" + maxValDiamonds);
            Cv.ShowImage("src", image);
            Cv.ShowImage("result", result);
            Cv.SaveImage("result.png", image);
            Cv.WaitKey();*/
        }

        static void WhatCard(int x)
        {

        }
    }
}
