using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog1442_FinalProject.Utils
{
    public class Round
    {
        public List<Card>[] Hands;
        public List<Card>[] CardsTaken;
        readonly int StartingPlayer;
        public int ActivePlayer;
        private Trick _trick;
        public Trick CurrentTrick
        {
            get { return _trick; }
            set
            {
                _trick = value;
                _trick.LeadPlayer = ActivePlayer;
            }
        }

        public Round()
        {
            Hands = new List<Card>[]
            {
                new List<Card>(),
                new List<Card>(),
                new List<Card>(),
                new List<Card>()
            };
            CardsTaken = new List<Card>[]
            {
                new List<Card>(),
                new List<Card>(),
                new List<Card>(),
                new List<Card>()
            };
            Random random = new Random();            
            foreach (int card in Enumerable.Range(0, 52).OrderBy(i => random.Next()))            
                Hands.OrderBy(h => h.Count).First().Add(new Card((Suit)(card % 4), (CardValue)(card % 13)));            
            StartingPlayer = Array.IndexOf(Hands, Hands.First(h => h.Any(c => c.Suit == Suit.Clubs && c.Value == CardValue.Two)));
            ActivePlayer = StartingPlayer;
            CurrentTrick = new Trick();
            CurrentTrick.LeadPlayer = ActivePlayer;
        }

        public void Play(int i)
        {
            Play(Hands[ActivePlayer][i % Hands[ActivePlayer].Count]);
        }

        public void Play(Card card)
        {
            if (!Hands[ActivePlayer].Contains(card))
                throw new ArgumentException("card", "The active player does not have this card");
            else if (ActivePlayer == CurrentTrick.LeadPlayer && !CardsTaken.Any(g => g.Count > 0) && (card.Suit != Suit.Clubs || card.Value != CardValue.Two))
                throw new ArgumentException("card", "You must lead with the two of clubs");
            else if (ActivePlayer == CurrentTrick.LeadPlayer && card.Suit == Suit.Hearts && !CardsTaken.Any(g => g.Any(c => c.Suit == Suit.Hearts)))
                throw new ArgumentException("card", "Hearts has not been broken yet");
            else if (ActivePlayer != CurrentTrick.LeadPlayer && CurrentTrick.LeadSuit != card.Suit && Hands[ActivePlayer].Any(c => c.Suit == CurrentTrick.LeadSuit))
                throw new ArgumentException("card", "You must follow suit");            
            CurrentTrick.Cards[CurrentTrick.NextIndex] = card;
            Hands[ActivePlayer].Remove(card);
            if (CurrentTrick.WinningPlayer == -1)
            {
                ActivePlayer++;
                ActivePlayer %= 4;
            }
            else
            {
                CardsTaken[CurrentTrick.WinningPlayer].AddRange(CurrentTrick.Cards);
                ActivePlayer = CurrentTrick.WinningPlayer;
            }
        }

        public int[] Scores
        {
            get
            {
                return CardsTaken.Select(g => g.Count(c => c.Suit == Suit.Hearts) + (g.Any(c => c.Suit == Suit.Spades && c.Value == CardValue.Queen) ? 13 : 0)).ToArray();
            }
        }
    }
}
