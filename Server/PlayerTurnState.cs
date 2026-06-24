using System;

namespace ConsoleRummy
{
    public class PlayerTurnState : IGameState
    {
        public void EnterState(GameManager table){}
        public void HandleState(GameManager table, IPlayerAction action)
        {
            action.ExecuteAction(table);
        }
        public void ExitState(GameManager table){}
    }
}