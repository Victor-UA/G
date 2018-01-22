using G.Base;
using RP.Math;
using System;
using System.Collections.Generic;
using System.Numerics;
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

        public Player AddPlayer()
        {
            Player player = new Player(75);
            lock (_lock)
            {
                _entities.Add(player.ID, player);            
            }
            return player;
        }
        public void KillPlayer(int id)
        {
            lock (_lock)
            {
                if (_entities.ContainsKey(id))
                {
                    Entity player = _entities[id];
                    if (player is Player)
                    {
                        _entities.Remove(id);
                        player.Dispose();
                    }
                }
            }
        }
        public void DoMoves(TimeSpan timeElapsed)
        {
            lock (_lock)
            {
                foreach (var item in _entities.Values)
                {
                    if (item is IMovable)
                    {
                        (item as IMovable).DoMoves();                        
                    }
                }
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

        public void SetPlayerStates(Player player)
        {
            lock (_lock)
            {
                if (_entities.ContainsKey(player.ID))
                {
                    Entity entity = _entities[player.ID];
                    if (entity is Player)
                    {
                        try
                        {
                            (entity as Player).MoveState = player.MoveState;
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
