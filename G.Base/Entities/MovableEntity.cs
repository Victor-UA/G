﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RP.Math;

namespace G.Base
{
    public abstract class MovableEntity : Entity, IMovable
    {
        protected MovableEntity(double mass) : base(mass)
        {
            MoveState = new MoveStates();
            Velocity = new Vector3();
            Power = new Powers();
            LastMoveTime = DateTime.Now;
        }

        public virtual MoveStates MoveState { get; set; }
        public virtual Vector3 Velocity { get; protected set; }
        public virtual Powers Power { get; set; }
        public virtual IEnvironment Environment { get; set; }
        public virtual double AerodynamicResistanceCoefficientS { get; protected set; }
        public virtual ISurface Surface { get; set; }
        public virtual double FrictionCoefficientS { get; protected set; }
        public virtual bool BreakesAreOnWithoutMoving { get; set; }
        protected virtual DateTime LastMoveTime { get; set; }

        public virtual void DoMoves()
        {
            DateTime Now = DateTime.Now;
            double time = (Now - LastMoveTime).TotalSeconds;
            double time2 = Math.Pow(time, 2);
            LastMoveTime = Now;

            #region Rotating
            if (MoveState.RotateLeft)
            {
                RotateZCounterClockWise(time);
            }
            if (MoveState.RotateRight)
            {
                RotateZClockWise(time);
            }
            #endregion

            double alpha = Position.Direction.Angle(Velocity);
            bool isGoingForward = alpha < Math.PI / 2;
            Vector3 Force = Vector3.Zero;
            bool brakesOn = false;

            #region Linear moving
            if (MoveState.Forward)
            {
                if (Velocity == Vector3.Zero || isGoingForward)
                {
                    Force += ForceForward();
                }
                else
                {
                    brakesOn = true;
                }
            }
            if (MoveState.Backward)
            {
                if (Velocity == Vector3.Zero || !isGoingForward)
                {
                    Force += ForceBackward();
                }
                else
                {
                    brakesOn = true;
                }
            }

            if (MoveState.StrafeLeft)
            {
                Force += ForceLeft();
            }
            if (MoveState.StrafeRight)
            {
                Force += ForceRight();
            }
            #endregion


            //X: Projected On Velocity
            //Y: RejectedOnVelocity                   

            if (Velocity.IsNaN() || Velocity.Magnitude < 0.5 || double.IsInfinity(Velocity.Magnitude))
            {
                Velocity = Vector3.Zero;
            }            

            Vector3 EnvironmentResistance =
                AerodynamicResistanceCoefficientS *
                Environment.Density *
                -Velocity * Velocity.Magnitude / 2;

            Vector3 BrakeFriction =
                Velocity.Magnitude > 0 ?
                      -Velocity.Normalize() *
                      FrictionCoefficientS * Surface.FrictionCoefficient *
                      Mass * 9.8
                      : Vector3.Zero;

            Vector3 VelocityRejectedOnDirection = Velocity.Rejection(Position.Direction);            
            Vector3 SideFriction =
                VelocityRejectedOnDirection.Magnitude > 0 ?
                    -VelocityRejectedOnDirection.Normalize() *
                    FrictionCoefficientS * Surface.FrictionCoefficient *
                    Mass * 9.8
                    : Vector3.Zero;

            if (!brakesOn)
            {
                Force += EnvironmentResistance + SideFriction;
            }
            else
            {
                Force += EnvironmentResistance + BrakeFriction;
            }

            Vector3 a = Force / Mass;
            Position.Location += Velocity * time + a * time2 / 2;
            Velocity += a * time;            
        }


        protected virtual Vector3 ForceForward()
        {
            return Position.Direction * Power.Linear.Forward;
        }

        protected virtual Vector3 ForceBackward()
        {
            return -Position.Direction * Power.Linear.Backward;
        }


        protected virtual Vector3 ForceLeft()
        {
            return Position.Direction.RotateZ(Math.PI / 2) * Power.Linear.StrafeLeft;
        }

        protected virtual Vector3 ForceRight()
        {
            return Position.Direction.RotateZ(-Math.PI / 2) * Power.Linear.StrafeRight;
        }


        protected virtual Vector3 ForceRotateXClockWise()
        {
            throw new NotImplementedException();
        }

        protected virtual Vector3 ForceRotateXCounterClockWise()
        {
            throw new NotImplementedException();
        }

        protected virtual Vector3 ForceRotateYClockWise()
        {
            throw new NotImplementedException();
        }

        protected virtual Vector3 ForceRotateYCounterClockWise()
        {
            throw new NotImplementedException();
        }

        protected virtual void RotateZClockWise(double time)
        {
            Position.Direction = Position.Direction.RotateZ(Power.Angular.ZClockWise * time);
        }

        protected virtual void RotateZCounterClockWise(double time)
        {
            Position.Direction = Position.Direction.RotateZ(-Power.Angular.ZCounterClockWise * time);
        }
    }
}
