using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls.Hud
{
    public abstract class Entry<EntryType>
    {
        protected string m_Name;

        protected EntryType m_Data;

        protected Group m_Group;

        protected int m_Width;

        protected int m_Height;

        protected Brush m_Brush;

        public abstract void Render(int x, int y);

        public Entry(Group group, string name, EntryType data)
        {
            m_Group = group;
            m_Name = name;
            m_Data = data;

            m_Width = 100;
            m_Height = 20;

            m_Brush = Renderer.InitializeGradientBrush(
                group.EntryColor.Color,
                m_Group.X + m_Group.PadX,
                m_Group.Y + m_Group.PadY,
                m_Width, m_Height);
        }

        public void Update(EntryType data)
        {
            m_Data = data;
        }
                
        public string Name
        {
          get { return m_Name; }
          set { m_Name = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public EntryType Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
    }

    public class EntryGeneric : Entry<Object>
    {
        public EntryGeneric(Group group, string name, Object data)
            : base(group, name, data)
        {
            UI.LogMessage("Adding entry: {0}: {1}", name, data);
        }

        public override void Render(int x, int y)
        {
            Renderer.DrawRectangle(new Vector3D(x, y, 0), Width, Height, m_Group.Foreground, m_Brush);
            Renderer.DrawString(String.Format("{0}: {1}", m_Name, Convert.ToString(m_Data)), x, y, false, Brushes.White);
        }
    }

    public class EntryBawl : EntryGeneric
    {
        private Entities.Bawl m_Bawl;
        private int m_Key;

        public EntryBawl(Group group, string name, Bawl bawl)
        : base(group, name, bawl)
        {
            m_Bawl = bawl;

            m_Width = (int)BawlSize.Huge + m_Group.PadX;
            m_Height = (int)BawlSize.Huge + m_Group.PadY;

            m_Key = m_Bawl.GetHashCode();

            m_Brush = Renderer.InitializeGradientBrush(
                m_Group.EntryColor.Color,
                m_Group.X + m_Group.PadX,
                m_Group.Y + m_Group.PadY,
                m_Width, m_Height);

        }

        public override void Render(int x, int y)
        {
            
            Renderer.DrawEllipse(m_Key, new Vector3D(x + m_Width / 2, y + m_Height / 2, 0), (int)m_Bawl.Size, null, null);

            Renderer.DrawRectangle(new Vector3D(x, y, 0), Width, Height, m_Group.Foreground, m_Brush);

            Renderer.DrawString(
                String.Format("ID: {0}", m_Bawl.ID),
                x, y, false, Brushes.White);

            Renderer.DrawString(
                String.Format("X: {0:#}", m_Bawl.Position.I),
                x, y + 12, false, Brushes.White);

            Renderer.DrawString(
                String.Format("Y: {0:#}", m_Bawl.Position.J),
                x, y + 24, false, Brushes.White);

            Renderer.DrawString(
                String.Format("M: {0}", m_Bawl.Mass),
                x, y + 36, false, Brushes.White);
                    
        }
        
    }


}
