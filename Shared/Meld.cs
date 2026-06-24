using System;
using System.IO.Compression;

namespace ConsoleRummy
{
    public class Meld
    {
        public List<Card> Cards {get; private set;}
        
        public Meld(List<Card> newMeld)
        {
            Cards = newMeld;
        }
        private bool CanAddCard(Card newCard)
        {
            return false;
        }

        public void AddCard(Card newCard){}


    }
}