using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Odds
    {
        public static void PreFlopOdds(Card h1, Card h2)
        {
            double odds = 100;
            if (h1.Value != h2.Value)
            {
                odds *= 6.0 / 50;
            }
            else
                odds *= 2.0 / 50;
            Console.WriteLine("Ваши шансы улучшить руку = " + odds.ToString(".00") + "%");
        }

        public static void FlopOdds(Card h1, Card h2, Card d1, Card d2, Card d3)
        {

        }
    }
}
