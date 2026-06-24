using System;

namespace ConsoleRummy
{
    public interface ICardDeck
    {
        Card Draw();
        void Shuffle();
        int GetCardsCount();
    }
}