using System;

namespace ConsoleRummy
{
    public class PlayerTurnState : IGameState
    {
        public void EnterState(GameManager table){}
        public void HandleState(GameManager table, IPlayerAction action){}
        public void ExitState(GameManager table){}
    }
}