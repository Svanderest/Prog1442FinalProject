using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1442_FinalProject.Utils
{
    public class Card
    {        
        public static Dictionary<string, Suit> Suits = new Dictionary<string, Suit>
        {
            { "clubs", Suit.Clubs },
            { "hearts", Suit.Hearts },
            { "diamonds", Suit.Diamonds },
            { "spades", Suit.Spades }
        };

        public static Dictionary<string, CardValue> Values = new Dictionary<string, CardValue>
        {
            { "two", CardValue.Two },
            { "three", CardValue.Three },
            { "four", CardValue.Four },
            { "five", CardValue.Five },
            { "six", CardValue.Six },
            { "seven", CardValue.Seven },
            { "eight", CardValue.Eight },
            { "nine", CardValue.Nine },
            { "ten", CardValue.Ten },
            { "jack", CardValue.Jack },
            { "queen", CardValue.Queen },
            { "king", CardValue.King },
            { "ace", CardValue.Ace }
        };

        public Suit Suit;
        public CardValue Value;

        public Card(Suit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public string ImagePath
        {
            get
            {
                string suit = Suits.Keys.FirstOrDefault(k => Suits[k] == Suit);
                string value = Values.Keys.FirstOrDefault(k => Values[k] == Value);
                return $"ms-appx:///Assets/cards/{suit}/{value}.png";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Card card)
                return card.Suit == this.Suit && card.Value == this.Value;
            else
                return false;
        }
    }
}
