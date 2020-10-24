namespace Devlight.Azure.Functions.Extensions.Startup
{
    using Microsoft.Azure.WebJobs;
    using System;

    public static class StartupTriggerWebJobsBuilderExtensions
    {
        public static IWebJobsBuilder AddStartupTrigger(this IWebJobsBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.AddExtension<StartupTriggerExtensionConfigProvider>();

            return builder;
        }
    }
}
