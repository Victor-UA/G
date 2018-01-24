using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public class Air : Environment
    {
        private static readonly object _lock;
        private static Air _instance;
        static Air()
        {
            _lock = new object();
            _instance = null;
        }


        private Air()
        {
            Density = 1.276;
        }        

        public static Air Instance
        { get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = new Air();
                    }
                }
                return _instance;
            }
        }
    }
}
