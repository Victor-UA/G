using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    class Glass: ISurface
    {
        private Glass()
        {
            FrictionCoefficient = 50;
        }

        static Glass()
        {
            Instance = new Glass();
        }

        public static Glass Instance { get; }

        public double FrictionCoefficient { get; }
    }
}
