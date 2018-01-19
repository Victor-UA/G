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
            Power.Linear.Forward = 450;
            Power.Linear.Backward = 300;
            Power.Linear.StrafeLeft = Power.Linear.StrafeRight = 150;
            Power.Angular.ZCounterClockWise = Power.Angular.ZClockWise = 4;

            AerodynamicResistanceCoefficientS = 8000;

            Size = new Vector3(10, 10, 10);
        }
        public MoveStates MoveState { get; set; } = new MoveStates();        
        public Vector3 Velocity { get; set; } = new Vector3();
        public Powers Power { get; set; } = new Powers();

        public IEnvironment Environment { get; set; } = Air.Instance;
        public double AerodynamicResistanceCoefficientS { get; set; }        
    }
}
