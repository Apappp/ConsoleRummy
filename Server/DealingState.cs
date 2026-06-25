using System;

namespace ConsoleRummy
{
    public class DealingState : IGameState
    {
        public void EnterState(GameManager table)
        {
            table.ShufflePlayers();
            table.CreateNewDeck();
            table.DealTheCards();
            table.ChangeState(new PlayerTurnState());
        }
        public void HandleState(GameManager table, IPlayerAction action){}
        public void ExitState(GameManager table){}
    }
}