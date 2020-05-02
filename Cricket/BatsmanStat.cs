using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class BatsmanStat
    {
        public BatsmanStat(string playerName, int score, int count4, int count6, int ballsFaced, bool isOut, bool isOnStrike, bool isOnField)
        {
            PlayerName = playerName;
            Score = score;
            Count4 = count4;
            Count6 = count6;
            BallsFaced = ballsFaced;
            IsOut = isOut;
            IsOnStrike = isOnStrike;
            IsOnField = isOnField;
        }

        public string PlayerName { get; set; }
        public int Score { get; set; }
        public int Count4 { get; set; }
        public int Count6 { get; set; }
        public int BallsFaced { get; set; }
        public bool IsOut { get; set; }
        public bool IsOnStrike { get; set; }
        public bool IsOnField { get; set; }

        public void Update(int runs)
        {
            this.Score += runs;
            this.Count4 += runs == 4 ? 1 : 0;
            this.Count6 += runs == 6 ? 1 : 0;
            this.BallsFaced++;
        }

        public void Out()
        {
            this.BallsFaced++; // May not be true for Run Out
            this.IsOut = true;
            this.IsOnField = false;
            this.IsOnStrike = false;
        }

        public void ToggleOnStrike()
        {
            this.IsOnStrike = !this.IsOnStrike;
            //Console.WriteLine("Batsman {0} changed strike from {1} to {2}", PlayerName, !this.IsOnStrike, this.IsOnStrike);
        }

        public void In()
        {
            this.IsOnField = true;
            this.IsOnStrike = true;
        }
    }
}
