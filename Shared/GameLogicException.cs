using System;

namespace ConsoleRummy
{
    public class GameLogicException : Exception
    {
        public GameLogicException(string message) : base(message) { }
    }
}