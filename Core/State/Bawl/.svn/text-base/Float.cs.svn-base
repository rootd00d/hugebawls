using System;
using System.Text;
using HugeBawls;
using HugeBawls.Entities;
using HugeBawls.State;

namespace HugeBawls.State.Bawl
{

    public class Float : State<Entities.Bawl>
    {
        private static Float s_Instance = null;

        public override void Enter(Entities.Bawl e)
        {
            UI.LogMessage(e.ID.ToString());

            e.Steering.GravityOn();
        }

        public override void Execute(Entities.Bawl e)
        {
        }

        public override void Exit(Entities.Bawl e)
        {
        }

        public override bool OnMessage(Entities.Bawl e, Message m)
        {
            switch (m.Type)
            {
                case MessageType.Collision:
                    e.Velocity = (Vector3D)m.Data[0];
                    break;
                default:
                    break;
            }

            return true;
        }

        public static Float Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new Float();
                return s_Instance;
            }
        }
    }

}