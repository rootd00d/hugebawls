using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HugeBawls.Hud
{
    public class Group
    {
        private string m_Name;
        private int m_X;
        private int m_Y;
        private int m_Width;
        private int m_Height;

        private Pen m_Fore;
        private Pen m_Back;
        private Pen m_Entry;
        private Brush m_Brush;

        private bool m_Vertical;

        private static int m_PadX = 5;
        private static int m_PadY = 5;

        private List<EntryGeneric> m_Entries;

        public Group(
            string name,
            int x,
            int y,
            Pen foreColor,
            Pen backColor,
            Pen entryColor,
            bool vertical)
        {
            m_Name = name;

            m_X = x;
            m_Y = y;

            m_Width = m_PadX * 2;
            m_Height = m_PadY * 2;

            m_Fore = foreColor;
            m_Back = backColor;
            m_Entry = entryColor;

            m_Vertical = vertical;

            m_Brush = null;

            m_Entries = new List<EntryGeneric>();
        }

        public void Add(EntryGeneric e)
        {
            m_Entries.Add(e);

            Refresh();
        }

        private void Refresh()
        {
            int total = 0;
            int maxWidth = 0;
            int maxHeight = 0;

            foreach (EntryGeneric entry in m_Entries)
            {
                maxWidth = Math.Max(maxWidth, entry.Width);
                maxHeight = Math.Max(maxHeight, entry.Height);

                if (m_Vertical)
                {
                    total += entry.Height;
                }
                else
                {
                    total += entry.Width;
                }
            }

            if (m_Vertical)
            {
                m_Height = total + 2 * m_PadY;
                m_Width = maxWidth + 2 * m_PadX;
            }
            else
            {
                m_Height = maxHeight + 2 * m_PadY;
                m_Width = total + 2 * m_PadX;
            }

            m_Brush = Renderer.InitializeGradientBrush(m_Back.Color, m_X, m_Y, m_Width, m_Height);
        }


        public EntryGeneric GetEntry(string name)
        {
            foreach (EntryGeneric entry in m_Entries)
            {
                if (entry.Name == name)
                    return entry;
            }

            return null;
        }

        public void Remove(EntryGeneric e)
        {
            if (!m_Entries.Contains(e))
            {
                return;
            }

            m_Entries.Remove(e);

            Refresh();
        }

        public int PadX
        {
            get { return m_PadX; }
        }

        public int PadY
        {
            get { return m_PadY; }
        }

        public int X
        {
            get { return m_X; }
        }

        public int Y
        {
            get { return m_Y; }
        }

        public void Render()
        {
            
            Renderer.DrawRectangle(new Vector3D(m_X, m_Y, 0), m_Width, m_Height, m_Fore, m_Brush);
            
            Renderer.DrawString(m_Name, m_X + 5, m_Y + m_Height, false, Brushes.White);

            int x = m_X + m_PadX;
            int y = m_Y + m_PadY;

            foreach (EntryGeneric e in m_Entries)
            {
                e.Render(x, y);

                if (m_Vertical)
                {
                    y += e.Height;
                }
                else
                {
                    x += e.Width;
                }

            }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public int Width
        {
            get { return m_Width; }
        }

        public int Height
        {
            get { return m_Height; }
        }

        public Pen Foreground
        {
            get { return m_Fore; }
        }

        public Pen Background
        {
            get { return m_Back; }
        }

        public Pen EntryColor
        {
            get { return m_Entry; }
        }
    }
}


