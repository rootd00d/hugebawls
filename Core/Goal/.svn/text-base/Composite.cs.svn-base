using System;
using System.Collections.Generic;
using System.Text;

namespace HugeBawls.Goal
{
    public class Composite<T> : Goal<T>
    {
        protected List<Goal<T>> m_Subgoals;

        public Composite()
        {
            m_Subgoals = new List<Goal<T>>();
        }

        public Status ProcessSubGoals()
        {
            while ((m_Subgoals.Count != 0) &&
                    (m_Subgoals[0].Complete || m_Subgoals[0].Failed))
            {
                m_Subgoals[0].Terminate();
                m_Subgoals.RemoveAt(0);
            }

            if (m_Subgoals.Count != 0)
            {
                Status result = m_Subgoals[0].Process();

                if (result == Status.Complete && m_Subgoals.Count > 1)
                {
                    return Status.Active;
                }
                else
                {
                    return result;
                }
            }

            return Status.Complete;
        }

        public override void AddSubGoal(Goal<T> goal)
        {
            m_Subgoals.Insert(0, goal);
        }

        public void RemoveAllSubGoals()
        {
            foreach (Goal<T> goal in m_Subgoals)
            {
                goal.Terminate();
            }

            m_Subgoals.Clear();
        }

        public override Status Process()
        {
            throw new NotImplementedException();
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override bool HandleMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }
    }

}
