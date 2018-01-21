using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RP.Math;

namespace G.Base
{
    public class Player : Entity, IMovable, IStrikable
    {        
        public Player(double mass): base(mass)
        {
            Power.Linear.Forward = 50;
            Power.Linear.Backward = 30;
            Power.Linear.StrafeLeft = Power.Linear.StrafeRight = 40;
            Power.Angular.ZCounterClockWise = Power.Angular.ZClockWise = 4;

            AerodynamicResistanceCoefficientS = 100;
            FrictionResistanceCoefficientS = 0.5;
            BreakesAreOnWithoutMoving = false;

            Size = new Vector3(10, 10, 10);
        }
        public MoveStates MoveState { get; set; } = new MoveStates();        
        public Vector3 Velocity { get; set; } = new Vector3();
        public Powers Power { get; set; } = new Powers();

        public IEnvironment Environment { get; set; } = Air.Instance;
        public double AerodynamicResistanceCoefficientS { get; set; }
        public double FrictionResistanceCoefficientS { get; set; }
        public bool BreakesAreOnWithoutMoving { get; set; }

    }
}
