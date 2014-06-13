using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    public enum Values { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
    public enum Suits { Spades, Clubs, Diamonds, Hearts };

    class Card
    {
        public Values Value;
        public Suits Suit;

        public Card(Values v, Suits s)
        {
            Value = v;
            Suit = s;
        }

        public override string ToString()
        {
            string s="";

            if ((int)Value < 9)
                s = ((int)Value + 2).ToString();
            else
                if ((int)Value == 9)
                    s = "Валет";
                else
                    if ((int)Value == 10)
                        s = "Дама";
                    else
                        if ((int)Value == 11)
                            s = "Король";
                        else
                            if ((int)Value == 12)
                                s = "Туз";
            switch ((int)Suit)
            {
                case 0:
                    s += " пики";
                    break;

                case 1:
                    s += " треф";
                    break;
                case 2:
                    s += " буби";
                    break;
                case 3:
                    s += " черви";
                    break;
            }

            return s;
        }

        public static int MaxCard(Card c1, Card c2)
        {
            return c1.Value - c2.Value;
        }

        public static int SortBySuit(Card c1, Card c2)
        {
            return c1.Suit - c2.Suit;
        }
    }
}
