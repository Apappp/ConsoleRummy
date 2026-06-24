using System;

namespace ConsoleRummy
{
    public class CreateMeldsAction : IPlayerAction
    {
        private int Seat {get; set;}
        public CreateMeldsAction(int seat)
        {
            Seat = seat;
        }
        public void ExecuteAction(GameManager table){}
        public int GetPlayerSeat() => Seat;
    }
}