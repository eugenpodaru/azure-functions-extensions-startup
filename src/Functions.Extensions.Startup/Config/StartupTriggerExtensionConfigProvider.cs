namespace Devlight.Azure.Functions.Extensions.Startup
{
    using Bindings;
    using Microsoft.Azure.WebJobs.Description;
    using Microsoft.Azure.WebJobs.Host.Config;
    using Microsoft.Extensions.Options;
    using System;

    [Extension("Startup Trigger")]
    public sealed class StartupTriggerExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IOptions<StartupTriggerOptions> _options;

        public StartupTriggerExtensionConfigProvider(IOptions<StartupTriggerOptions> options) => _options = options ?? throw new ArgumentNullException(nameof(options));

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.AddBindingRule<StartupTriggerAttribute>().BindToTrigger(new StartupTriggerBindingProvider(_options));
        }
    }
}
