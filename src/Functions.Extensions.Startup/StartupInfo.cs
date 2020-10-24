namespace Devlight.Azure.Functions.Extensions.Startup
{
    using System;

    public sealed class StartupInfo
    {
        public string HostName { get; private set; }
        public string SiteName { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }

        internal StartupInfo()
        {
        }

        internal static StartupInfo Create(string name, string version) => new StartupInfo
        {
            Name = name,
            Version = version,
            HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME"),
            SiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")
        };
    }
}
