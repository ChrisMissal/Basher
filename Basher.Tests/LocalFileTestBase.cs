using System;
using System.IO;
using Basher.Tests.Scenarios;
using Shouldly;
using Xunit;

namespace Basher.Tests
{
    public abstract class LocalFileTestBase : IDisposable
    {
        private readonly ReplayTestBase replay;
        protected readonly string fileName;
        private readonly FileStream stream;

        protected LocalFileTestBase(ReplayTestBase replay)
        {
            this.replay = replay;
            this.fileName = replay.FileName;
            this.stream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);
        }

        [Fact]
        public void Can_read_expected_file_header()
        {
            var header = this.ExecuteReadHeader();

            header.Found.ShouldBeTrue();
            header.SummaryOffset.ShouldBe(this.replay.ExpectedSummaryOffset);
        }

        private ReplayElement.HeaderElement ExecuteReadHeader()
        {
            var headerVisitor = new HeaderReader(stream);

            var gameReplay = new GameReplayEngine(Console.Out);
            headerVisitor.Read(gameReplay);

            return gameReplay.Header;
        }

        public void Dispose()
        {
            this.stream?.Dispose();
        }
    }
}