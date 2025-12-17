

using Topshelf;
using TopShelfService;

HostFactory.Run(x =>
{
    x.Service<CustomService>();
    x.SetServiceName("TopShelfService");
    x.SetDisplayName("ServiceTopSHelf");

        x.EnableServiceRecovery(r =>
    {
        r.RestartService(1)// Restart the service after 1 minute
        .RestartService(5)
        .TakeNoAction();
        
        r.SetResetPeriod(1); // Reset the failure count after 1 day
    });

    x.DependsOn("MicrosoftService");

    x.RunAsLocalSystem();
    x.StartAutomaticallyDelayed();
});