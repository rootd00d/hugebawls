using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HugeBawls.Hud;
using HugeBawls.Entities;

namespace HugeBawls.State.Pointer
{
    public class Select : State<Entities.Pointer>
    {
        private static Select s_Instance = null;

        private EntryBawl m_BawlEntry;

        public override void Enter(Entities.Pointer p)
        {
            UI.LogMessage(p.ID.ToString());
            p.SetCursor(Entities.Pointer.CursorDefault);
        }

        public override void Execute(Entities.Pointer p)
        {
            if (p.Queue.Count == 0)
                return;

            MouseEventArgs e = (MouseEventArgs)p.Queue.Dequeue();

            if (e.Button == MouseButtons.Left)
            {
                Entities.Bawl b = (Entities.Bawl)Entities.Pointer.NearestEntity;

                if (b != null)
                {
                    b.Selected = !b.Selected;

                    if (b.Selected)
                    {
                        Group group = Hud.Control.Instance.GetGroup("Bawls");
                        group.Add(new EntryBawl(group, b.ID.ToString(), b));
                    }
                    else
                    {
                        if (Hud.Control.Instance.HasGroup("Bawls"))
                        {
                            Group group = Hud.Control.Instance.GetGroup("Bawls");
                            EntryGeneric entry = group.GetEntry(b.ID.ToString());
                            group.Remove(entry);
                        }
                    }
                }

            }
            else if (e.Button == MouseButtons.Right)
            {
                if (p.FSM.Global == Follow.Instance)
                {
                    p.FSM.ChangeGlobal(null);
                }
                else
                {
                    p.FSM.ChangeGlobal(Follow.Instance);
                }
            }
        }

        public override void Exit(Entities.Pointer p)
        {
            p.Queue.Clear();
        }

        public static Select Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new Select();
                return s_Instance;
            }
        }
    }



}