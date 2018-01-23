using G.Base;
using RP.Math;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace G.Logic
{
    public class BaseLogic: IDisposable
    {
        public BaseLogic()
        {
            _entities = new Dictionary<int, Entity>();            
        }

        private object _lock = new object();
        private IDictionary<int, Entity> _entities;
        private Random _random = new Random();

        public Player AddPlayer()
        {
            MovableEntity player = new Player(75);
            player.Position.Location = new Vector3(_random.Next(500), _random.Next(500), 0);
            player.Position.Direction = new Vector3(_random.Next(500), _random.Next(500), 0);
            lock (_lock)
            {
                _entities.Add(player.ID, player);            
            }
            player.Start();
            return player as Player;
        }
        public void KillPlayer(int id)
        {
            lock (_lock)
            {
                try
                {
                    _entities[id].Dispose();
                    _entities.Remove(id);                    
                }
                catch (Exception) { }
            }
        }
        
        public void CheckStrikes()
        {
            lock (_lock)
            {
                foreach (var item in _entities.Values)
                {
                    if (item is IStrikable)
                    {

                    }
                }
            }
        }

        public IDictionary<int, Entity> GetEntities()
        {
            lock (_lock)
            {
                return new Dictionary<int, Entity>(_entities);
            }
        }        

        public void SetPlayerStates(int id, MoveStates moveState)
        {
            lock (_lock)
            {
                if (_entities.ContainsKey(id))
                {
                    Entity entity = _entities[id];
                    if (entity is Player)
                    {
                        try
                        {
                            (entity as Player).MoveState = moveState;
                        }
                        catch (Exception){}
                    }
                }
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var item in _entities.Values)
                {
                    item.Dispose();
                }
                _entities.Clear();
            }
        }
    }
}
