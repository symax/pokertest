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

        public static void FlushPlus(params Card[] cards)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 13; j++)
                    deck.Add(new Card((Values)j, (Suits)i));

            for (int i = 0; i < cards.Length; i++)
                foreach (Card c in deck)
                    if (cards[i].Value == c.Value && cards[i].Suit == c.Suit)
                        deck.Remove(c); // в deck остались только неизвестные карты

            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            desk.Sort(Card.SortBySuit);
            int num = 0;
            double odds;
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i < 3; i++)
                {
                    if (desk[i].Suit == desk[i - 1].Suit && desk[i - 1].Suit == desk[i - 2].Suit && desk[i - 2].Suit == desk[i - 3].Suit && desk[i - 3].Suit == desk[i - 4].Suit)
                    {
                        foreach (Card c in deck)
                            if (c.Suit == desk[i].Suit && c.Value > desk[i - 4].Value)
                                num++;
                        odds = ((double)num / (double)desk.Count) * 100.0;
                        Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
                        break;
                    }
                }
            }
        }

        public static void FHPlus(params Card[] cards)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 13; j++)
                    deck.Add(new Card((Values)j, (Suits)i));

            for (int i = 0; i < cards.Length; i++)
                foreach (Card c in deck)
                    if (cards[i].Value == c.Value && cards[i].Suit == c.Suit)
                        deck.Remove(c); // в deck остались только неизвестные карты

            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);

            int num = 0;
            double odds = 100;

            bool Set = false;
            Values setValue = 0, value2;
            for (int i = desk.Count - 1; i > 1; i--)
            {
                if (desk[i].Value == desk[i - 1].Value && desk[i - 1].Value == desk[i - 2].Value)
                {
                    Set = true;
                    setValue = desk[i].Value;
                    desk.RemoveAt(i);
                    desk.RemoveAt(i - 1);
                    desk.RemoveAt(i - 2);

                    num++;//4-я карта с таким же Value лежит в колоде

                    break;
                }
            }
            if (Set)
            {
                for (int i = desk.Count - 1; i > 0; i++)
                    if (desk[i].Value == desk[i - 1].Value)
                    {
                        value2 = desk[i].Value;

                        //ищем карты на столе, большие по Value, чем пара
                        foreach (Card c in desk)
                            if (c.Value > value2 && c.Value != setValue)
                                //ищем пару к ним
                                foreach (Card d in deck)
                                    if (d.Value == c.Value)
                                        num++;

                        //если сет меньше, чем пара по Value
                        if (setValue < value2)
                            //ищем 3-ю карту к паре
                            foreach (Card d in deck)
                                if (d.Value == value2)
                                    num++;

                        odds *= (double)num / (double)cards.Length;
                        Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
                        break;
                    }
            }
        }

        //
    }
}
