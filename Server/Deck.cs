using System;

namespace ConsoleRummy
{
    public class Deck : ICardDeck
    {
        private List<Card> cards = new List<Card>();

        public Deck(int numOfDecks = 1)
        {
            string[] suits = { "Kier", "Karo", "Trefl", "Pik" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Walet", "Dama", "Król", "As" };
            for(int i = 0; i < numOfDecks; i++){
                foreach (string suit in suits)
                {
                    foreach (string rank in ranks)
                    {
                        cards.Add(new Card(suit, rank));
                    }
                }
            }

            Shuffle();
        }

        public Card Draw()
        {
            if (cards.Count == 0) return null; 
        
            Card drawnCard = cards[0];
            cards.RemoveAt(0);
            return drawnCard;
        }

        public void Refill(List<Card> recycledCards)
        {
            cards.AddRange(recycledCards);
            Shuffle();
        }

        public void Shuffle(){}

        public int GetCardsCount()
        {
            return cards.Count;
        }

    }
}