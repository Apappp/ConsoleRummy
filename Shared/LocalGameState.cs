using System;

namespace ConsoleRummy
{
    public class LocalGameState
    {
        public List<Card> MyHand { get; set; } = new List<Card>();
        public List<Meld> TableMelds { get; set; } = new List<Meld>();
        public Card? TopDiscardCard { get; set; }
        public string? CurrentTurnPlayerName { get; set; }
        public int Seat {get; set;}
        public int CardsLeftInDeck { get; set; }

    }
}