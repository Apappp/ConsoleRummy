using System;
using System.ComponentModel;

namespace ConsoleRummy
{
    public interface ICardDeck
    {
        Card? Draw();
        List<Card> DrawMany(int count);
        void Refill(List<Card> recycledCards);
        void Shuffle();
        int GetCardsCount();
    }
}