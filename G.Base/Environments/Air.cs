using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public class Air : Environment
    {
        private Air()
        {
            this.Density = 1.276;
        }

        static Air()
        {
            Instance = new Air();
        }

        public static Air Instance { get; }
    }
}
