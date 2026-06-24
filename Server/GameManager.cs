using System;
using System.Runtime.InteropServices.Swift;
using System.Security.Authentication.ExtendedProtection;

namespace ConsoleRummy
{
    public class GameManager
    {
        private IGameState CurrentState;
        public List<Player> Players;
        public ICardDeck deck;
        public List<Card> DiscardPile;

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
            return "Nieznany gracz";
        }

        public void ShufflePlayers()
        {
            Random rng = new Random();
            Players = Players.OrderBy(player => rng.Next()).ToList();
            for(int i = 0; i < Players.Count; i++)
            {
                Players[i].Seat = i + 1;
            }
        }
    }
}