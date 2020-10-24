namespace Devlight.Azure.Functions.Extensions.Startup.Bindings
{
    using Listeners;
    using Microsoft.Azure.WebJobs.Host.Bindings;
    using Microsoft.Azure.WebJobs.Host.Listeners;
    using Microsoft.Azure.WebJobs.Host.Protocols;
    using Microsoft.Azure.WebJobs.Host.Triggers;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    internal sealed class StartupTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly IOptions<StartupTriggerOptions> _options;

        public Type TriggerValueType => typeof(StartupInfo);

        public IReadOnlyDictionary<string, Type> BindingDataContract => new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public StartupTriggerBinding(ParameterInfo parameter, IOptions<StartupTriggerOptions> options)
        {
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var startupInfo = value as StartupInfo ?? StartupInfo.Create(_options.Value?.Title, _options.Value?.Version);

            var valueProvider = new ValueProvider(startupInfo);
            var bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            return Task.FromResult<ITriggerData>(new TriggerData(valueProvider, bindingData));
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            return context == null
                ? throw new ArgumentNullException(nameof(context))
                : Task.FromResult<IListener>(new StartupTriggerListener(context.Executor, _options));
        }

        public ParameterDescriptor ToParameterDescriptor() => new StartupTriggerParameterDescriptor
        {
            Name = _parameter.Name
        };

        private class ValueProvider : IValueProvider
        {
            private readonly object _value;

            public ValueProvider(object value) => _value = value;

            public Type Type => typeof(StartupInfo);

            public Task<object> GetValueAsync() => Task.FromResult(_value);

            public string ToInvokeString() => string.Empty;
        }

        private class StartupTriggerParameterDescriptor : TriggerParameterDescriptor
        {
            public override string GetTriggerReason(IDictionary<string, string> _) => "Trigger fired when host started";
        }
    }
}
