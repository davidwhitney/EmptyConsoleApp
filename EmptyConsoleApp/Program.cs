using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceProcess;
using SimpleServices;
using ServiceInstaller = SimpleServices.ServiceInstaller;

namespace EmptyConsoleApp
{
    public class OldProgram
    {
        public static void Main_Original(string[] args)
        {
            Console.WriteLine("Hello world!");
            Console.ReadLine();
            Console.WriteLine("Goodbye world!");
        }
    }


    [RunInstaller(true)]
    public class Program : ServiceInstaller
    {
        private static void Main(string[] args)
        {
            new Service(args,
                        new List<IWindowsService>
                            {
                                new MyService(),
                            }.ToArray,
                        installationSettings: (serviceInstaller, serviceProcessInstaller) =>
                            {
                                serviceInstaller.ServiceName = "SimpleServices.ExampleApplication";
                                serviceInstaller.StartType = ServiceStartMode.Manual;
                                serviceProcessInstaller.Account = ServiceAccount.LocalService;
                            },
                        configureContext: x => { x.Log = Console.WriteLine; })
                .Host();
        }

        private class MyService : IWindowsService
        {
            public ApplicationContext AppContext { get; set; }

            public void Start(string[] args)
            {
                AppContext.Log("Hello world!");
            }

            public void Stop()
            {
                AppContext.Log("Goodbye world!");
            }

        }
    }
}
