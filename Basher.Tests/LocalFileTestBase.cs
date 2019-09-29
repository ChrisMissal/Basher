using System;
using System.IO;
using System.Linq;
using Basher.Tests.Scenarios;
using Shouldly;
using Xunit;

namespace Basher.Tests
{
    public abstract class LocalFileTestBase : IDisposable
    {
        private readonly ReplayTestBase replay;
        private readonly string fileName;
        private readonly FileStream stream;
        private readonly ReplayFileEnumerator enumerator;

        protected LocalFileTestBase(ReplayTestBase replay)
        {
            this.replay = replay;
            this.fileName = replay.FileName;
            this.stream = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);
            this.enumerator = new ReplayFileEnumerator(stream);
        }

        [Fact]
        public void Can_read_expected_file_header()
        {
            var header = this.GetHeader();

            header.Found.ShouldBeTrue();
            header.ServerName.ShouldBe(this.replay.ServerName);
        }

        [Fact]
        public void Can_read_expected_MatchDate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Can_read_expected_GameMode()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Can_read_expected_Duration()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Can_read_expected_DireKills()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Can_read_expected_RadiantKills()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Can_read_expected_Victor()
        {
            throw new NotImplementedException();
        }

        private Header GetHeader()
        {
            var message = this.enumerator.First();

            if (message?.Inner is CDemoFileHeader header)
            {
                return new Header(header);
            }

            return null;
        }

        public void Dispose()
        {
            this.stream?.Dispose();
        }

        private class Header
        {
            private readonly CDemoFileHeader header;

            public Header(CDemoFileHeader header)
            {
                this.header = header;
            }

            public bool Found => this.header != null;

            public string ServerName => this.header.ServerName;
        }
    }
}