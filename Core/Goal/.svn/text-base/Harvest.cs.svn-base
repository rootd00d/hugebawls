using System;
using System.Collections.Generic;
using System.Text;
using HugeBawls.Entities;

namespace HugeBawls.Goal
{
    class Harvest : Composite<Vehicle>
    {
        private Vehicle m_Owner;
        private Vector3D m_Target;

        public Harvest(Vehicle owner, Vector3D target)
        {
            m_Owner = owner;
            m_Target = target;
        }

        public override void Activate()
        {
            m_Status = Status.Active;

            m_Owner.Steering.ArriveOn(m_Target);
        }

        public override Status Process()
        {
            return Status.Active;
        }

        public override void Terminate()
        {
            
        }
    }
}
