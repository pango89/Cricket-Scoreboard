using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cricket
{
    public class Over
    {
        public Over(int overNo)
        {
            Balls = new List<Ball>();
            OverNo = overNo;
        }

        public List<Ball> Balls { get; set; }
        public int OverNo { get; set; }

        public int GetTotalRuns() => this.Balls.Sum(x => x.Run + x.ExtraRun);
        public int GetTotalWickets() => this.Balls.Sum(x => x.BallType == BallType.Wicket ? 1 : 0);

        public void AddBall(BallType ballType, int runs, int extra)
        {
            Ball ball = new Ball(ballType, runs, extra, Balls.Count + 1);
            this.Balls.Add(ball);
        }

        public bool IsEndOfOver()
        {
            return this.CountCurrentBalls() == Constants.MaxBallsInOver;
        }

        public int CountCurrentBalls()
        {
            return this.Balls.Count(x => x.BallType == BallType.Regular || x.BallType == BallType.Wicket);
        }

        public int GetExtraRuns()
        {
            return this.Balls.Sum(x => x.ExtraRun);
        }

        public bool IsEmptyOver()
        {
            //Console.WriteLine(this.Balls.Count);
            return this.Balls.Count == 0;
        }

        public void DisplayOver()
        {
            for (int i = 0; i < this.Balls.Count; i++)
            {
                Console.Write("{0}{1} \t", this.Balls[i].Run + this.Balls[i].ExtraRun, GetSuffix(this.Balls[i].BallType));
            }
            Console.WriteLine();
        }

        public string GetSuffix(BallType type)
        {
            if (type == BallType.Wicket)
                return "W";

            if (type == BallType.Wide)
                return "WD";

            if (type == BallType.NoBall)
                return "NB";

            return "";
        }
    }
}
