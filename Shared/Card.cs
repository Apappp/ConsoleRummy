using System;
using System.Net.Security;

namespace ConsoleRummy
{
    public class Card
    {
        public string Suit {get; private set; }
        public string Rank {get; private set; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}