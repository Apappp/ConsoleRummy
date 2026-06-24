using System;

namespace ConsoleRummy
{
    public class Player
    {
        public string NetworkId {get; private set; }
        public string Nickname {get; private set; } 
        public int Seat {get; set; }
        public List<Card> Hand {get; set;}

        public Player(string networkId, string name)
        {
            NetworkId = networkId;
            Nickname = name;
        }
    }
}