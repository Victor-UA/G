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
            Entities = new HashSet<Entity>();
        }
        public ICollection<Entity> Entities { get; }
        private Player _player;

        public void NewPlayer()
        {
            _player = new Player();            
        }
    }
}
