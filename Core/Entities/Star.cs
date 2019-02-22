using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using HugeBawls;
using HugeBawls.State;

namespace HugeBawls.Entities
{
    public class Star : Vehicle
    {
        const int STAR_PREFIX = 22200000;

        private int m_Size;

        private int m_CacheID;

        private Vector3D m_Offset;

        public Star(
            Vector3D position,
            Vector3D velocity,
            int size
            )
            :
            base(position, velocity, new Vector3D(0), new Vector3D(0), new Vector3D(0), 0)
        {
            m_Size = size;

            m_CacheID = STAR_PREFIX + m_Size;

            m_Offset = new Vector3D((double)m_Size / Settings.StarsMaxSize, (double)m_Size / Settings.StarsMaxSize, 0);
        }

        public void IncrementOffset()
        {
            Position.I = (Position.I + m_Offset.I) % World.GameSize.I;
            Position.J = (Position.J + m_Offset.J) % World.GameSize.J;
        }


        public override void Render()
        {
            Renderer.DrawStar(m_CacheID, Position, m_Size);
        }

        public override int GetHashCode()
        {
            return m_CacheID;
        }

        public override bool HandleMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public bool Visible
        {
            get
            {
                return World.Visible(Position, Size / 2);
            }
        }

    }
}
