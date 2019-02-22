using System;
using System.Text;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls.State.Human
{
    public class Global : State<Entities.Human>
    {
        private static Global s_Instance = null;

        public override void Enter(Entities.Human e)
        {
            UI.LogMessage(e.ID.ToString());
        }

        public override void Execute(Entities.Human e)
        {

        }

        public override void Exit(Entities.Human e)
        {
        }


        public static Global Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new Global();
                return s_Instance;
            }
        }
    }

}
