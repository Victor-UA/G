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
            lock (Lock)
            {
                ID = Counter++;
            }
            Position = new Position();
            Size = new Vector3();
            Mass = mass;
        }

        private static int Counter = 0;
        public static object Lock { get; } = new object();
        public static double MaxIterrationTime = 0;

        public int ID { get; }
        public Position Position { get; protected set; }
        public Vector3 Size { get; protected set; }

        protected double _mass;
        public virtual double Mass
        {
            get
            {
                return _mass;
            }
            protected set
            {
                if (value == 0)
                {
                    throw new ArgumentOutOfRangeException("The Mass can't be 0");
                }
                _mass = value;
            }
        }        
        

        public abstract void Start();

        public abstract void Pause();

        public abstract void Resume();

        public virtual void Dispose() { }
    }
}
