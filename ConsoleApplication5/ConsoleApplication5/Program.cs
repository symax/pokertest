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
            //взять 1 скрин
            Console.WriteLine("В руке:\n");
            WhatSuit(445, 470);
            WhatSuit(506, 470);
            //выдать шансы улучшить руку

            //взять 2 скрин
            Console.WriteLine("\nНа столе:\n");
            WhatSuit(348, 272);
            WhatSuit(413, 272);
            WhatSuit(478, 272);
            //выдать шансы улучшить руку

            //взять 3 скрин
            WhatSuit(543, 272);
            //выдать шансы улучшить руку

            //взять 4 скрин
            WhatSuit(608, 272);
            //выдать итоговую комбинацию

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

            Card card;
            switch (num)
            {
                //black suit
                case 0:
                    card = new Card((Values)WhatCard(true), Suits.Spades);
                    Console.WriteLine(card.ToString());
                    break;
                case 1:
                    card = new Card((Values)WhatCard(true), Suits.Clubs);
                    Console.WriteLine(card.ToString());
                    break;
                //red suit
                case 2:
                    card = new Card((Values)WhatCard(false), Suits.Diamonds);
                    Console.WriteLine(card.ToString());
                    break;
                case 3:
                    card = new Card((Values)WhatCard(false), Suits.Hearts);
                    Console.WriteLine(card.ToString());
                    break;
            }
        }

        static int WhatCard(bool black)
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
            return num;
        }
    }
}
