using Microsoft;
using System.ServiceProcess;


var service = new CustomService();
if (Environment.UserInteractive)
{
    service.Start();
    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();
    service.Stop();

}
else
    ServiceBase.Run(service);