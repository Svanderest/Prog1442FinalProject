using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1442_FinalProject.Utils
{
    public class Trick
    {
        public Card[] Cards;
        public int LeadPlayer;

        public Suit? LeadSuit
        {
            get
            {
                if (Cards[0] != null)
                    return Cards[0].Suit;
                else
                    return null;
            }
        }

        public int WinningPlayer
        {
            get
            {
                if (Cards.Count(c => c != null) == 4)                                    
                    return (Array.IndexOf(Cards, Cards.Where(c => c.Suit == LeadSuit).OrderByDescending(c => c.Value).First()) + LeadPlayer) % 4;                
                else
                    return -1;
            }
        }

        public int NextIndex
        {
            get { return Cards.Count(c => c != null); }
        }

        public Trick()
        {
            Cards = new Card[4];
        }
    }
}
