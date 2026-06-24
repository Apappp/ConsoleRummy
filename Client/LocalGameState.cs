using System;

namespace ConsoleRummy
{
    public class LocalGameState
    {
        public List<Card> MyHand { get; set; }
        public List<Meld> TableMelds { get; set; }
        public Card TopDiscardCard { get; set; }
        public string CurrentTurnPlayerId { get; set; }
        public int CardsLeftInDeck { get; set; }

        public LocalGameState()
        {
            MyHand = new List<Card>();
            TableMelds = new List<Meld>();
        }
    }
}