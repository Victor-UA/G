using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G.Base
{
    public class Entity: IDisposable
    {
        public Entity()
        {
            ID = Counter++;
            Position = new Position();            
        }

        private static int Counter = 0;

        public int ID { get; }
        public Position Position { get; private set; }                

        public virtual void Dispose()
        {
            Position = null;
        }
    }
}
