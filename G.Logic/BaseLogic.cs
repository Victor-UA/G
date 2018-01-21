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
                        Vector3 BreaksFrictionResistanse =
                                  movableItem.Velocity *
                                  movableItem.FrictionResistanceCoefficientS *
                                  item.Mass * 9.8;
                        //double alpha = item.Position.Direction.Angle(movableItem.Velocity);
                        //bool isForward = alpha < 90 || alpha > 270;

                        if (mState.Forward || mState.Backward)
                        {
                            if (mState.Forward)
                            {
                                Force += item.Position.Direction * movableItem.Power.Linear.Forward;
                            }
                            if (mState.Backward)
                            {
                                Force -= item.Position.Direction * movableItem.Power.Linear.Backward;
                            }
                        }
                        else
                        {
                            if (movableItem.BreakesAreOnWithoutMoving)
                            {
                                Force -= BreaksFrictionResistanse;
                            }
                        }                        
                        
                                                
                        if (mState.StrafeLeft)
                        {
                            Force -= item.Position.Direction.RotateZ(Math.PI / 2) * movableItem.Power.Linear.StrafeLeft;
                        }
                        if (mState.StrafeRight)
                        {
                            Force += item.Position.Direction.RotateZ(Math.PI / 2) * movableItem.Power.Linear.StrafeRight;
                        }

                        double t = timeElapsed.TotalSeconds;
                        double t2 = Math.Pow(t, 2);

                        Vector3 EnvironmentResistance =                            
                            movableItem.AerodynamicResistanceCoefficientS *
                            movableItem.Environment.Density *
                            movableItem.Velocity * movableItem.Velocity.Magnitude / 2;

                        if (movableItem.Velocity <= Vector3.Epsilon)
                        {
                            Vector3 a = (Force - EnvironmentResistance) / item.Mass;
                            item.Position.Location += a * t2 / 2;
                            movableItem.Velocity += a * t;
                        }
                        else
                        {
                            Vector3 SideFrictionResistanse = movableItem.Velocity.Rejection(item.Position.Direction);
                            SideFrictionResistanse =
                                SideFrictionResistanse *
                                movableItem.FrictionResistanceCoefficientS *
                                item.Mass * 9.8;

                            //X: Projected On Velocity
                            //Y: RejectedOnVelocity                   
                            Vector3 SideFrictionResistanseX = SideFrictionResistanse.Projection(movableItem.Velocity);                            
                            Vector3 SideFrictionResistanseY = SideFrictionResistanse - SideFrictionResistanseX;                            

                            Vector3 ForceX = Force.Projection(movableItem.Velocity);
                            Vector3 ForceY = Force - ForceX - SideFrictionResistanseY;
                            ForceX = ForceX - EnvironmentResistance - SideFrictionResistanseX;


                            Vector3 ax = ForceX / item.Mass;
                            Vector3 ay = ForceY / item.Mass;
                            movableItem.Velocity += (ax + ay) * t;

                            if (movableItem.Velocity.IsNaN())
                            {
                                movableItem.Velocity = Vector3.Zero;
                            }

                            Vector3 dX = movableItem.Velocity + ax * t2 / 2;
                            Vector3 dY = ay * t2 / 2;                            
                            item.Position.Location += (dX + dY);
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
