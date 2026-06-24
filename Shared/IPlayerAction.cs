using System;
using System.Text.Json.Serialization;

namespace ConsoleRummy
{
    public interface IPlayerAction
    {
        void ExecuteAction(GameManager table);
        int GetPlayerSeat();
    }
}