using System;
using System.Collections.Generic;
using System.Text;

namespace HugeBawls.Goal
{
    public enum Status
    {
        Active,
        Complete,
        Failed
    }

    public abstract class Goal<T>
    {
        protected T m_Owner;
        protected Status m_Status;
        protected int m_Type;

        public abstract void Activate();

        public abstract Status Process();

        public abstract void Terminate();

        public abstract bool HandleMessage(Message msg);

        public virtual void AddSubGoal(Goal<T> goal)
        {
            throw new Exception("Can not add subgoal to atomic goal");
        }

        public bool Active
        {
            get { return (m_Status == Status.Active); }
        }

        public bool Complete
        {
            get { return (m_Status == Status.Complete); }
        }

        public bool Failed
        {
            get { return (m_Status == Status.Failed); }
        }

        public int Type
        {
            get { return m_Type; }
        }

    }

}
