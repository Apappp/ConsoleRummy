using System;

namespace ConsoleRummy
{
    public class CreateMeldsAction : IPlayerAction
    {
        public int Seat {get; set;}
        public CreateMeldsAction(int seat)
        {
            Seat = seat;
        }
        public void ExecuteAction(GameManager table){}
    }
}