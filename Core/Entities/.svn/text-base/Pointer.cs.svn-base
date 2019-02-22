using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using HugeBawls.State;

namespace HugeBawls.Entities
{
    public class Pointer : Base
    {
        private UI m_Owner = null;

        private StateMachine<Pointer> m_FSM;

        private Vector3D m_Location = new Vector3D(0);

        private Queue<EventArgs> m_EventQ = null;

        private Cursor m_CursorCurrent;

        private static Cursor s_CursorDefault;
        private static Cursor s_CursorBawl;
        private static Cursor s_CursorOrbital;

        private static Graphics s_gPointer;

        private static Bitmap s_bmpPointer;
        private static Bitmap s_bmpPointerTool;

        private static Base s_NearestEntity;

        public Pointer(UI ui, Image image)
        {
            m_Owner = ui;

            m_EventQ = new Queue<EventArgs>();

            m_FSM = new StateMachine<Pointer>(this);

            m_FSM.Change(State.Pointer.Select.Instance);
            
            s_CursorDefault = CursorFromImage(image);

            SetCursor(CursorDefault);
        }

        public Cursor CursorFromImage(Image image)
        {
            ImageAttributes iaCursor = new ImageAttributes();

            ColorMatrix cmCursor = new ColorMatrix();

            Bitmap bmpSource = (Bitmap)image;

            Bitmap bmpCursor = new Bitmap(bmpSource.Width * 2, bmpSource.Height * 2);

            Graphics gCursor = Graphics.FromImage(bmpCursor);

            gCursor.Clear(Color.Transparent);

            gCursor.DrawImageUnscaled(bmpSource, bmpSource.Width, bmpSource.Height);

            Rectangle pos = new Rectangle(
                m_Owner.ClientRectangle.Location,
                m_Owner.ClientRectangle.Size);

                gCursor.DrawImage(
                bmpCursor,
                pos, bmpCursor.Width / 2, bmpCursor.Width / 2,
                bmpCursor.Width, bmpCursor.Height,
                GraphicsUnit.Pixel,
                iaCursor
                );

            IntPtr ptr = bmpCursor.GetHicon();

            return new Cursor(ptr);
        }

        public void UpdateBawlCursor()
        {
            int size = (int)UI.SelectedSize;

            Bitmap bmpBawl = new Bitmap(size, size);

            Graphics gBawl = Graphics.FromImage(bmpBawl);

            gBawl.Clear(Color.Transparent);

            gBawl.FillEllipse(UI.SelectedBrush, 0, 0, size, size);
            gBawl.DrawEllipse(Pens.Black, 0, 0, size, size);


            bool current = (m_CursorCurrent == s_CursorBawl);

            if (s_CursorBawl != null)
                s_CursorBawl.Dispose();

            s_CursorBawl = CursorFromImage(bmpBawl);

            if (current)
                SetCursor(CursorBawl);
        }

        public bool Enqueue(object sender, EventArgs e)
        {
            try
            {
                m_EventQ.Enqueue(e);
                return true;
            }
            catch (Exception ex)
            {
                UI.LogMessage(ex.ToString());
                return false;
            }
        }

        public override void Update(double delta)
        {
            s_NearestEntity = World.FindNearestVehicle(Location);

            m_FSM.Update();
        }

        public void Render()
        {
        }

        public void SetCursor(Cursor c)
        {
            this.m_CursorCurrent = c;
            m_Owner.Cursor = c;
        }

        public void SetLocation(int x, int y)
        {
            m_Location.I = (double)x;
            m_Location.J = (double)y;

            m_Location = World.ToWorld(m_Location);
        }

        public override bool HandleMessage(Message msg)
        {
            return m_FSM.HandleMessage(msg);
        }

        public Queue<EventArgs> Queue
        {
            get { return m_EventQ; }
        }
       
        public Vector3D Location
        {
            get { return m_Location; }
        }

        public StateMachine<Pointer> FSM
        {
            get { return m_FSM; }
        }

        public static Cursor CursorDefault
        {
            get { return s_CursorDefault; }
        }

        public static Cursor CursorBawl
        {
            get { return s_CursorBawl; }
        }

        public static Base NearestEntity
        {
            get { return s_NearestEntity; }
        }

    }
}
