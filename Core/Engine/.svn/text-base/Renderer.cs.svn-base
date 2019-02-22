using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Reflection;
using System.IO;


using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls
{

    public class Renderer
    {

        private static ResourceManager s_Resources =
            new System.Resources.ResourceManager(
                "ResourceClient.Images",
                System.Reflection.Assembly.GetExecutingAssembly());

        private static Bitmap s_BackBuffer;
        private static Graphics s_Graphics;
        private static Graphics s_GraphicsBuffer;

        private static Bitmap s_bWorldViewBuffer;
        private static Graphics s_gWorldView;
        private static Graphics s_gWorldViewBuffer;

        private static Dictionary<int, Bitmap> s_BitmapCache;

        private static AdjustableArrowCap s_LineCap;

        private static Font s_FontWorld = new Font("Courier New", 8);

        private static Pen s_PenVelocity;
        private static Pen s_PenAcceleration;
        private static Pen s_PenAngularVelocity;
        private static Pen s_PenAngularAcceleration;
        private static Pen s_PenHeading;
        private static Pen s_PenPartition;

        private static Brush m_bPink;
        private static Brush m_bPurple;
        private static Brush m_bBlue;
        private static Brush m_bGreen;
        private static Brush m_bYellow;
        private static Brush m_bBlack;

        public static Pen PenVelocity { get { return s_PenVelocity; } }
        public static Pen PenAcceleration { get { return s_PenAcceleration; } }
        public static Pen PenAngularAcceleration { get { return s_PenAngularAcceleration; } }
        public static Pen PenAngularVelocity { get { return s_PenAngularVelocity; } }
        public static Pen PenHeading { get { return s_PenHeading; } }


        public static Brush Pink { get { return m_bPink; } }
        public static Brush Purple { get { return m_bPurple; } }
        public static Brush Blue { get { return m_bBlue; } }
        public static Brush Green { get { return m_bGreen; } }
        public static Brush Yellow { get { return m_bYellow; } }
        public static Brush Black { get { return m_bBlack; } }

        

        static Renderer()
        {
            s_BitmapCache = new Dictionary<int, Bitmap>(Settings.NumEntities);

            s_LineCap = new AdjustableArrowCap(3, 5, false);

            s_PenVelocity = new Pen(Brushes.Blue);
            s_PenVelocity.Width = 1;
            s_PenVelocity.EndCap = LineCap.Custom;
            s_PenVelocity.CustomEndCap = s_LineCap;

            s_PenAcceleration = new Pen(Brushes.Red);
            s_PenAcceleration.Width = 1;
            s_PenAcceleration.EndCap = LineCap.Custom;
            s_PenAcceleration.CustomEndCap = s_LineCap;

            s_PenAngularVelocity = new Pen(Brushes.PowderBlue);
            s_PenAngularVelocity.Width = 2;
            s_PenAngularVelocity.EndCap = LineCap.Custom;
            s_PenAngularVelocity.CustomEndCap = s_LineCap;

            s_PenAngularAcceleration = new Pen(Brushes.Lavender);
            s_PenAngularAcceleration.Width = 2;
            s_PenAngularAcceleration.EndCap = LineCap.Custom;
            s_PenAngularAcceleration.CustomEndCap = s_LineCap;

            s_PenHeading = new Pen(Brushes.Silver);
            s_PenHeading.Width = 1;
            s_PenHeading.EndCap = LineCap.Custom;
            s_PenHeading.CustomEndCap = s_LineCap;

            s_PenPartition = new Pen(Brushes.White);
            s_PenPartition.Width = 1;
            s_PenPartition.DashStyle = DashStyle.Dot;
            
        }

        public static LinearGradientBrush InitializeGradientBrush(Color c, int x, int y, int width, int height)
        {
            
            return new LinearGradientBrush(
                new Rectangle(x, y, width, height),
                    c,
                    Color.Transparent,
                    LinearGradientMode.Vertical);
        }

        public static void InitializeBrushes()
        {
            m_bPink = Brushes.DeepPink;
            m_bPurple = Brushes.Purple;
            m_bBlue = Brushes.RoyalBlue;
            m_bGreen = Brushes.Green;
            m_bYellow = Brushes.Yellow;
            m_bBlack = Brushes.Black;
        }

        public static void InitializeMain(PictureBox area)
        {
            if (area.Width != 0 && area.Height != 0)
            {
                s_BackBuffer = new Bitmap(area.Width, area.Height);

                s_GraphicsBuffer = Graphics.FromImage(s_BackBuffer);

                s_Graphics = area.CreateGraphics();

                s_GraphicsBuffer.SmoothingMode = SmoothingMode.AntiAlias;
                s_Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                //UI.LogMessage("Main({0}x{1})", area.Width, area.Height);
            }

            InitializeBrushes();
        }

        public static void InitializeWorld(PictureBox area)
        {
            if (area.Width != 0 && area.Height != 0)
            {
                s_bWorldViewBuffer = new Bitmap(area.Width, area.Height);

                s_gWorldViewBuffer = Graphics.FromImage(s_bWorldViewBuffer);

                s_gWorldView = area.CreateGraphics();

                s_gWorldViewBuffer.SmoothingMode = SmoothingMode.AntiAlias;
                s_gWorldView.SmoothingMode = SmoothingMode.AntiAlias;

                //UI.LogMessage("World({0}x{1})", area.Width, area.Height);
            }

            InitializeBrushes();
        }


        public static void Render()
        {
            World.Render();

            if (UI.ShowGravity)
                HighlightCenterOfGravity();

            UI.Pointer.Render();

            Hud.Control.Instance.Render();

            MiniMapDrawView();

            SwapGraphicsBuffer();

            ClearGraphicsBuffer();

            WorldViewSwapGraphicsBuffer();
            
            WorldViewClearGraphicsBuffer();

        }

        public static void HighlightCenterOfGravity()
        {
            DrawRectangle(World.ToLocal(Steering.CenterOfGravity), 5, 5, Pens.Black, Brushes.Aqua);

        }

        public static void HighlightNearestBawl()
        {
            Bawl nearest = (Bawl)Entities.Pointer.NearestEntity;

            if (nearest != null)
            {
                Renderer.DrawEllipse(
                    nearest.Position,
                    (int)nearest.Size + 6,
                    Pens.Black,
                    Brushes.Silver);
            }
        }

        public static void SwapGraphicsBuffer()
        {
            s_Graphics.DrawImageUnscaled(s_BackBuffer, 0, 0);
        }

        public static void ClearGraphicsBuffer()
        {
            s_GraphicsBuffer.Clear(Color.Black);
        }

        public static Image GetImageResource(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            Stream stream = asm.GetManifestResourceStream(name);

            return Image.FromStream(stream);
        }

        public static void DrawStars()
        {
            foreach (Partition<Vehicle> partition in World.VisiblePartitions)
            {
                foreach (Vehicle vehicle in partition.Members)
                {
                    Star star = vehicle as Star;

                    if (star == null) continue;

                    if (star.Visible) star.Render();
                }
            }
        }

        public static void DrawStar(int ID, Vector3D global, int height)
        {
            Vector3D pos = World.ToLocal(global);

            int y;

            if (s_BitmapCache.ContainsKey(ID))
            {
                Bitmap b = s_BitmapCache[ID];

                int x = (int)(pos.I - (double)b.Width / 2);

                y = (int)(pos.J - b.Height / 2);

                s_GraphicsBuffer.DrawImageUnscaled(s_BitmapCache[ID], x, y);
            }
            else
            {
                Image img = GetImageResource("HugeBawls.Resources.Star2.png");

                Bitmap bmpOrig = new Bitmap(img);

                double xScale = (double)height / (double)bmpOrig.Height;

                Bitmap bmp = new Bitmap((int)(xScale * bmpOrig.Width), height);

                Graphics g = Graphics.FromImage(bmp);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.DrawImage(bmpOrig, 0, 0, bmp.Width, bmp.Height);

                int x = (int)(pos.I - (double)bmp.Width / 2);

                y = (int)(pos.J - height / 2);

                s_GraphicsBuffer.DrawImageUnscaled(bmp, x, y);

                s_BitmapCache.Add(ID, bmp);

                UI.LogMessage("Caching bitmap: {0}", ID);
            }
        }

        public static void DrawHuman(int ID, Vector3D pos, double height)
        {            
            int y = (int)(pos.J - height / 2);

            if (s_BitmapCache.ContainsKey(ID))
            {
                Bitmap b = s_BitmapCache[ID];

                int x = (int)(pos.I - (double)b.Width / 2);

                s_GraphicsBuffer.DrawImageUnscaled(s_BitmapCache[ID], x, y);
            }
            else
            {
                Image img = GetImageResource("HugeBawls.Resources.Human.png");

                Bitmap bmpOrig = new Bitmap(img);

                double xScale = (double)height / (double)bmpOrig.Height;

                Bitmap bmp = new Bitmap((int)(xScale * bmpOrig.Width), (int)height);
                
                Graphics g = Graphics.FromImage(bmp);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.DrawImage(bmpOrig, 0, 0, bmp.Width, bmp.Height);

                int x = (int)(pos.I - (double)bmp.Width / 2);

                s_GraphicsBuffer.DrawImageUnscaled(bmp, x, y);

                s_BitmapCache.Add(ID, bmp);

                UI.LogMessage("Caching bitmap: {0}", ID);
            }
        }

        public static void DrawEllipse(Vector3D pos, int size, Pen p, Brush b)
        {
            double rad = size / 2;

            int x = (int)(pos.I - rad);
            int y = (int)(pos.J - rad);

            s_GraphicsBuffer.FillEllipse(b, (float)x, (float)y, size, size);

            s_GraphicsBuffer.DrawEllipse(p, (float)x, (float)y, size, size);            
        }

        public static void DrawEllipse(int ID, Vector3D pos, int size, Pen p, Brush b)
        {
            double rad = size / 2;

            int x = (int)(pos.I - rad);
            int y = (int)(pos.J - rad);

            if (s_BitmapCache.ContainsKey(ID))
            {
                s_GraphicsBuffer.DrawImageUnscaled(s_BitmapCache[ID], x, y);
            }
            else
            {
                Bitmap bmp = new Bitmap(size + 1, size + 1);
                
                Graphics g = Graphics.FromImage(bmp);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.Clear(Color.Transparent);

                Brush newBrush = b;

                if (UI.ShowGradient)
                {
                    Pen tmpPen = new Pen(b);
                    newBrush = InitializeGradientBrush(tmpPen.Color, 0, 0, bmp.Width, bmp.Height);
                }

                g.FillEllipse(newBrush, 0, 0, size, size);

                //g.DrawEllipse(p, 0, 0, size, size);

                s_GraphicsBuffer.DrawImageUnscaled(bmp, x, y);

                s_BitmapCache.Add(ID, bmp);

                UI.LogMessage("Caching bitmap: {0}", ID);
            }
        }

        public static void DrawRectangle(Vector3D pos, int width, int height, Pen p, Brush b)
        {/*
            Rectangle rect = new Rectangle(
                new Point((int)pos.I - width / 2, (int)pos.J - height / 2),
                new Size(width, height)
                );
           */

            Rectangle rect = new Rectangle(
                new Point((int)pos.I, (int)pos.J),
                new Size(width, height)
                );

            s_GraphicsBuffer.FillRectangle(b, rect);
            s_GraphicsBuffer.DrawRectangle(p, rect);
        }

        public static void DrawVector(Vector3D origin, Vector3D vector, Pen pen)
        {   
            s_GraphicsBuffer.DrawLine(pen, origin, origin + vector);
        }

        public static void DrawPartitionLine(float x1, float y1, float x2, float y2)
        {
            s_GraphicsBuffer.DrawLine(s_PenPartition, x1, y1, x2, y2);
        }

        public static void DrawString(string msg, float x, float y, bool vertical, Brush b)
        {
            s_GraphicsBuffer.DrawString(msg, s_FontWorld, b, x, y);
        }


        public static void MiniMapDrawBawl(Bawl bawl)
        {
            s_gWorldViewBuffer.DrawRectangle(
                bawl.Pen,
                (float)(bawl.Position.I / World.MiniMapRatioX),
                (float)(bawl.Position.J / World.MiniMapRatioY),
                1, 1);
            
        }

        public static void MiniMapDrawView()
        {
            float x = (float)(World.Origin.I / World.MiniMapRatioX);
            float y = (float)(World.Origin.J / World.MiniMapRatioY);
            float i = (float)(World.ViewSize.I / World.MiniMapRatioX);
            float j = (float)(World.ViewSize.J / World.MiniMapRatioY);

            s_gWorldViewBuffer.DrawRectangle(Pens.Silver, x, y, i, j);
        }

        public static void WorldViewSwapGraphicsBuffer()
        {
            s_gWorldView.DrawImageUnscaled(s_bWorldViewBuffer, 0, 0);
        }

        public static void WorldViewClearGraphicsBuffer()
        {
            s_gWorldViewBuffer.Clear(Color.Black);
        }




    }
}
