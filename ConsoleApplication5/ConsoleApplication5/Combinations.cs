using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication5
{
    class Combinations
    {
        public virtual int IsThatCombination(params Card[] cards)
        {
            return 0;
        }

        public List<Combinations> CreateListCombinations()
        {
            List<Combinations> combinations = new List<Combinations>();
            combinations.Add(new RoyalFlush());
            combinations.Add(new StraightFlush());
            combinations.Add(new FourOfAKind());
            combinations.Add(new FullHouse());
            combinations.Add(new Flush());
            combinations.Add(new Straight());
            combinations.Add(new ThreeOfAKind());
            combinations.Add(new TwoPairs());
            combinations.Add(new Pair());
            combinations.Add(new HighCard());
            return combinations;
        }

        public static void WhatCombination(params Card[] cards)
        {
            List<Combinations> combs = new Combinations().CreateListCombinations();
            int num = 0;
            foreach (Combinations c in combs)
                if ((num = c.IsThatCombination(cards)) != 0)
                    break;
            if (cards.Length > 2)
                switch (num) //9-RF, 8-SF, 7-FOAK, 6-FH, 5-Fl, 4-Str, 3-TOAK, 2-2P, 1-P, 0-HK
                {
                    case 9:
                        Console.WriteLine("Шансы улучшить руку = 0%");
                        break;
                    case 8:
                        Console.WriteLine("Шансы улучшить руку = 0%");
                        break;
                    case 7:
                        Console.WriteLine("Шансы улучшить руку = 0%");
                        break;
                    case 6:
                        Odds.FHPlus(cards);
                        break;
                    case 5:
                        Odds.FlushPlus(cards);
                        break;
                    case 4:
                        Odds.StrPlus(cards);
                        break;
                    case 3:
                        Odds.TOAKPlus(cards);
                        break;
                    case 2:
                        Odds.TPPlus(cards);
                        break;
                    case 1:
                        Odds.PairPlus(cards);
                        break;

                    default:
                        List<Card> deck = new List<Card>();
                        for (int i = 0; i < 4; i++)
                            for (int j = 0; j < 13; j++)
                                deck.Add(new Card((Values)j, (Suits)i));

                        for (int i = 0; i < cards.Length; i++)
                            for(int j=0; j<deck.Count;j++)
                                if (cards[i].Value == deck[j].Value && cards[i].Suit == deck[j].Suit)
                                {
                                    deck.RemoveAt(j); // в deck остались только неизвестные карты
                                    j--;
                                }

                        int kol = 0;
                        if (cards[0].Value == cards[1].Value)
                            kol = 2;
                        else
                            kol = 6;
                        double odds = 100.0;
                        int tmp = Odds.FlushDraw(ref deck, cards);
                        kol += tmp;
                        tmp = Odds.StraightDraw(ref deck, cards);
                        kol += tmp;
                        odds *= (double)kol / (double)(52 - cards.Length);
                        Console.WriteLine("Шансы улучшить руку = " + odds.ToString(".00") + '%');
                        break;
                }
            else
                Odds.PreFlopOdds(cards[0], cards[1]);

        }
    }

    class Pair : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            for (int i = 0; i < desk.Count - 1; i++)
                if (desk[i].Value == desk[i + 1].Value)
                {
                    Console.WriteLine("Пара " + desk[i].Value.ToString());
                    return 1;
                }

            return 0;
        }
    }

    class TwoPairs : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);

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
                        Console.WriteLine("Две пары " + value1.ToString() + " и " + value2.ToString());
                        return 2;
                    }
                }
            }

            return 0;
        }
    }

    class ThreeOfAKind : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            if (desk.Count > 2)
            {
                for (int i = desk.Count - 1; i > 1; i--)
                {
                    if (desk[i].Value == desk[i - 1].Value && desk[i - 1].Value == desk[i - 2].Value)
                    {
                        Console.WriteLine("Сет " + desk[i].Value.ToString());
                        return 3;
                    }
                }
            }

            return 0;
        }
    }

    class Straight : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
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

            //Ищем 5 подряд
            bool Ace = false;
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i > 3; i--)
                {
                    if (desk[i].Value == Values.Ace)
                        Ace = true;
                    if ((desk[i].Value == desk[i - 1].Value + 1) && (desk[i - 1].Value == desk[i - 2].Value + 1) &&
                        (desk[i - 2].Value == desk[i - 3].Value + 1) && (desk[i - 3].Value == desk[i - 4].Value + 1))
                    {
                        Console.WriteLine("Стрит до " + desk[i].Value.ToString());
                        return 4;
                    }
                    else
                        if (Ace && (desk[i - 1].Value == Values.Five) &&
                        (desk[i - 2].Value == Values.Four) && (desk[i - 3].Value == Values.Three) &&
                        (desk[i - 4].Value == Values.Two))
                        {
                            Console.WriteLine("Стрит до " + desk[i-1].Value.ToString());
                            return 4;
                        }
                }
            }

            return 0;
        }
    }

    class Flush : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            desk.Sort(Card.SortBySuit);
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i > 3; i--)
                {
                    if (desk[i].Suit == desk[i - 1].Suit && desk[i - 1].Suit == desk[i - 2].Suit && desk[i - 2].Suit == desk[i - 3].Suit && desk[i - 3].Suit == desk[i - 4].Suit)
                    {
                        Console.WriteLine("Флеш " + desk[i].Suit.ToString() + " до " + desk[i].Value.ToString());
                        return 5;
                    }
                }
            }

            return 0;
        }
    }

    class FullHouse : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            bool Set = false;
            Values value1=0, value2;
            for (int i = desk.Count - 1; i > 1; i--)
            {
                if (desk[i].Value == desk[i - 1].Value && desk[i - 1].Value == desk[i - 2].Value)
                {
                    Set = true;
                    value1 = desk[i].Value;
                    desk.RemoveAt(i);
                    desk.RemoveAt(i - 1);
                    desk.RemoveAt(i - 2);
                    break;
                }
            }
            if (Set)
            {
                for (int i = desk.Count - 1; i > 0; i--)
                    if (desk[i].Value == desk[i - 1].Value)
                    {
                        value2 = desk[i].Value;
                        Console.WriteLine("Фул Хаус на " + value1.ToString() + " и " + value2.ToString());
                        return 6;
                    }
            }

            return 0;
        }
    }

    class FourOfAKind : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            for (int i = desk.Count - 1; i > 2; i--)
            {
                if (desk[i].Value == desk[i - 1].Value && desk[i - 1].Value == desk[i - 2].Value && desk[i-2].Value == desk[i-3].Value)
                {
                    Console.WriteLine("Каре " + desk[i].Value.ToString());
                    return 7;
                }
            }

            return 0;
        }
    }

    class StraightFlush : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
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

            //Ищем 5 подряд
            bool Ace = false;
            Suits suitAce=0;
            desk.Sort(Card.SortBySuit);
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i > 3; i--)
                {
                    if (desk[i].Value == Values.Ace)
                    {
                        Ace = true;
                        suitAce = desk[i].Suit;
                    }
                    if ((desk[i].Value == desk[i - 1].Value + 1) && (desk[i - 1].Value == desk[i - 2].Value + 1) &&
                        (desk[i - 2].Value == desk[i - 3].Value + 1) && (desk[i - 3].Value == desk[i - 4].Value + 1) &&
                        desk[i].Suit == desk[i-1].Suit && desk[i-1].Suit == desk[i-2].Suit && desk[i-2].Suit == desk[i-3].Suit &&
                        desk[i-3].Suit == desk[i-4].Suit)
                    {
                        Console.WriteLine("Стрит Флеш до " + desk[i].Value.ToString());
                        return 8;
                    }
                    else
                        if (Ace && (desk[i-1].Value == Values.Five) &&
                        (desk[i - 2].Value == Values.Four) && (desk[i - 3].Value == Values.Three) &&
                        (desk[i - 4].Value == Values.Two) && desk[i-1].Suit == desk[i - 2].Suit && desk[i - 2].Suit == desk[i - 3].Suit &&
                        desk[i - 3].Suit == desk[i - 4].Suit && desk[i - 4].Suit == suitAce)
                        {
                            Console.WriteLine("Стрит Флеш до " + desk[i-1].Value.ToString());
                            return 8;
                        }
                }
            }

            return 0;
        }
    }

    class RoyalFlush : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
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

            //Ищем 5 подряд
            desk.Sort(Card.SortBySuit);
            if (desk.Count > 4)
            {
                for (int i = desk.Count - 1; i > 3; i--)
                {
                    if ((desk[i].Value == Values.Ace) && (desk[i - 1].Value == Values.King) && (desk[i - 2].Value == Values.Queen) &&
                        (desk[i - 3].Value == Values.Jack) && (desk[i - 4].Value == Values.Ten) &&
                        desk[i].Suit == desk[i - 1].Suit && desk[i - 1].Suit == desk[i - 2].Suit && desk[i - 2].Suit == desk[i - 3].Suit &&
                        desk[i - 3].Suit == desk[i - 4].Suit)
                    {
                        Console.WriteLine("Роял Флеш на " + desk[i].Suit.ToString());
                        return 9;
                    }
                }
            }

            return 0;
        }
    }

    class HighCard : Combinations
    {
        public override int IsThatCombination(params Card[] cards)
        {
            List<Card> desk = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                desk.Add(cards[i]);
            desk.Sort(Card.MaxCard);
            Console.WriteLine("Старшая карта " + desk[desk.Count - 1].Value.ToString());
            return 0;
        }
    }
}
