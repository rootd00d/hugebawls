using System;
using System.Collections.Generic;
using System.Text;

namespace HugeBawls.Entities
{

    public abstract class Base : IDisposable
    {
        private int m_ID = 0;

        private static int m_iNextID = 0;

        private bool m_Selected = false;

        public int GetNextID()
        {
            return m_iNextID++;
        }

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public abstract void Update(double delta);

        public bool Selected
        {
            get { return m_Selected; }
            set { m_Selected = value; }
        }

        public abstract bool HandleMessage(Message msg);

        public void Dispose()
        {
        }

    }



}
