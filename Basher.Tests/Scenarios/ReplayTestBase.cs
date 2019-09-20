using System;
using System.IO;

namespace Basher.Tests.Scenarios
{
    public abstract class ReplayTestBase
    {
        protected ReplayTestBase(string matchNumber)
        {
            this.MatchNumber = matchNumber;
            this.ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            this.FileName = Path.Combine($"{this.ProgramFiles}", @"Steam\steamapps\common\dota 2 beta\game\dota\replays", $"{this.MatchNumber}.dem");
        }

        public string ProgramFiles { get; set; }

        public string MatchNumber { get; }

        public string FileName { get; }

        public ulong ExpectedSummaryOffset { get; protected set; }
    }
}