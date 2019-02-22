using System;
using System.Text;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls.State.Human
{
    
    public class Harvest : State<Entities.Human>
    {
        private static Harvest s_Instance = null;

        public override void Enter(Entities.Human e)
        {
            UI.LogMessage(e.ID.ToString());

            Vehicle b = World.FindNearestNeighbour(e.Position);

            if (b != null)
            {
                e.Steering.ArriveOn(b.Position);
            }
        }

        public override void Execute(Entities.Human e)
        {
            Vehicle b = World.FindNearestNeighbour(e.Position);

            if (b != null)
            {
                e.Steering.SetTarget(b.Position);
            }
            else
            {
                e.Steering.ArriveOff();
            }
        }

        public override void Exit(Entities.Human e)
        {
            UI.LogMessage(e.ID.ToString());

            e.Steering.ArriveOff();
        }


        public static Harvest Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new Harvest();
                return s_Instance;
            }
        }
    }

}