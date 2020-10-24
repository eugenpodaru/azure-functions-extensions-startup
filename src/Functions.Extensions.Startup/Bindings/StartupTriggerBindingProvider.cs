namespace Devlight.Azure.Functions.Extensions.Startup.Bindings
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs.Host.Triggers;
    using Microsoft.Extensions.Options;

    internal sealed class StartupTriggerBindingProvider : ITriggerBindingProvider
    {
        private readonly IOptions<StartupTriggerOptions> _options;

        public StartupTriggerBindingProvider(IOptions<StartupTriggerOptions> options) => _options = options ?? throw new ArgumentNullException(nameof(options));

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<StartupTriggerAttribute>(inherit: false);

            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            if (parameter.ParameterType != typeof(StartupInfo))
            {
                throw new InvalidOperationException(string.Format("Can't bind StartupTriggerAttribute to type '{0}'.", parameter.ParameterType));
            }

            return Task.FromResult<ITriggerBinding>(new StartupTriggerBinding(parameter, _options));
        }
    }
}
