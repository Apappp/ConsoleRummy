using System;

namespace ConsoleRummy
{
    public class ConsoleRenderer
    {
        public void DrawGameScreen(LocalGameState localGame, List<string> chatMessages, string playerName)
        {
            Console.Clear();
            DrawTableSection(localGame);
            DrawHandSection(localGame);
            DrawChatSection(chatMessages);
        }

        public void DrawLobbyScreen(List<string> chatMessages)
        {
            Console.Clear();
            Console.WriteLine("--- POCZEKALNIA (LOBBY) ---");
            Console.WriteLine("Oczekiwanie na start gry przez Hosta...\n");
            
            DrawChatSection(chatMessages);
        }

        private void DrawChatSection(List<string> chatMessages)
        {
            Console.WriteLine("--- CZAT ---");
            
            var lastMessages = chatMessages.TakeLast(10).ToList();
            int emptyLines = 10 - lastMessages.Count;
            
            for (int i = 0; i < emptyLines; i++)
            {
                Console.WriteLine();
            }

            foreach (var msg in lastMessages)
            {
                Console.WriteLine(msg);
            }
            Console.WriteLine("------");
            Console.Write(" > ");
        }

        private void DrawTableSection(LocalGameState localGame)
        {
            Console.WriteLine("--- STÓŁ ---");
            Console.WriteLine($"Ruch wykonuje: {localGame.CurrentTurnPlayerName}");
            Console.WriteLine($"Karty w talii: {localGame.CardsLeftInDeck}");
            
            if (localGame.TopDiscardCard != null)
            {
                Console.WriteLine($"Na stosie odrzuconych leży: {localGame.TopDiscardCard.Rank} {localGame.TopDiscardCard.Suit}");
            }
            else
            {
                Console.WriteLine("Na stosie odrzuconych leży: [Pusto]");
            }
        }

        private void DrawHandSection(LocalGameState localGame)
        {
            Console.WriteLine("\n--- TWOJA RĘKA ---");
            for (int i = 0; i < localGame.MyHand.Count; i++)
            {
                string cardString = $"[{i + 1}] {localGame.MyHand[i].Rank}{localGame.MyHand[i].Suit}";
                
                Console.Write(cardString.PadRight(12));

                if ((i + 1) % 7 == 0)
                {
                    Console.WriteLine();
                }
            }
            
            if (localGame.MyHand.Count > 0 && localGame.MyHand.Count % 7 != 0)
            {
                Console.WriteLine();
            }
            
        }
    }
}