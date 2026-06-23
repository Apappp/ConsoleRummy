using System;

namespace ConsoleRummy
{
    public class RoundSummaryState : IGameState
    {
        public void EnterState(GameManager table){}
        public void HandleState(GameManager table, IPlayerAction action){}
        public void ExitState(GameManager table){}
    }
}