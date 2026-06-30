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
                throw new GameLogicException("BŁĄD: Nie znaleziono gracza przy stole!");
            if(player.Seat != table.CurrentPlayerSeat)
                throw new GameLogicException("Poczekaj na swoją kolej..");
            if (!player.HasDrawn)
                throw new GameLogicException("Nie ciągnąłeś karty..");
            if(CardToDiscardIndex < 0 || CardToDiscardIndex >= player.Hand.Count)
                throw new GameLogicException("Nie posiadasz takiej karty!");

            table.DiscardPile.Add(player.Hand[CardToDiscardIndex]);
            player.Hand.RemoveAt(CardToDiscardIndex);
            player.HasDrawn = false;
            table.NextTurn();
        }
    }
}