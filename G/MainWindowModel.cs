using G.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G
{
    public class MainWindowModel
    {
        public MainWindowModel()
        {
            Entities = new HashSet<IEntity>();
        }
        public ICollection<IEntity> Entities { get; }
        private Player _player;

        public void NewPlayer()
        {
            _player = new Player(75);            
        }
    }
}
