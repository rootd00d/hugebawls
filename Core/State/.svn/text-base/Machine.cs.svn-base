using System;
using System.Collections.Generic;
using System.Text;

namespace HugeBawls.State
{
    public class StateMachine<EntityType>
    {
        private EntityType m_Owner;

        private State<EntityType> m_Current;

        private State<EntityType> m_Previous;

        private State<EntityType> m_Global;

        public StateMachine(EntityType owner)
        {
            m_Owner = owner;
            m_Current = null;
            m_Previous = null;
            m_Global = null;
        }

        public void Update()
        {
            if (m_Global != null)
                m_Global.Execute(m_Owner);

            if (m_Current != null)
                m_Current.Execute(m_Owner);
        }

        public void ChangeGlobal(State<EntityType> s)
        {
            if (m_Global != null)
                m_Global.Exit(m_Owner);

            m_Global = s;

            if (m_Global != null)
                m_Global.Enter(m_Owner);
        }

        public void Change(State<EntityType> s)
        {
            m_Previous = m_Current;

            if (m_Current != null) m_Current.Exit(m_Owner);
            
            m_Current = s;

            if (m_Current != null) m_Current.Enter(m_Owner);
        }

        public void Revert()
        {
            Change(m_Previous);
        }

        public bool HandleMessage(Message m)
        {
            try
            {
                if (m_Current != null)
                    return m_Current.OnMessage(m_Owner, m);

                if (m_Global != null)
                    return m_Global.OnMessage(m_Owner, m);
            }
            catch (Exception e)
            {
                UI.LogMessage(e.ToString());
                return false;
            }

            return true;
        }


        public State<EntityType> Current
        {
            get { return m_Current; }
        }

        public State<EntityType> Previous
        {
            get { return m_Previous; }
        }

        public State<EntityType> Global
        {
            get { return m_Global; }
        }
    }
}
