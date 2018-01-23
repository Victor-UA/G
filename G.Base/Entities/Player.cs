using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RP.Math;

namespace G.Base
{
    public class Player : MovableEntity, IStrikable
    {        
        public Player(double mass): base(mass)
        {
            Power.Linear.Forward = 50000;
            Power.Linear.Backward = 30000;
            Power.Linear.StrafeLeft = Power.Linear.StrafeRight = 40000;
            Power.Angular.ZCounterClockWise = Power.Angular.ZClockWise = 4;

            Environment = Air.Instance;
            AerodynamicResistanceCoefficientS = 1;
            Surface = Glass.Instance;
            FrictionCoefficientS = 0.5;
            BreakesAreOnWithoutMoving = false;

            Size = new Vector3(10, 10, 10);
        }

        public override void Start()
        {
            IsRun = true;
            DoMovesAsync(_cts.Token);
        }

        public override void Pause()
        {
            IsPaused = IsRun && true;
        }

        public override void Resume()
        {
            IsPaused = false;
        }
    }
}
