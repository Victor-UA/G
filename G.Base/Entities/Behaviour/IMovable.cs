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
        MoveStates MoveState { get; set; }
        Vector3 Velocity { get; }
        Powers Power { get; set; }
        IEnvironment Environment { get; set; }
        double AerodynamicResistanceCoefficientS { get; }
        ISurface Surface { get; set; }
        double FrictionCoefficientS { get; }
        bool BreakesAreOnWithoutMoving { get; set; }
        bool IsRun { get; }
        bool IsPaused { get; }        
    }
}
