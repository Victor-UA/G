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
            Player player = new Player();
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
                        IMovable movableItem = item as IMovable;
                        MoveStates mState = movableItem.MoveState;

                        if (mState.RotateLeft)
                        {
                            item.Position.Direction = item.Position.Direction.RotateZ(-movableItem.Power.Angular.ZCounterClockWise * timeElapsed.TotalSeconds);
                        }
                        if (mState.RotateRight)
                        {
                            item.Position.Direction = item.Position.Direction.RotateZ(movableItem.Power.Angular.ZClockWise * timeElapsed.TotalSeconds);
                        }

                        Vector3 Force = new Vector3();
                        if (mState.Forward)
                        {
                            Force += item.Position.Direction * movableItem.Power.Linear.Forward;
                        }
                        if (mState.Backward)
                        {
                            Force -= item.Position.Direction * movableItem.Power.Linear.Backward;
                        }
                        if (mState.StrafeLeft)
                        {
                            Force -= item.Position.Direction.RotateZ(Math.PI/2) * movableItem.Power.Linear.StrafeLeft;
                        }
                        if (mState.StrafeRight)
                        {
                            Force += item.Position.Direction.RotateZ(Math.PI/2) * movableItem.Power.Linear.StrafeRight;
                        }

                        if (Force.Magnitude != 0)
                        {
                            Vector3 ForceProjectedOnVelocity = Force.Projection(movableItem.Velocity);
                            Vector3 ForceRejectedOnVelocity = Force - ForceProjectedOnVelocity;
                            Vector3 Velocity1ProjectedOnVelocity = movableItem.Velocity + ForceProjectedOnVelocity / (item.Mass * timeElapsed.TotalSeconds);
                            Vector3 Velocity1RejectedOnVelocity = ForceRejectedOnVelocity / (item.Mass * timeElapsed.TotalSeconds);
                            //Визначити кінцеву позицію з урахуванням постійної дії сили
                            //V = V0 + F/(m*t)
                            //movableItem.Velocity
                            //item.Position.Location += delta * timeElapsed.TotalSeconds * movableItem.Velocity;
                        }
                        else
                        {
                            movableItem.Velocity = item.Position.Direction * 0;
                        }                        
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
