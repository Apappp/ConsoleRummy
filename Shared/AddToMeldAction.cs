using System;

namespace ConsoleRummy
{
    public class AddToMeldAction : IPlayerAction
    {
        private int Seat {get; set;}
        public AddToMeldAction(int seat)
        {
            Seat = seat;
        }
        public void ExecuteAction(GameManager table){}
        public int GetPlayerSeat() => Seat;
    }
}