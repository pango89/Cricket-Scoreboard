using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket
{
    public class Ball
    {
        public Ball(BallType ballType, int run, int extraRun, int ballNo)
        {
            BallType = ballType;
            Run = run;
            ExtraRun = extraRun;
            BallNo = ballNo;
        }

        public BallType BallType { get; set; }
        public int Run { get; set; }
        public int ExtraRun { get; set; }
        public int BallNo { get; set; }
    }
}
