using System;

namespace ConsoleRummy
{
    public class SwapCardsAction : IPlayerAction
    {
        public int Seat {get; set;}
        public int Card1Index {get; set;}
        public int Card2Index {get; set;}
        public SwapCardsAction(){}
        public SwapCardsAction(int seat, int card1, int card2)
        {
            Seat = seat;
            Card1Index = card1;
            Card2Index = card2;
        }
        public void ExecuteAction(GameManager table)
        {
            Player? player = table.Players.FirstOrDefault(p => p.Seat == Seat);
            if(player == null)
                throw new GameLogicException("BŁĄD: Nie znaleziono gracza przy stole!");
            if(Card1Index < 0 || Card1Index >= player.Hand.Count || Card2Index < 0 || Card2Index >= player.Hand.Count)
                throw new GameLogicException("Podano index spoza zakresu!");
            
            Card tempCard = player.Hand[Card1Index];
            player.Hand[Card1Index] = player.Hand[Card2Index];
            player.Hand[Card2Index] = tempCard;
        }
    }
}