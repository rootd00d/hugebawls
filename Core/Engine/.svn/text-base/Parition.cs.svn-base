using System;
using System.Collections.Generic;
using System.Text;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls
{
    public class Partition<MemberType>
    {
        protected List<MemberType> m_Members;

        protected int m_Row;
        protected int m_Col;

        protected int m_MinX;
        protected int m_MinY;
        protected int m_MaxX;
        protected int m_MaxY;

        public Partition(int row, int col, int width, int height)
        {
            m_Row = row;
            m_Col = col;

            m_MinY = row * height;
            m_MinX = col * width;

            m_MaxY = m_MinY + width;
            m_MaxX = m_MinX + height;

            m_Members = new List<MemberType>();
        }

        public void Add(MemberType member)
        {
            // PERF: assumes the member is not ALREADY a member
            m_Members.Add(member);
        }

        public void Remove(MemberType member)
        {
            m_Members.Remove(member);
        }

        public List<MemberType> Members
        {
            get { return m_Members; }
        }

        public bool Belongs(Vehicle member)
        {
            return ((member.Position.I >= m_MinX) &&
                (member.Position.J >= m_MinY) &&
                (member.Position.J < m_MaxX) &&
                (member.Position.J < m_MaxY));
        }

        public int Row
        {
            get { return m_Row; }
        }

        public int Column
        {
            get { return m_Col; }
        }
        
    }

}