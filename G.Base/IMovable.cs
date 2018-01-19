using RP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public interface IMovable
    {
        MoveStates MoveState { get; }
        Vector3 Velocity { get; set; }
        Powers Power { get; set; }
    }
}
