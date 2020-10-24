Azure Functions Startup Trigger
===

|Branch|Status|
|---|---|
|master|[![Build status](https://dev.azure.com/devlight/azure-functions-extensions-startup/_apis/build/status/eugenpodaru.azure-functions-extensions-startup?branchName=master)|


This repo contains the startup binding extension for the **Azure WebJobs SDK**. This repo is available as the **Devlight.Azure.Functions.Extensions.Startup** [nuget package](http://www.nuget.org/packages/Devlight.Azure.Functions.Extensions.Startup).

### StartupTrigger

A simple startup trigger for running code when the runtime starts. You might need it if you have some heavier initialization to run on startup. 
The runtime starts when the function app wakes up after going idle due to inactivity, when the function app restarts due to function changes, and when the function app scales out.

Here is how to use it:

```csharp
public static void StartupJob([StartupTrigger] StartupInfo startup)
{
    Console.WriteLine("The function app has just started, woke up or scaled out!");
}
```
StartupTrigger uses the [Singleton](https://github.com/Azure/azure-webjobs-sdk/wiki/Singleton) feature of the WebJobs SDK to ensure that only a single instance of the triggered function runs.