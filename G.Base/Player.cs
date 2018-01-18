using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public class Player : Entity, IMovable, IStrikable
    {
        public MoveStates MoveState { get; set; } = new MoveStates();
        public double Velocity { get; set; } = 0;
    }
}
