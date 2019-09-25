using System;

namespace Basher.Tests.Scenarios
{
    public class ReplayTest4984038549 : ReplayTestBase
    {
        public ReplayTest4984038549() : base("4984038549")
        {
            this.ExpectedSummaryOffset = 101971604uL;
            this.Victor = "Dire";
            this.RadiantKills = 10;
            this.DireKills = 25;
            this.Duration = new TimeSpan(0, 39, 58);
            this.GameMode = "Captains Mode";
            this.MatchDate = new DateTimeOffset(2019, 8, 24, 4, 17, 0, TimeSpan.Zero);
        }
    }
}