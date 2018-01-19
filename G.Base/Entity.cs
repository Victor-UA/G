using RP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public abstract class Entity : IEntity
    {
        public Entity(double mass)
        {
            ID = Counter++;
            Position = new Position();
            Size = new Vector3();
            Mass = mass;
        }

        private static int Counter = 0;

        public int ID { get; }
        public Position Position { get; protected set; }
        public Vector3 Size { get; protected set; }

        protected double _mass;
        public virtual double Mass
        { get
            {
                return _mass;
            } protected set
            {
                if (value == 0)
                {
                    throw new ArgumentOutOfRangeException("The Mass can't be 0");
                }
                _mass = value;
            }
        }        

        public virtual void Dispose()
        {
            Position = null;
        }
    }
}
