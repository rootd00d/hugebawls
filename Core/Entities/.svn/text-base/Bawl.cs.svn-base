 using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using HugeBawls;
using HugeBawls.State;

namespace HugeBawls.Entities
{
    public class Bawl : Vehicle
    {
        private StateMachine<Bawl> m_FSM;

        private Pen m_Pen;
        private Brush m_Brush;
        
        private BawlSize m_Size;

        private int m_CacheID;

        public Bawl(
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
            m_Pen =  pen;
            m_Brush = brush;
            m_Size = size;

            m_CacheID = (m_Brush.GetHashCode() + (int)m_Size);

            MaxSpeed = Settings.BawlMaxVelocity;
            MaxAccel = Settings.BawlMaxAcceleration;

            m_FSM = new StateMachine<Bawl>(this);

            m_FSM.ChangeGlobal(State.Bawl.Global.Instance);
        }

        public StateMachine<Bawl> FSM
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
            set
            {
                m_Brush = value;
                m_Pen = new Pen(m_Brush);
                m_CacheID = (m_Brush.GetHashCode() + (int)m_Size);
            }
        }

        public override void Update(double delta)
        {
            if (m_FSM.Current != null)
                FSM.Current.Execute(this);

            base.Update(delta);
        }
        
        public override void Render()
        {
            if (Visible)
            {
                Vector3D p = World.ToLocal(Position);

                if (this.Selected)
                {
                    Renderer.DrawEllipse(
                        p,
                        (int)Size + 10,
                        Pens.Black,
                        Brushes.Yellow);

                }

                if (UI.ShowNearest && (this == Pointer.NearestEntity))
                {
                    Renderer.DrawEllipse(
                        p,
                        (int)Size + 6,
                        Pens.Black,
                        Brushes.Silver);
                }

                Renderer.DrawEllipse(m_CacheID, p, (int)Size, Pens.White, m_Brush);

                if (UI.ShowHeading)
                {
                    Renderer.DrawVector(p, Heading * ((double)Size / 2), Pens.Silver);
                }

                if (UI.ShowVelocity)
                {
                    Renderer.DrawVector(
                        p,
                        Velocity * Settings.VelocityVectorScale,
                        Renderer.PenVelocity);
                }

                if (UI.ShowAcceleration)
                {
                    Renderer.DrawVector(
                        p,
                        Acceleration * Settings.AccelerationVectorScale,
                        Renderer.PenAcceleration);
                }
            }

            Renderer.MiniMapDrawBawl(this);

        }

        public override int GetHashCode()
        {
            return (m_Size.GetHashCode() + m_Brush.GetHashCode());
        }

        public override bool HandleMessage(Message msg)
        {
            return m_FSM.HandleMessage(msg);    
        }

        public BawlSize Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public bool Visible
        {
            get
            {
                double rad = ((double)Size / 2);

                return World.Visible(Position, rad);
            }
        }

        

    }
}
