using System;
using System.Collections.Generic;
using System.Text;

using HugeBawls;


namespace HugeBawls.Entities
{


    public abstract class Vehicle : Base
    {
        private Partition<Vehicle> m_Partition;

        private Vector3D m_vHeading;


        private Vector3D m_vNormal;

        /// <summary>
        /// Position
        /// </summary>
        private Vector3D m_vPos;

        private Vector3D m_vPosP;

        /// <summary>
        /// Velocity
        /// </summary>
        private Vector3D m_vVel;

        /// <summary>
        /// Acceleration
        /// </summary>
        private Vector3D m_vAccel;

        /// <summary>
        /// Mass
        /// </summary>
        private double m_dMass;

        /// <summary>
        /// Rotation
        /// </summary>
        private Vector3D m_vAAccel;

        private Vector3D m_vAVel;

        private Steering m_Steering;

        private double m_dMaxSpeed;

        private double m_dMaxAccel;

        private bool m_Active;

        public Vehicle(
            Vector3D position,
            Vector3D velocity,
            Vector3D acceleration,
            Vector3D angularVelocity,
            Vector3D angularAcceleration,
            double mass
            )
        {
            m_vPos = position;
            m_vPosP = position;

            m_vVel = velocity;
            m_vAccel = acceleration;

            m_vAVel = angularVelocity;
            m_vAAccel = angularAcceleration;

            m_vHeading = new Vector3D(1); // ->

            m_dMass = mass;

            ID = GetNextID();

            m_Steering = new Steering(this);
        }

        public abstract void Render();

        public void Stop()
        {
            m_vVel = new Vector3D(0, 0, 0);
            m_vAccel = new Vector3D(0, 0, 0);
        }


        public override void Update(double delta)
        {
            Vector3D vSteeringForce = m_Steering.Calculate();

            if ((vSteeringForce.MagnitudeSq() > Settings.VectorMinMagnitude))
            {
                if (Mass != 0)
                {
                    m_vAccel = vSteeringForce / Mass * Steering.AccelerationMultiplier;
                }
                else
                {
                    m_vAccel = vSteeringForce * Steering.AccelerationMultiplier;
                }

                m_vAccel.Truncate(MaxAccel);

                m_vVel += m_vAccel * delta;

                m_vVel.Truncate(MaxSpeed);

            }

            /// How do I account angular acceleration?
            m_vAVel += m_vAAccel * delta;

            m_vPosP = m_vPos;

            m_vPos += m_vVel * delta * Steering.VelocityMultiplier;

            m_Partition = World.VehicleReport(this);
        }

        public double MaxSpeed
        {
            get { return m_dMaxSpeed; }
            set { m_dMaxSpeed = value; }
        }

        public double MaxAccel
        {
            get { return m_dMaxAccel; }
            set { m_dMaxAccel = value; }
        }

        public Vector3D Position
        {
            get { return m_vPos; }
            set { m_vPos = value; }
        }

        public Vector3D PreviousPosition
        {
            get { return m_vPosP; }
            set { m_vPosP = value; }
        }

        public Vector3D Velocity
        {
            get { return m_vVel; }
            set { m_vVel = value; }
        }

        public Vector3D Acceleration
        {
            get { return m_vAccel; }
            set { m_vAccel = value; }
        }

        public Vector3D AngularAcceleration
        {
            get { return m_vAAccel; }
            set { m_vAAccel = value; }
        }

        public Vector3D AngularVelocity
        {
            get { return m_vAVel; }
            set { m_vAVel = value; }
        }

        public Steering Steering
        {
            get { return m_Steering; }
        }

        public double Mass
        {
            get { return m_dMass; }
            set { m_dMass = value; }
        }

        public Vector3D Heading
        {
            get { return m_vHeading; }
            set { m_vHeading = value; }
        }

        public Partition<Vehicle> Partition
        {
            get { return m_Partition; }
            set { m_Partition = value; }
        }
    }

}