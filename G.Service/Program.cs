﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace G.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                MainService.Instance
            };
        ServiceBase.Run(ServicesToRun);
        }
    }
}
