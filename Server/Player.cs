using System;

namespace ConsoleRummy
{
    public class Player
    {
        public string NetworkId {get; private set; }
        public string Nickname {get; private set;}

        public Player(string id, string name)
        {
            NetworkId = id;
            Nickname = name;
        }
    }
}