using System;

namespace ConsoleRummy
{
    public class DrawCardAction : IPlayerAction
    {
        public int Seat {get; set;}

        public DrawCardAction(){}

        public DrawCardAction(int seat)
        {
            Seat = seat;
        }
        public void ExecuteAction(GameManager table)
        {
            Player? player = table.Players.FirstOrDefault(p => p.Seat == Seat);
            if(player == null)
                throw new GameLogicException("BŁĄD: Nie znaleziono gracza przy stole!");
            if(player.Seat != table.CurrentPlayerSeat)
                throw new GameLogicException("Poczekaj na swoją kolej..");
            if(player.HasDrawn)
                throw new GameLogicException("BŁĄD: Ciągnąłeś już kartę");
            
            player.Hand.Add(table.Deck.Draw());
            player.HasDrawn = true;
            
        }
        public int GetPlayerSeat() => Seat;
    }
}