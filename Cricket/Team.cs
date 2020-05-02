using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class Team
    {
        public Team(string teamName, List<Player> players)
        {
            TeamName = teamName;
            Players = players;
        }

        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
    }
}
