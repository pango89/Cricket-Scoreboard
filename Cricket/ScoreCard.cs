using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cricket
{
    public class ScoreCard
    {
        public ScoreCard(int oversForInning, Team team1, Team team2)
        {
            this.Team1 = team1;
            this.Team2 = team2;

            BattingStats = new List<BatsmanStat>();
            this.CreateBattingOrder(team1);

            BowlingStats = new List<BowlerStat>();
            this.CreateBowlingOrder(team2);

            OversForInning = oversForInning;

            CurrentOverNumber = 0;
            Overs = new List<Over>();
            AddNewOver();
        }

        public void CreateBowlingOrder(Team team)
        {
            for (int i = 0; i < team.Players.Count; i++)
            {
                BowlerStat bowler = new BowlerStat(team.Players[i].PlayerName, 0, 0, 0, 0, 0, 0, 0, false);
                if (i == team.Players.Count - 1)
                    bowler.IsOnStrike = true;

                this.BowlingStats.Add(bowler);
            }
        }

        public void CreateBattingOrder(Team team)
        {
            for (int i = 0; i < team.Players.Count; i++)
            {
                BatsmanStat batsman = new BatsmanStat(team.Players[i].PlayerName, 0, 0, 0, 0, false, false, false);
                if (i == 0)
                {
                    batsman.IsOnStrike = true;
                    batsman.IsOnField = true;
                }
                if (i == 1)
                {
                    batsman.IsOnField = true;
                }

                this.BattingStats.Add(batsman);
            }
        }

        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        public List<BatsmanStat> BattingStats { get; set; }
        public List<BowlerStat> BowlingStats { get; set; }

        public List<Over> Overs { get; set; }
        public int OversForInning { get; set; }
        public int CurrentOverNumber { get; set; }

        public void DisplayBowlingScoreCard()
        {
            Console.WriteLine("Bowling Scorecard for Team : {0}", this.Team2.TeamName);
            Console.WriteLine("{0,-20}{1}{2,10}{3,10}{4,10}{5,15}", Constants.PlayerName, "Overs", "Runs", "Wickets", "Maiden", "Dots");

            for (int i = 0; i < BowlingStats.Count; i++)
            {
                BowlerStat bowler = BowlingStats[i];

                Console.WriteLine("{0}{1,-10}{2,10}{3,10}{4,10}{5,10}{6,15}", bowler.PlayerName, GetHighlighter(bowler), bowler.OversBalled + "."+ bowler.BallsInCurrentOverBalled, bowler.RunsConceded, bowler.WicketsTaken, bowler.MaidenOvers, bowler.DotBalls);
            }

            Console.WriteLine();
        }

        public void DisplayBattingScoreCard()
        {
            Console.WriteLine("Batting Scorecard for Team : {0}", this.Team1.TeamName);
            Console.WriteLine("{0,-20}{1}{2,10}{3,10}{4,10}{5,15}", Constants.PlayerName, Constants.Score, Constants.Fours, Constants.Sixes, Constants.Balls, "Status");

            for (int i = 0; i < BattingStats.Count; i++)
            {
                BatsmanStat batsman = BattingStats[i];
                Console.WriteLine("{0}{1,-10}{2,10}{3,10}{4,10}{5,10}{6,15}", batsman.PlayerName, GetHighlighter(batsman), batsman.Score, batsman.Count4, batsman.Count6, batsman.BallsFaced, batsman.IsOut ? "Out" : "Not Out");
            }

            // Log Ball By Ball
            Console.WriteLine();

        }

        public void DisplayBallOrder()
        {
            this.Overs[this.CurrentOverNumber - 1].DisplayOver();
            Console.WriteLine();
        }

        public void DisplayAggregate()
        {
            
            Console.WriteLine("Total: {0}/{1}", this.GetRunsScoredYet(), this.GetWicketsYet());


            int ball = this.Overs[this.CurrentOverNumber - 1].CountCurrentBalls();
            int over = this.CurrentOverNumber - 1;
            if (ball == Constants.MaxBallsInOver)
            {
                ball = 0;
                over = this.CurrentOverNumber;
            }

            Console.WriteLine("Overs: {0}.{1}", over, ball);
            Console.WriteLine("Extras: {0}", this.Overs.Sum(x => x.GetExtraRuns()));
        }

        public string GetHighlighter(BatsmanStat batsman)
        {
            if (!batsman.IsOut)
            {
                if (batsman.IsOnStrike)
                    return "**";
                if (batsman.IsOnField)
                    return "*";
            }

            return String.Empty;
        }

        public string GetHighlighter(BowlerStat bowler)
        {
            if (bowler.IsOnStrike)
                return "*";

            return String.Empty;
        }

        public void DisplayScoreCard()
        {
            DisplayBallOrder();
            DisplayBattingScoreCard();
            Console.WriteLine();
            DisplayBowlingScoreCard();
            Console.WriteLine();
            DisplayAggregate();
            Console.WriteLine();


            if (IsAllOut() || IsLastOverFinished())
            {
                Console.WriteLine("Inning Finished");
                Console.WriteLine();
            }
        }

        public void UpdateScore(BallType type, int run)
        {
            this.BattingStats.First(x => x.IsOnStrike).Update(run);
            if (run % 2 != 0 || IsCurrentOverFinished())
                RotateStrike();

            this.BowlingStats.First(x => x.IsOnStrike).Update(type, run);
            if (IsCurrentOverFinished())
                BowlerChange();
        }
        public void UpdateScore(BallType ballType)
        {
            if (ballType == BallType.Wicket)
            {
                BatsmanStat incomingBat = this.BattingStats.FirstOrDefault(x => !x.IsOnField && !x.IsOut);
                BatsmanStat outgoingBat = this.BattingStats.First(x => x.IsOnStrike);
                outgoingBat.Out();
                if (incomingBat != null)
                    incomingBat.In();

                this.BowlingStats.First(x => x.IsOnStrike).Update(BallType.Wicket, 0);
            }
            if (ballType == BallType.NoBall || ballType == BallType.Wide)
            {
                this.BowlingStats.First(x => x.IsOnStrike).Update(ballType, 1);
            }

            if (IsCurrentOverFinished())
            {
                RotateStrike();
                BowlerChange();
            }
        }

        public void RotateStrike()
        {
            BatsmanStat bat1 = this.BattingStats.First(x => x.IsOnStrike);
            BatsmanStat bat2 = this.BattingStats.First(x => !x.IsOnStrike && x.IsOnField);

            bat1.ToggleOnStrike();
            bat2.ToggleOnStrike();
        }

        public void BowlerChange()
        {
            BowlerStat bowl1 = this.BowlingStats.First(x => x.IsOnStrike);
            BowlerStat bowl2 = this.BowlingStats.Last(x => !x.IsOnStrike);

            bowl1.ToggleOnStrike();
            bowl2.ToggleOnStrike();
        }

        public void AddBallPlayed(int run)
        {
            if (this.IsCurrentOverFinished())
                this.AddNewOver();

            this.Overs[this.CurrentOverNumber - 1].AddBall(BallType.Regular, run, 0);
            this.UpdateScore(BallType.Regular, run);
        }

        public void AddBallPlayed(string type)
        {
            if (this.IsCurrentOverFinished())
                this.AddNewOver();
            switch (type)
            {
                case "W":
                    this.Overs[this.CurrentOverNumber - 1].AddBall(BallType.Wicket, 0, 0);
                    this.UpdateScore(BallType.Wicket);
                    break;
                case "Wd":
                    this.Overs[this.CurrentOverNumber - 1].AddBall(BallType.Wide, 0, 1);
                    this.UpdateScore(BallType.Wide);
                    break;
                case "Nb":
                    this.Overs[this.CurrentOverNumber - 1].AddBall(BallType.NoBall, 0, 1);
                    this.UpdateScore(BallType.NoBall);
                    break;
                default:
                    break;
            }
        }

        public bool IsCurrentOverFinished()
        {
            return this.Overs[this.CurrentOverNumber - 1].IsEndOfOver();
        }

        public void AddNewOver()
        {
            this.CurrentOverNumber++;
            Over newOver = new Over(this.CurrentOverNumber - 1);
            this.Overs.Add(newOver);
        }

        public bool IsLastOverFinished()
        {
            //Console.WriteLine("Current Over = {0} and Over Finsihed = {1}", this.CurrentOverNumber, this.IsCurrentOverFinished());

            return this.CurrentOverNumber == this.OversForInning && IsCurrentOverFinished();
        }

        public bool IsAllOut()
        {
            return this.BattingStats.Count(x => x.IsOut) == this.BattingStats.Count() - 1;
        }

        public int GetRunsScoredYet()
        {
            int sum = 0;
            for (int i = 0; i < this.CurrentOverNumber; i++)
                sum += this.Overs[i].GetTotalRuns();

            return sum;
        }
        public int GetWicketsYet()
        {
            return this.BattingStats.Count(x => x.IsOut);
        }

        public int GetRemainingWickets()
        {
            return this.BattingStats.Count(x => !x.IsOut) - 1;
        }
    }
}
