using System;

namespace ConsoleRummy
{
    public class DrawCardAction : IPlayerAction
    {
        private int Seat {get; set;}

        public DrawCardAction(int seat)
        {
            Seat = seat;
        }
        public void ExecuteAction(GameManager table)
        {
            Player player = table.Players.First(p => p.Seat == Seat);
            player.Hand.Add(table.Deck.Draw());
        }
        public int GetPlayerSeat() => Seat;
    }
}