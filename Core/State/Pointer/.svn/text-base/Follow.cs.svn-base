using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HugeBawls.Hud;
using HugeBawls.Entities;

namespace HugeBawls.State.Pointer
{
    public class Follow : State<Entities.Pointer>
    {
        private static Follow s_Instance = null;

        private static Vehicle s_Follow = null;

        public override void Enter(Entities.Pointer p)
        {
            s_Follow = (Vehicle)Entities.Pointer.NearestEntity;

            if (s_Follow != null)
            {
                UI.LogMessage(p.ID.ToString() + " following " + s_Follow.ID.ToString());
            }
        }

        public override void Execute(Entities.Pointer p)
        {
            if (s_Follow == null)
            {
                UI.LogMessage(p.ID.ToString() + ": Nothing to follow");
                p.FSM.Revert();
                return;
            }
            else
            {
                World.SetOrigin(s_Follow.Position - World.ViewSize / 2);
            }
        }

        public override void Exit(Entities.Pointer p)
        {
            p.SetCursor(Entities.Pointer.CursorDefault);

            UI.LogMessage(p.ID.ToString() + " stopped following " + s_Follow.ID.ToString());

            s_Follow = null;
        }

        public static Follow Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new Follow();
                return s_Instance;
            }
        }
    }

}