using System;

namespace ConsoleRummy
{
    public class DiscardAction : IPlayerAction
    {
        public int Seat {get; set;}
        public int CardToDiscardIndex { get; set;}
        public DiscardAction(){}
        public DiscardAction(int seat, int cardToDiscardIndex)
        {
            Seat = seat;
            CardToDiscardIndex = cardToDiscardIndex;
        }
        public void ExecuteAction(GameManager table)
        {
            Player? player = table.Players.FirstOrDefault(p => p.Seat == Seat);
            if(player == null)
            {
                return;
            }
            table.DiscardPile.Add(player.Hand[CardToDiscardIndex]);
            player.Hand.RemoveAt(CardToDiscardIndex);
            table.NextTurn();
        }
    }
}