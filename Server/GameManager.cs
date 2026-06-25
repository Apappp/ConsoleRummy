using System;
using System.Runtime.InteropServices.Swift;
using System.Security.Authentication.ExtendedProtection;

namespace ConsoleRummy
{
    public class GameManager
    {
        private IGameState CurrentState;
        public List<Player> Players;
        public ICardDeck Deck;
        public List<Card> DiscardPile {get; private set;} = new List<Card>();
        public int CurrentPlayerSeat {get; set;} = 1;

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

        public LocalGameState GetStateForPlayer(Guid networkId)
        {
            Player player = Players.First(p => p.NetworkId == networkId.ToString());
            
            if (player == null) return null; 

            return new LocalGameState
            {
                Seat = player.Seat,
                CurrentTurnPlayerName = Players.First(p => p.Seat == CurrentPlayerSeat).Nickname,
                MyHand = player.Hand, 
                TopDiscardCard = DiscardPile.Count > 0 ? DiscardPile.Last() : null,
                CardsLeftInDeck = Deck.GetCardsCount() 
            };
        }
        
        public void NextTurn()
        {
            CurrentPlayerSeat++;
            CurrentPlayerSeat %= Players.Count;
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

        public void CreateNewDeck()
        {
            Deck = (Players.Count > 2) ? new Deck(2) : new Deck();
        }
    }
}