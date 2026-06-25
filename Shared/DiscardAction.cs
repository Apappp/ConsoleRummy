using System;

namespace ConsoleRummy
{
    public class DiscardAction : IPlayerAction
    {
        private int Seat {get; set;}
        private int CardToDiscardIndex { get; set;}
        public DiscardAction(int seat, int cardToDiscardIndex)
        {
            Seat = seat;
            CardToDiscardIndex = cardToDiscardIndex;
        }
        public void ExecuteAction(GameManager table)
        {
            Player player = table.Players.First(p => p.Seat == Seat);
            table.DiscardPile.Add(player.Hand[CardToDiscardIndex]);
            player.Hand.RemoveAt(CardToDiscardIndex);
            table.NextTurn();
        }
        public int GetPlayerSeat() => Seat;
    }
}