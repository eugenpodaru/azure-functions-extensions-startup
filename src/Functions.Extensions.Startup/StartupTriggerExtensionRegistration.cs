using Devlight.Azure.Functions.Extensions.Startup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(StartupTriggerExtensionRegistration), "Startup Trigger")]

namespace Devlight.Azure.Functions.Extensions.Startup
{
    public sealed class StartupTriggerExtensionRegistration : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder) => builder.AddStartupTrigger();
    }
}
