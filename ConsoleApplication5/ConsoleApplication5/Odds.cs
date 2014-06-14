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

        public static void FlopOdds(params Card[] cards)
        {
            List<Card> deck = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    deck.Add(new Card((Values)j, (Suits)i));
                }
            }

            for (int i = 0; i < deck.Count; i++)
                for (int j = 0; j < cards.Length; j++)
                    if (deck[i].Suit == cards[j].Suit && deck[i].Value == cards[j].Value)
                    {
                        deck.RemoveAt(i);
                        i--;
                    }
            //Остались только неизвестные карты

            List<Card> result = new List<Card>(), temp = new List<Card>();
            result = CombDraw.FourOfAKindDraw(deck, cards);

        }
    }

    class CombDraw
    {
        public static List<Card> FourOfAKindDraw(List<Card> deck, params Card[] cards)
        {
            List<Card> result = new List<Card>();
            List<Card> knownCards = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                knownCards.Add(cards[i]);
            knownCards.Sort(Card.MaxCard);

            for(int i=knownCards.Count-1; i>1; i--)
                if (knownCards[i].Value == knownCards[i - 1].Value && knownCards[i - 1].Value == knownCards[i - 2].Value)
                {
                    foreach (Card c in deck)
                        if (c.Value == knownCards[i].Value)
                            result.Add(c);
                }

            return result;
        }

        public static List<Card> FullHouseDraw(List<Card> deck, params Card[] cards)
        {
            List<Card> result = new List<Card>();
            List<Card> knownCards = new List<Card>();
            for (int i = 0; i < cards.Length; i++)
                knownCards.Add(cards[i]);
            knownCards.Sort(Card.MaxCard);

            for (int i = knownCards.Count - 1; i > 1; i--)
                if (knownCards[i].Value == knownCards[i - 1].Value && knownCards[i - 1].Value == knownCards[i - 2].Value)
                {
                    foreach (Card c in deck)
                        if (c.Value == knownCards[i].Value)
                            result.Add(c);
                }

            return result;
        }
    }
}
