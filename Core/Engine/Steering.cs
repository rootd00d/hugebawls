using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using HugeBawls;
using HugeBawls.Entities;
using HugeBawls.Hud;

namespace HugeBawls
{
    public enum SteeringBehaviour
    {
        Gravity = 1,
        Seek = 2,
        Flee = 4,
        Arrive = 8,
    }
    
    public class Steering
    {
        public static double AccelerationMultiplier = Settings.ScaleAcceleration;
        public static double VelocityMultiplier = Settings.ScaleVelocity;
        public static double GravityMultiplier = Settings.ScaleGravity;

        private Vehicle m_Vehicle;

        private Vector3D m_TargetSeek;
        private Vector3D m_TargetArrive;
        private Vector3D m_TargetFlee;

        private Hud.EntryGeneric hudEntryArrive;

        private int m_Behaviours;

        private static Vector3D s_CenterOfGravity;

        private static double s_AverageMass;

        public Steering(Vehicle vehicle)
        {
            m_Vehicle = vehicle;
            m_Behaviours = 0;
        }

        public void GravityOn()
        {
            EnableBehaviour(SteeringBehaviour.Gravity);
        }

        public void SeekOn(Vector3D target)
        {
            m_TargetSeek = target;
            EnableBehaviour(SteeringBehaviour.Seek);
        }

        public void ArriveOn(Vector3D target)
        {
            m_TargetArrive = target;
            EnableBehaviour(SteeringBehaviour.Arrive);

            hudEntryArrive = Hud.Control.Instance.AddGeneric("Steering", "Arrive", null);
        }

        public void ArriveOff()
        {
            m_TargetArrive = null;
            RemoveBehaviour(SteeringBehaviour.Arrive);

            Hud.Control.Instance.GetGroup("Steering").Remove(hudEntryArrive);

            hudEntryArrive = null;
        }

        private void EnableBehaviour(SteeringBehaviour behaviour)
        {
            m_Behaviours = m_Behaviours | (int)behaviour;
        }

        public void RemoveBehaviour(SteeringBehaviour behaviour)
        {
            m_Behaviours = m_Behaviours & (~(int)behaviour);
        }

        public void SetTarget(Vector3D pos)
        {
            m_TargetArrive = pos;
        }

        public bool On(SteeringBehaviour behaviour)
        {
            return ((m_Behaviours & (int)behaviour) ==
                (int)behaviour);
        }

        public Vector3D Calculate()
        {
            Vector3D resultant = new Vector3D(0, 0, 0);

            if (On(SteeringBehaviour.Gravity))
            {
                resultant += Gravitate();
            }

            if (On(SteeringBehaviour.Seek))
            {
                resultant += Seek(m_TargetSeek);
            }

            if (On(SteeringBehaviour.Arrive))
            {
                resultant += steer(m_TargetArrive);
            }

            return resultant;
        }

        public Vector3D Gravitate()
        { 
            Vector3D vForce = new Vector3D(0);

            World.PrepareVehicleNeighbours(m_Vehicle.Partition);

            foreach (Partition<Vehicle> partition in World.CurrentNeighbours)
            {

                foreach (Vehicle bawl in partition.Members)
                {

                    if (bawl == m_Vehicle) continue;

                    Vector3D vDirection = World.NeighbourDirection(m_Vehicle.PreviousPosition, bawl.PreviousPosition);

                    double distanceSq = vDirection.MagnitudeSq();


                    if (vDirection > Settings.VectorMinMagnitude)
                    {
                        vDirection.Normalize();

                        vDirection *= GravityMultiplier * bawl.Mass / distanceSq;
                    }
                    else
                    {
                        vDirection = new Vector3D(0);
                    }

                    vForce += vDirection;
                }
            }

            return vForce;

        }

        public Vector3D Seek(Vector3D targetPos)
        {
            Vector3D desiredVelocity = (targetPos - m_Vehicle.Position).Normalized() * m_Vehicle.MaxSpeed;

            return (desiredVelocity - m_Vehicle.Velocity);
        }

        /*
        public Vector3D Arrive(Vector3D targetPos)
        {
            Vector3D toTarget = targetPos - m_Vehicle.Position;

            double speed = m_Vehicle.Velocity.Magnitude();

            if (toTarget.Magnitude() > speed * HugeBawls.TimeDelta.TotalSeconds)
            {
                return (toTarget.Normalized() * m_Vehicle.MaxAccel);
            }
            else
            {
                return (toTarget.Normalized() * (-0.5 * m_Vehicle.Mass * speed * speed) / toTarget.Magnitude());
            }


        }

         */

        /*

        public Vector3D Arrive(Vector3D targetPos)
        {
            double dThresh = double.MaxValue;

            Vector3D toTarget = targetPos - m_Vehicle.Position;

            if (m_Vehicle.Acceleration.MagnitudeSq() != 0)
            {
                dThresh = 2 * m_Vehicle.Velocity.MagnitudeSq() / m_Vehicle.Acceleration.Magnitude();
            }

            if (toTarget.Magnitude() < 10 && m_Vehicle.Velocity < 1)
            {
                m_Vehicle.Steering.RemoveBehaviour(SteeringBehaviour.Arrive);
                return new Vector3D(0);
            }
        
            if (dThresh > toTarget.Magnitude())
            {
                return (toTarget.Normalized() * -1 * m_Vehicle.Mass * m_Vehicle.MaxAccel);
            }
            else
            {
                return (toTarget.Normalized() * m_Vehicle.Mass * m_Vehicle.MaxAccel);
            }

        }
         * */

        public Vector3D Arrive(Vector3D targetPos)
        {
            double dThresh = double.MaxValue;
            double dAccelSq = m_Vehicle.Acceleration.MagnitudeSq();
            Vector3D toTarget = targetPos - m_Vehicle.Position;

            //if (dAccelSq > 0)
            //{

            //}

            double speed = m_Vehicle.Velocity.Magnitude();
            double dist = toTarget.Magnitude();

            if (dist > 0)
            {

                dThresh = 2 * m_Vehicle.Velocity.MagnitudeSq() / (m_Vehicle.MaxAccel * m_Vehicle.MaxAccel);
                double theta = 0;

                if (speed != 0 && dist != 0)
                {
                    theta = Math.Acos(m_Vehicle.Velocity.Dot(toTarget) / Math.Abs(speed * dist));
                }

                hudEntryArrive.Update(theta * 360 / (2 * Math.PI));

                if (dThresh > toTarget.Magnitude())
                {
                    return (Vector3D)(toTarget.Normalized() * -1 * m_Vehicle.Mass * m_Vehicle.MaxAccel).Rotated2D(-theta % Math.Truncate(Math.PI / 4));
                }
                else
                {
                    return (Vector3D)(toTarget.Normalized() * m_Vehicle.Mass * m_Vehicle.MaxAccel);
                }
            }
            else
            {
                return new Vector3D(0);
            }

        }

        Vector3D steer(Vector3D targetPos)
        {
            Vector3D steer;  // The steering vector
            Vector3D desired = targetPos - m_Vehicle.Position;  // A vector pointing from the location to the target
            double d = desired.Magnitude(); // Distance from the target is the magnitude of the vector
            // If the distance is greater than 0, calc steering (otherwise return zero vector)
            if (d > 0)
            {
                // Normalize desired
                desired.Normalize();
                // Two options for desired vector magnitude (1 -- based on distance, 2 -- maxspeed)
                if ((d < 100.0f))
                {
                    desired = desired * m_Vehicle.MaxSpeed * (d / 100.0f); // This damping is somewhat arbitrary
                }
                else
                {
                    desired = desired * m_Vehicle.MaxSpeed;
                }
                // Steering = Desired minus Velocity
                steer = desired - m_Vehicle.Velocity;
                steer.Truncate(m_Vehicle.MaxAccel);
            }
            else
            {
                steer = new Vector3D(0);
            }
            return steer;
        }

        
        public Vector3D Collapse()
        {
            Vector3D vDirection = s_CenterOfGravity - m_Vehicle.Position;

            if (vDirection.MagnitudeSq() < Settings.VectorMinMagnitude)
                return vDirection;

            vDirection.Normalize();

            double dForce = GravityMultiplier * m_Vehicle.Mass / m_Vehicle.Position.DistanceSq(s_CenterOfGravity);

            return vDirection * dForce;
        }



        public static void ComputeCenterOfGravity()
        {
            int numBalls = 0;
            double avgMass = 0;
            double totalMass = 0;
            Vector3D vCenter = new Vector3D(0);

            foreach (Vehicle b in Manager.Instance.Registered())
            {
                if (numBalls == 0)
                {
                    vCenter = b.Position * b.Mass;
                    numBalls++;
                    totalMass = b.Mass;
                    avgMass = b.Mass;
                    s_AverageMass = avgMass;
                    continue;
                }
                
                numBalls++;
                totalMass += b.Mass;
                avgMass = totalMass / numBalls;

                vCenter += (b.Position * b.Mass);    
            }

            if (avgMass == 0)
            {
                s_CenterOfGravity = new Vector3D(0);
            }
            else
            {
                vCenter /= (totalMass);

                s_CenterOfGravity = vCenter;
            }

            s_AverageMass = avgMass;
        }


        public static void ResolveCollisions()
        {
            //bool colliding = false;

            // TODO: do
            //{
                foreach (Vehicle b in Manager.Instance.Registered())
                {
                    using (b as Bawl)
                    {
                        if (b != null)
                        {
                            ResolveCollision(b);
                        }
                    }
                }
            
            //}
            //while (colliding);

        }

        public static void ResolveCollision(Vehicle vehicle)
        {
            Bawl bA = vehicle as Bawl;

            if (bA == null) return;

            World.PrepareVehicleNeighbours(bA.Partition);

            foreach (Partition<Vehicle> partition in World.CurrentNeighbours)
            {
                foreach (Vehicle vehicle2 in partition.Members)
                {
                    Bawl bB = vehicle2 as Bawl;

                    if (bB == null) continue;

                    if (bA == bB) continue;

                    Vector3D distance = bA.Position - bB.Position;

                    double length = distance.Magnitude();

                    Vector3D n = distance.Normalized();

                    double overlap = ((double)(int)bB.Size + (double)(int)bA.Size) / 2 - length;

                    if (overlap >= 0)
                    {
                        if (
                            bA.Mass < bB.Mass ||
                            ( (bA.Mass == bB.Mass) && (bA.Velocity > bB.Velocity) )
                            )
                        {
                            bA.PreviousPosition = bA.Position;
                            bA.Position += n * overlap;
                        }
                        else if (
                            bB.Mass < bA.Mass ||
                            ((bB.Mass == bA.Mass) && (bB.Velocity > bA.Velocity))
                            )
                        {
                            bB.PreviousPosition = bB.Position;
                            bB.Position -= n * overlap;
                        }
                        else
                        {
                            bA.PreviousPosition = bA.Position;
                            bB.PreviousPosition = bB.Position;
                            bA.Position += n * overlap / 2;
                            bB.Position -= n * overlap / 2;
                        }

                        Vector3D vAB = bA.Velocity - bB.Velocity;

                        double vRel = vAB.Dot(n);

                        if (vRel < -Settings.VectorMinMagnitude)
                        {
                            double e = 1.0;

                            double j = ((-(1.0 + e) * vRel) / (1.0 / bA.Mass + 1.0 / bB.Mass));

                            bA.Velocity += n * (j / bA.Mass);
                            bB.Velocity -= n * (j / bB.Mass);

                            bA.Velocity.Truncate(bA.MaxSpeed);
                            bB.Velocity.Truncate(bB.MaxSpeed);
                        }

                    }
                }
            }
        }

        public static Vector3D CenterOfGravity
        {
            get { return s_CenterOfGravity; }
        }
    }
}

