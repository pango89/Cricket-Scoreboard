using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class Player
    {
        public Player(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; set; }
    }
}
