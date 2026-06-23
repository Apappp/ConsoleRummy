using System;
using System.Runtime.InteropServices.Swift;
using System.Security.Authentication.ExtendedProtection;

namespace ConsoleRummy
{
    public class GameManager
    {
        private IGameState CurrentState;
        public List<Player> Players;

        public GameManager()
        {
            CurrentState = new LobbyState();
            Players = new List<Player>();
        }

        public void ChangeState(IGameState newState)
        {
            CurrentState.ExitState(this);
            CurrentState = newState;
            CurrentState.EnterState(this);
        }

        public void HandleState(IPlayerAction action)
        {
            CurrentState.HandleState(this, action);
        }

        public string GetPlayerNameById(string id)
        {
            foreach (Player player in Players)
            {
                if(id == player.NetworkId)
                {
                    return player.Nickname;
                }
            }
            return "Unknown player";
        }
    }
}