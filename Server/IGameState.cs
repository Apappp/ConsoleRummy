using System;

namespace ConsoleRummy
{
    public interface IGameState
    {
        void EnterState(GameManager table);
        void HandleState(GameManager table, IPlayerAction action);
        void ExitState(GameManager table);
    }
}