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
        public Player()
        {
            LinearPowers pLinear = Power.Linear;
            pLinear.Forward = 50;
            pLinear.Backward = 25;
            pLinear.StrafeLeft = pLinear.StrafeRight = 30;

            AngularPowers aPowers = Power.Angular;
            aPowers.ZCounterClockWise = aPowers.ZClockWise = 4;            

            Size = new Vector3(10, 10, 10);

        }
        public MoveStates MoveState { get; set; } = new MoveStates();        
        public Vector3 Velocity { get; set; } = new Vector3();
        public Powers Power { get; set; } = new Powers();        
    }
}
