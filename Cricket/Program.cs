using System;
using System.Collections.Generic;
using System.Linq;

namespace Cricket
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] names1 = new string[]{ "P1", "P2", "P3", "P4", "P5" };
            string[] names2 = new string[]{ "P6", "P7", "P8", "P9", "P10" };

            List<Player> playersTeam1 = names1.Select(x => new Player(x)).ToList();
            List<Player> playersTeam2 = names2.Select(x => new Player(x)).ToList();

            Team team1 = new Team("A", playersTeam1);
            Team team2 = new Team("B", playersTeam2);
            Game g = new Game(team1, team2, 2);

            ///g.GamePlay();
            g.BallPlayed(1);
            g.BallPlayed(1);
            g.BallPlayed(1);
            g.BallPlayed(1);
            g.BallPlayed(1);
            g.BallPlayed(2);

            g.BallPlayed("W");
            g.BallPlayed(4);
            g.BallPlayed(4);
            g.BallPlayed("Wd");
            g.BallPlayed("W");
            g.BallPlayed(1);
            g.BallPlayed(6);

            g.BallPlayed(4);
            g.BallPlayed(6);
            g.BallPlayed("W");
            g.BallPlayed("W");
            g.BallPlayed(1);
            g.BallPlayed(1);

            g.BallPlayed(6);
            g.BallPlayed(1);
            g.BallPlayed("W");
            g.BallPlayed(6);
            ///g.GamePlay();
        }
    }
}
