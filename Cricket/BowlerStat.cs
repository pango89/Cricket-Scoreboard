using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class BowlerStat
    {
        public BowlerStat(string playerName, int oversBalled, int ballsInCurrentOverBalled, int runsConceded, int wicketsTaken, int maidenOvers, int dotBalls, int economy, bool isOnStrike)
        {
            PlayerName = playerName;
            OversBalled = oversBalled;
            BallsInCurrentOverBalled = ballsInCurrentOverBalled;
            RunsConceded = runsConceded;
            WicketsTaken = wicketsTaken;
            MaidenOvers = maidenOvers;
            DotBalls = dotBalls;
            Economy = economy;
            IsOnStrike = isOnStrike;
        }

        public string PlayerName { get; set; }
        public int OversBalled { get; set; }

        public int BallsInCurrentOverBalled { get; set; }
        public int RunsConceded { get; set; }
        public int WicketsTaken { get; set; }
        public int MaidenOvers { get; set; }
        public int DotBalls { get; set; }
        public int Economy { get; set; }
        public bool IsOnStrike { get; set; }

        public void Update(BallType type, int runs)
        {
            this.RunsConceded += runs;

            if (runs == 0)
                DotBalls++;

            if (type == BallType.Regular || type == BallType.Wicket)
                this.BallsInCurrentOverBalled++;

            if (type == BallType.Wicket)
                this.WicketsTaken++;

            this.UpdateOvers();
        }

        public void UpdateOvers()
        {
            if(this.BallsInCurrentOverBalled == Constants.MaxBallsInOver)
            {
                if (DotBalls == Constants.MaxBallsInOver)
                    this.MaidenOvers++;

                this.BallsInCurrentOverBalled = 0;
                this.OversBalled++;
            }
        }

        public void ToggleOnStrike()
        {
            this.IsOnStrike = !this.IsOnStrike;
        }
    }
}
