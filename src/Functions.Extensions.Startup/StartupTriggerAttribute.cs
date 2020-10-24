namespace Devlight.Azure.Functions.Extensions.Startup
{
    using System;
    using Microsoft.Azure.WebJobs.Description;

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public sealed class StartupTriggerAttribute : Attribute
    {
    }
}
