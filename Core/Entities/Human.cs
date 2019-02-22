using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using HugeBawls.State;

namespace HugeBawls.Entities
{
    public class Human : Vehicle
    {
        private StateMachine<Human> m_FSM;

        private Pen m_Pen;
        private Brush m_Brush;

        private BawlSize m_Size;

        private int m_CacheID;

        public Human(
            Vector3D position,
            Vector3D velocity,
            Vector3D acceleration,
            Vector3D angularVelocity,
            Vector3D angularAcceleration,
            double mass,
            Pen pen,
            Brush brush,
            BawlSize size
            )
            :
            base(position, velocity, acceleration, angularVelocity, angularAcceleration, mass)
        {
            m_Pen = pen;
            m_Brush = brush;
            m_Size = size;

            m_CacheID = (m_Brush.GetHashCode() + (int)m_Size) % 256;
            
            MaxSpeed = Settings.HumanMaxVelocity;
            MaxAccel = Settings.HumanMaxAcceleration;

            m_FSM = new StateMachine<Human>(this);

            m_FSM.ChangeGlobal(State.Human.Global.Instance);
        }

        public override void Render()
        {
            Renderer.DrawHuman(m_CacheID, World.ToLocal(Position), (double)BawlSize.Huge);
        }

        public override bool HandleMessage(Message msg)
        {
            return m_FSM.HandleMessage(msg);
        }


        public StateMachine<Human> FSM
        {
            get { return m_FSM; }
        }

        public Pen Pen
        {
            get { return m_Pen; }
        }

        public Brush Brush
        {
            get { return m_Brush; }
        }

        public override void Update(double delta)
        {
            if (m_FSM.Global != null)
                m_FSM.Global.Execute(this);

            if (m_FSM.Current != null)
                FSM.Current.Execute(this);

            base.Update(delta);
        }
    }
}