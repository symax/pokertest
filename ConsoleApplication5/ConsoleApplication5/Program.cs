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
            WhatSuit(445, 470);
            WhatSuit(506, 470);

            Console.WriteLine("\nНа столе:\n");
            WhatSuit(348, 272);
            WhatSuit(413, 272);
            WhatSuit(478, 272);

            WhatSuit(543, 272);

            WhatSuit(608, 272);

            Console.ReadKey(true);
        }

        static void WhatSuit(int x, int y)
        {
            var src = new IplImage(path + "screenriver.png");

            List<CvMat> templates = new List<CvMat>(new CvMat[] { new CvMat(path + "tempspades.png"),
                                                                  new CvMat(path + "tempclubs.png"),
                                                                  new CvMat(path + "tempdiamonds.png"),
                                                                  new CvMat(path + "temphearts.png") });

            int rW = 21, rH = 43;
            Cv.SetImageROI(src, new CvRect(x, y, rW, rH));
            Cv.SaveImage("temp.png", src);
            Cv.ResetImageROI(src);

            var image = new CvMat(path + @"bin/Debug/temp.png");
            double maximum = 0, maxVal, minVal;
            CvPoint maxLoc, minLoc;
            int num = 0;
            CvMat template;
            IplImage result;
            for (int i = 0; i < 4; i++)
            {
                template = templates[i];
                result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
                Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
                Cv.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                if (maxVal > maximum)
                {
                    maximum = maxVal;
                    num = i;
                }
            }

            switch (num)
            {
                //black suit
                case 0:
                    WhatCard(true);
                    Console.WriteLine(" пики");
                    break;
                case 1:
                    WhatCard(true);
                    Console.WriteLine(" треф");
                    break;
                //red suit
                case 2:
                    WhatCard(false);
                    Console.WriteLine(" буби");
                    break;
                case 3:
                    WhatCard(false);
                    Console.WriteLine(" черви");
                    break;
            }
        }

        static void WhatCard(bool black)
        {
            List<CvMat> templates = new List<CvMat>();
            var image = new CvMat(path + @"bin/Debug/temp.png");
            double maximum = 0, maxVal, minVal;
            CvPoint maxLoc, minLoc;
            int num = 0;
            CvMat template;
            IplImage result;

            if (black)
            {
                templates.AddRange(new CvMat[] { new CvMat(path + "B2.png"),
                                                 new CvMat(path + "B3.png"),
                                                 new CvMat(path + "B4.png"),
                                                 new CvMat(path + "B5.png"),
                                                 new CvMat(path + "B6.png"),
                                                 new CvMat(path + "B7.png"),
                                                 new CvMat(path + "B8.png"),
                                                 new CvMat(path + "B9.png"),
                                                 new CvMat(path + "B10.png"),
                                                 new CvMat(path + "BJack.png"),
                                                 new CvMat(path + "BQueen.png"),
                                                 new CvMat(path + "BKing.png"),
                                                 new CvMat(path + "BAce.png"),});
            }
            else
            {
                templates.AddRange(new CvMat[] { new CvMat(path + "R2.png"),
                                                 new CvMat(path + "R3.png"),
                                                 new CvMat(path + "R4.png"),
                                                 new CvMat(path + "R5.png"),
                                                 new CvMat(path + "R6.png"),
                                                 new CvMat(path + "R7.png"),
                                                 new CvMat(path + "R8.png"),
                                                 new CvMat(path + "R9.png"),
                                                 new CvMat(path + "R10.png"),
                                                 new CvMat(path + "RJack.png"),
                                                 new CvMat(path + "RQueen.png"),
                                                 new CvMat(path + "RKing.png"),
                                                 new CvMat(path + "RAce.png"),});
            }

            for (int i = 0; i < 13; i++)
            {
                template = templates[i];
                result = new IplImage(new CvSize(image.Cols - template.Cols + 1, image.Rows - template.Rows + 1), BitDepth.F32, 1);
                Cv.MatchTemplate(image, template, result, MatchTemplateMethod.CCoeff);
                Cv.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
                if (maxVal > maximum)
                {
                    maximum = maxVal;
                    num = i;
                }
            }

            switch (num)
            {
                case 0:
                    Console.Write("2");
                    break;
                case 1:
                    Console.Write("3");
                    break;
                case 2:
                    Console.Write("4");
                    break;
                case 3:
                    Console.Write("5");
                    break;
                case 4:
                    Console.Write("6");
                    break;
                case 5:
                    Console.Write("7");
                    break;
                case 6:
                    Console.Write("8");
                    break;
                case 7:
                    Console.Write("9");
                    break;
                case 8:
                    Console.Write("10");
                    break;
                case 9:
                    Console.Write("Валет");
                    break;
                case 10:
                    Console.Write("Дама");
                    break;
                case 11:
                    Console.Write("Король");
                    break;
                case 12:
                    Console.Write("Туз");
                    break;
            }
        }
    }
}
