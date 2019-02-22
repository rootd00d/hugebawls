using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HugeBawls.Hud;
using HugeBawls.Entities;

namespace HugeBawls.State.Pointer
{

    public class AddHuman : State<Entities.Pointer>
    {
        private static AddHuman s_Instance = null;

        public override void Enter(Entities.Pointer p)
        {
            UI.LogMessage(p.ID.ToString());
            p.SetCursor(Entities.Pointer.CursorDefault);
        }

        public override void Execute(Entities.Pointer p)
        {
            if (p.Queue.Count == 0)
                return;

            MouseEventArgs m = (MouseEventArgs)p.Queue.Dequeue();

            if (m.Button == MouseButtons.Left)
            {
                Entities.Human h = new Entities.Human(
                    World.ToWorld(new Vector3D((double)m.X, (double)m.Y, 0)),
                    new Vector3D(0),
                    new Vector3D(0),
                    new Vector3D(0),
                    new Vector3D(0),
                    0,
                    Pens.Beige,
                    Brushes.Beige,
                    BawlSize.Large);

                h.FSM.Change(State.Human.Harvest.Instance);

                UI.LogMessage("Adding Human({0})", h.ID);

                Manager.Instance.Register(h);

            }
        }

        public override void Exit(Entities.Pointer e)
        {

        }
        public static AddHuman Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new AddHuman();
                return s_Instance;
            }
        }
    }
}