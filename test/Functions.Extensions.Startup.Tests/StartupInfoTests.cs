namespace Devlight.Azure.Functions.Extensions.Startup.Tests
{
    using FluentAssertions;
    using Xunit;

    public class StartupInfoTests
    {
        [Fact]
        public void Create_ReturnsExpectedValue()
        {
            var name = "name";
            var startupInfo = StartupInfo.Create(name, null);
            startupInfo.Version.Should().BeNull();

            var version = "version";
            startupInfo = StartupInfo.Create(name, version);
            startupInfo.Version.Should().Be(version);
        }
    }
}
