using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public abstract class Environment : IEnvironment
    {
        public double Density { get; protected set; }
    }
}
