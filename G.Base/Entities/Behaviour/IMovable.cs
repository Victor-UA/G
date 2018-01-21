﻿using RP.Math;
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
        IEnvironment Environment { get; set; }
        double AerodynamicResistanceCoefficientS { get; set; }
        double FrictionResistanceCoefficientS { get; set; }
        bool BreakesAreOnWithoutMoving { get; set; }
    }
}
