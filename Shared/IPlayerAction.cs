using System;

namespace ConsoleRummy
{
    public interface IPlayerAction
    {
        void ExecuteAction(GameManager table);
        string GetPlayerId();
    }
}