using System;

namespace ConsoleRummy
{
    public class ConsoleRenderer
    {
        public void DrawScreen(LocalGameState localGame)
        {
            Console.Clear();
            Console.WriteLine("--- STÓŁ ---");
            Console.WriteLine($"Karty w talii: {localGame.CardsLeftInDeck}");
            Console.WriteLine($"Na stosie odrzuconych leży: {localGame.TopDiscardCard?.Rank} {localGame.TopDiscardCard?.Suit}");
            
            Console.WriteLine("\n--- TWOJA RĘKA ---");
            for (int i = 0; i < localGame.MyHand.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {localGame.MyHand[i].Rank} {localGame.MyHand[i].Suit}");
            }
            Console.WriteLine("--------");
        }
    }
}