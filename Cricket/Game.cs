using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class Game
    {
        public Game(Team team1, Team team2, int overs)
        {
            Team1 = team1;
            Team2 = team2;
            Inning1 = new ScoreCard(overs, team1, team2);
            Inning2 = new ScoreCard(overs, team2, team1);
            IsTeam1BattingFirst = true;
        }

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public ScoreCard Inning1 { get; set; }
        public ScoreCard Inning2 { get; set; }

        public bool IsTeam1BattingFirst { get; set; } // Toss

        public bool IsFirstInningOver()
        {
            return this.Inning1.IsLastOverFinished() || this.Inning1.IsAllOut();
        }

        public bool IsSecondInningOver()
        {
            return this.Inning2.IsLastOverFinished() || this.Inning2.IsAllOut() || this.IsChasedSuccessfully();
        }

        public bool IsChasedSuccessfully()
        {
            int runsDiff = Inning1.GetRunsScoredYet() - Inning2.GetRunsScoredYet();
            return runsDiff < 0;
        }

        public void BallPlayed(ScoreCard inning, string ball)
        {
            inning.AddBallPlayed(ball);
            if (inning.IsLastOverFinished() || inning.IsAllOut())
                inning.DisplayScoreCard();
        }

        public void BallPlayed(string ball)
        {
            if (!IsFirstInningOver())
                this.BallPlayed(this.Inning1, ball);
            else
                this.BallPlayed(this.Inning2, ball);

            if (IsFirstInningOver() && IsSecondInningOver())
                this.DeclareResult();
        }

        public void BallPlayed(ScoreCard inning, int ball)
        {
            inning.AddBallPlayed(ball);
            if (inning.IsCurrentOverFinished() || inning.IsAllOut() || this.IsChasedSuccessfully())
                inning.DisplayScoreCard();
        }

        public void BallPlayed(int ball)
        {
            if (!IsFirstInningOver())
                this.BallPlayed(this.Inning1, ball);
            else
                this.BallPlayed(this.Inning2, ball);

            if (IsFirstInningOver() && IsSecondInningOver())
                this.DeclareResult();
        }

        public void DeclareResult()
        {
            int runsDiff = Inning1.GetRunsScoredYet() - Inning2.GetRunsScoredYet();

            if (runsDiff > 0)
            {
                Console.WriteLine("Result: Team {0} won the match by {1} runs.", Inning1.Team1.TeamName, runsDiff);
                return;
            }

            if (IsChasedSuccessfully())
            {
                int wicketsRem = Inning2.GetRemainingWickets();
                Console.WriteLine("Result: Team {0} won the match by {1} wickets.", Inning2.Team1.TeamName, wicketsRem);
                return;
            }

            Console.WriteLine("Result: Match Tied.");
        }
    }
}
