using System;
using System.ComponentModel;

namespace ConsoleRummy
{
    public interface ICardDeck
    {
        Card? Draw();
        List<Card> DrawMany(int count);
        void Shuffle();
        int GetCardsCount();
    }
}