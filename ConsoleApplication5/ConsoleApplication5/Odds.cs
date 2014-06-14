﻿using System;
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
                        odds = ((double)num / (double)(52 - cards.Length)) * 100.0;
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
            double odds = 100.0;

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

                        odds *= (double)num / (double)(52 - cards.Length);
                        Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
                        break;
                    }
            }
        }

        public static void StrPlus(params Card[] cards)
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
            double odds = 100.0;
            //Убираем повторяющиеся карты
            for (int i = 0; i < desk.Count - 1; i++)
            {
                if (desk[i].Value == desk[i + 1].Value)
                {
                    desk.Remove(desk[i]);
                    i--;
                }
            }

            bool Ace = false;
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i > 3; i--)
                {
                    if (desk[i].Value == Values.Ace)
                        Ace = true;
                    if (((desk[i].Value == desk[i - 1].Value + 1) && (desk[i - 1].Value == desk[i - 2].Value + 1) &&
                        (desk[i - 2].Value == desk[i - 3].Value + 1) && (desk[i - 3].Value == desk[i - 4].Value + 1)) ||
                        (Ace && (desk[i].Value == Values.Five) &&
                        (desk[i - 1].Value == Values.Four) && (desk[i - 2].Value == Values.Three) &&
                        (desk[i - 3].Value == Values.Two)))
                    {
                        foreach (Card c in deck)
                            if (c.Value == desk[i].Value + 1)
                            {
                                num++;
                                deck.Remove(c);
                            }
                        break;
                    }
                }
            }

            //в deck остались неизвестные карты, которые не подошли по условию в этой проверке
            int tmp = FlushDraw(ref deck, cards);
            num += tmp;
            odds *= (double)num / (double)(52 - cards.Length);
            Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
        }

        public static int FlushDraw(ref List<Card> deck, params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            desk.Sort(Card.SortBySuit);
            int num = 0;
            if (desk.Count > 3)
            {
                for (int i = desk.Count - 1; i < 2; i++)
                {
                    if (desk[i].Suit == desk[i - 1].Suit && desk[i - 1].Suit == desk[i - 2].Suit && desk[i - 2].Suit == desk[i - 3].Suit)
                    {
                        foreach (Card c in deck)
                            if (c.Suit == desk[i].Suit)
                            {
                                num++;
                                deck.Remove(c);
                            }
                        return num;
                    }
                }
            }
            return 0;
        }

        public static void TOAKPlus(params Card[] cards)
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
            double odds = 100.0;

            if (desk.Count > 2)
            {
                for (int i = desk.Count - 1; i > 1; i--)
                {
                    if (desk[i].Value == desk[i - 1].Value && desk[i - 1].Value == desk[i - 2].Value)
                    {
                        foreach(Card c in deck)
                            if (c.Value == desk[i].Value)
                            {
                                num++;
                                deck.Remove(c);
                            }
                        break;
                    }
                }
            }

            int tmp = FlushDraw(ref deck, cards);
            num += tmp;
            tmp = StraightDraw(ref deck, cards);
            num += tmp;
            odds *= (double)num / (double)(52 - cards.Length);
            Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
        }

        public static int StraightDraw(ref List<Card> deck, params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            //Убираем повторяющиеся карты
            for (int i = 0; i < desk.Count - 1; i++)
            {
                if (desk[i].Value == desk[i + 1].Value)
                {
                    desk.Remove(desk[i]);
                    i--;
                }
            }
            int num=0;

            //Ищем 4 подряд или 3+1 или 2+2 или 1+3
            if (desk.Count > 3)
            {
                bool Ace = false;
                for (int i = desk.Count - 1; i > 2; i--)
                {
                    if(desk[i].Value == Values.Ace)
                        Ace = true;
                    //4 подряд или 432A
                    if ((desk[i].Value == desk[i-1].Value + 1 && desk[i-1].Value == desk[i-2].Value + 1 && desk[i-2].Value == desk[i-3].Value + 1) ||
                        (desk[i].Value == Values.Four && desk[i-1].Value == Values.Three && desk[i-3].Value == Values.Two && Ace))
                    {
                        if (desk[i].Value != Values.Ace)    //если max карта не туз
                        {
                            foreach(Card c in deck)
                                if (c.Value == desk[i].Value + 1)
                                {
                                    num++;
                                    deck.Remove(c);
                                }
                        }
                        if (desk[i - 3].Value != Values.Two)   //если min карта не 2
                        {
                            foreach (Card c in deck)
                                if (c.Value == desk[i - 3].Value - 1)
                                {
                                    num++;
                                    deck.Remove(c);
                                }
                        }
                        else    
                            //если min карта это 2 - ищем тузы
                            if (desk[i - 3].Value == Values.Two && !Ace)
                            {
                                foreach (Card c in deck)
                                    if (c.Value == Values.Ace)
                                    {
                                        num++;
                                        deck.Remove(c);
                                    }
                            }
                    }
                    else
                        //3+1 || 543-A
                        if ((desk[i].Value == desk[i - 1].Value + 1 && desk[i - 1].Value == desk[i - 2].Value + 1 && desk[i - 2].Value == desk[i - 3].Value + 2) ||
                            (desk[i].Value == Values.Five && desk[i-1].Value == Values.Four && desk[i-2].Value == Values.Three && Ace))
                        {
                            foreach(Card c in deck)
                                if (c.Value == desk[i - 2].Value - 1)
                                {
                                    num++;
                                    deck.Remove(c);
                                }
                        }
                        else
                            //2+2 || 54-2A
                            if ((desk[i].Value == desk[i - 1].Value + 1 && desk[i - 1].Value == desk[i - 2].Value + 2 && desk[i - 2].Value == desk[i - 3].Value + 1) ||
                               (desk[i].Value == Values.Five && desk[i - 1].Value == Values.Four && desk[i - 2].Value == Values.Two && Ace))
                            {
                                foreach(Card c in deck)
                                    if (c.Value == desk[i - 1].Value - 1)
                                    {
                                        num++;
                                        deck.Remove(c);
                                    }
                            }
                            else
                                //1+3 || 5-32A
                                if ((desk[i].Value == desk[i - 1].Value + 2 && desk[i - 1].Value == desk[i - 2].Value + 1 && desk[i - 2].Value == desk[i - 3].Value + 1) ||
                                   (desk[i].Value == Values.Five && desk[i - 1].Value == Values.Three && desk[i - 2].Value == Values.Two && Ace))
                                {
                                    foreach (Card c in deck)
                                        if (c.Value == desk[i].Value - 1)
                                        {
                                            num++;
                                            deck.Remove(c);
                                        }
                                }
                }
            }
            return num;
        }

        public static void TPPlus(params Card[] cards)
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
            double odds = 100.0;

            if (desk.Count > 3)
            {
                int flag = 0;
                Values value1 = 0, value2 = 0;
                for (int i = desk.Count - 1; i > 0; i--)
                {
                    if (flag == 0)
                    {
                        if (desk[i].Value == desk[i - 1].Value)
                        {
                            value1 = desk[i].Value;
                            flag++;
                        }
                    }
                    else
                        if (flag == 1)
                        {
                            if (desk[i].Value == desk[i - 1].Value)
                            {
                                value2 = desk[i].Value;
                                flag++;
                            }
                        }
                    if (flag == 2)
                    {
                        foreach(Card c in deck)
                            if (c.Value == value1 || c.Value == value2)
                            {
                                num++;
                                deck.Remove(c);
                            }
                        break;
                    }
                }
            }

            int tmp = FlushDraw(ref deck, cards);
            num += tmp;
            tmp = StraightDraw(ref deck, cards);
            num += tmp;
            odds *= (double)num / (double)(52 - cards.Length);
            Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
        }

        public static void PairPlus(params Card[] cards)
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
            double odds = 100.0;

            for (int i = 0; i < desk.Count - 1; i++)
                if (desk[i].Value == desk[i + 1].Value)
                {
                    foreach(Card c in deck)
                        if (c.Value == desk[i].Value)
                        {
                            num++;
                            deck.Remove(c);
                        }
                    foreach(Card c in desk)
                        if(c.Value != desk[i].Value)
                            foreach(Card d in deck)
                                if (c.Value == d.Value)
                                {
                                    num++;
                                    deck.Remove(d);
                                }
                    break;
                }

            int tmp = FlushDraw(ref deck, cards);
            num += tmp;
            tmp = StraightDraw(ref deck, cards);
            num += tmp;
            odds *= (double)num / (double)(52 - cards.Length);
            Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
        }
    }
}
