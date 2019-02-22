using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Reflection;
using System.Diagnostics;

using HugeBawls;
using HugeBawls.Entities;
using HugeBawls.Hud;

namespace HugeBawls
{
    public partial class UI : Form
    {
        #region Members

        private static bool s_ShowVelocity = Settings.ShowVelocity;
        private static bool s_ShowAcceleration = Settings.ShowAcceleration;
        private static bool s_ShowHeading = Settings.ShowHeading;

        private static bool s_ShowNearest = true;
        private static bool s_ShowGravity = true;
        private static bool s_ShowGradient = true;
        

        private static Pen s_Pen;
        private static Brush s_Brush;
        private static BawlSize s_Size;
        private static double s_Mass;

        private static Entities.Pointer s_Pointer;

        private static RichTextBox s_TextBox;

        #endregion Members

        #region Constructors

        public UI()
        {
            InitializeComponent();

            s_TextBox = txtConsole;

            this.SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.DoubleBuffer, true);

            Initialize();
            
            HugeBawls.Initialize();

            SetDefaults();

            InitializeToolbox();

            s_Pointer = new Entities.Pointer(this, btnCursor.Image);

            pbxMain.Cursor = s_Pointer.CursorFromImage(btnCursor.Image);
        }

        #endregion Constructors

        #region Methods

        void Initialize()
        {
            World.Initialize(
                Settings.WorldSize,
                new Vector3D((double)pbxMain.Width, (double)pbxMain.Height, 0),
                new Vector3D((double)pbxWorld.Width, (double)pbxWorld.Height, 0),
                new Vector3D(
                    (double)Settings.WorldSize.I / 2 - (double)pbxMain.Width / 2,
                    (double)Settings.WorldSize.J / 2 - (double)pbxMain.Height / 2,
                    0)
            );

            Renderer.InitializeMain(pbxMain);
            Renderer.InitializeWorld(pbxWorld);
        }

        public void SetDefaults()
        {
            EnableTools();
            EnableColors();
            EnableSizes();

            UI.SelectedBrush = Renderer.Pink;
            UI.SelectedSize = BawlSize.Small;
            UI.SelectedMass = (double)BawlMass.Small;

            txtMass.Text = UI.SelectedMass.ToString();

            btnSmall.Enabled = false;
        }


        public void InitializeToolbox()
        {
            int x = 14;
            int y = 14;
            int rad = (int)(x / 2);

            Bitmap bmpBawl = new Bitmap(16, 16);

            Graphics gBawl = Graphics.FromImage(bmpBawl);

            gBawl.Clear(Color.Transparent);

            gBawl.FillEllipse(Renderer.Blue, 0, 0, x, y);
            gBawl.DrawEllipse(Pens.Black, 0, 0, x, y);

            btnBawl.Image.Dispose();

            btnBawl.Image = bmpBawl;
        }


        public void SetOrigin(int x, int y)
        {
            Vector3D o = new Vector3D(0);

            o.I = ((double)x * World.MiniMapRatioX) - (World.ViewSize.I / 2) / World.MiniMapRatioX;
            o.J = ((double)y * World.MiniMapRatioY) - (World.ViewSize.J / 2) / World.MiniMapRatioY;

            World.SetOrigin(o);
        }


        public void SetBrush(Brush b)
        {
            UI.LogMessage("Brush = {0}", b.ToString());
            UI.SelectedBrush = b;
        }



        private static string Caller()
        {
            StackTrace st = new StackTrace(1, true);

            try
            {
                return (
                    st.GetFrame(1).GetMethod().DeclaringType.Name +
                    "::" + st.GetFrame(1).GetMethod().Name
                );
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public static void LogMessage(string fmt, params object[] args)
        {
            string msg = String.Format(fmt, args);

            msg = msg.Insert(0, "(" + Caller() + "): ");

            s_TextBox.AppendText(msg + "\n");

            s_TextBox.ScrollToCaret();
        }

        public void Clear()
        {
            s_TextBox.Clear();
        }



        private void EnableTools()
        {
            btnCursor.Checked = false;
            btnBawl.Checked = false;
            btnOrbital.Checked = false;
        }

        private void EnableColors()
        {
            btnPurple.Enabled = true;
            btnPink.Enabled = true;
            btnBlue.Enabled = true;
            btnGreen.Enabled = true;
            btnYellow.Enabled = true;
            btnBlack.Enabled = true;
        }

        private void EnableSizes()
        {
            btnSmall.Enabled = true;
            btnMedium.Enabled = true;
            btnLarge.Enabled = true;
            btnHuge.Enabled = true;
        }



        void ScaleUpdate(double scale)
        {
            HugeBawls.ScaleUpdate(scale);

            tbUpdate.Value = (int)(tbUpdate.Maximum / scale);

            txtUpdate.Text = String.Format("{0}", HugeBawls.UpdateRegulator.Interval());
        }

        void ScaleRenderer(double scale)
        {
            HugeBawls.ScaleRenderer(scale);

            tbRenderer.Value = (int)(tbRenderer.Maximum / scale);
             
            txtRenderer.Text = String.Format("{0}", HugeBawls.RenderRegulator.Interval());
        }


        void ScaleGravityMultiplier(double scale)
        {
            Steering.GravityMultiplier = Settings.ScaleGravity * scale;

            tbGravity.Value = (int)(tbGravity.Maximum * scale);

            txtGravity.Text = String.Format("{0:00e+0}", Steering.GravityMultiplier);
        }

        void ScaleVelocityMultiplier(double scale)
        {
            Steering.VelocityMultiplier = Settings.ScaleVelocity * scale;

            tbVelocity.Value = (int)(tbVelocity.Maximum * scale);

            txtVelocity.Text = String.Format("{0:00e+0}", Steering.VelocityMultiplier);
        }

        void ScaleAccelerationMultiplier(double scale)
        {
            Steering.AccelerationMultiplier = Settings.ScaleAcceleration * scale;

            tbAcceleration.Value = (int)(tbAcceleration.Maximum * scale);

            txtAcceleration.Text = String.Format("{0:00e+0}", Steering.AccelerationMultiplier);
        }

        #endregion Methods

        #region CallBacks



        void UI_Load(object sender, System.EventArgs e)
        {
            ScaleUpdate(1);
            ScaleRenderer(1);
            ScaleGravityMultiplier(1);
            ScaleVelocityMultiplier(1);
            ScaleAccelerationMultiplier(1);

            Assembly asm = Assembly.GetExecutingAssembly();
            
            string[] names = asm.GetManifestResourceNames();

            foreach (string name in names)
            {
                UI.LogMessage(name);
            } 


        }

        void pbxMain_Resize(object sender, System.EventArgs e)
        {
            this.Initialize();

            Renderer.InitializeMain(pbxMain);
        }

        void pbxWorld_Resize(object sender, System.EventArgs e)
        {
            this.Initialize();

            Renderer.InitializeWorld(pbxWorld);
        }


        private void pbxMain_Click(object sender, EventArgs e)
        {
            if (!Pointer.Enqueue(sender, e))
            {
                UI.LogMessage("Failed to queue pointer event");
            }

        }


        void pbxWorld_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;

            if (args.Button == MouseButtons.Left)
            {
                SetOrigin(args.X, args.Y);
            }
        }



        private void pbxMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;

            Pointer.SetLocation(args.X, args.Y);

            tssPointerX.Text = String.Format("X: {0}", (int)Pointer.Location.I);
            tssPointerY.Text = String.Format("Y: {0}", (int)Pointer.Location.J);
        }


        public void pbxWorld_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;

            if (e.Button == MouseButtons.Left)
            {
                SetOrigin(args.X, args.Y);
            }

        }

        private void btnPurple_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Purple;
            EnableColors();
            btnPurple.Enabled = false;
        }

        private void btnPink_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Pink;
            EnableColors();
            btnPink.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Blue;
            EnableColors();
            btnBlue.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Green;
            EnableColors();
            btnGreen.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Yellow;
            EnableColors();
            btnYellow.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            UI.SelectedBrush = Renderer.Black;
            EnableColors();
            btnBlack.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnSmall_Click(object sender, EventArgs e)
        {
            UI.SelectedSize = BawlSize.Small;
            UI.SelectedMass = BawlMass.Small;

            txtMass.Text = UI.SelectedMass.ToString();

            EnableSizes();
            btnSmall.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnMedium_Click(object sender, EventArgs e)
        {
            UI.SelectedSize = BawlSize.Medium;
            UI.SelectedMass = BawlMass.Medium;

            txtMass.Text = UI.SelectedMass.ToString();

            EnableSizes();
            btnMedium.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnLarge_Click(object sender, EventArgs e)
        {
            UI.SelectedSize = BawlSize.Large;
            UI.SelectedMass = BawlMass.Large;

            txtMass.Text = UI.SelectedMass.ToString();

            EnableSizes();
            btnLarge.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void btnHuge_Click(object sender, EventArgs e)
        {
            UI.SelectedSize = BawlSize.Huge;
            UI.SelectedMass = BawlMass.Huge;

            txtMass.Text = UI.SelectedMass.ToString();

            EnableSizes();
            btnHuge.Enabled = false;
            Pointer.UpdateBawlCursor();
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            HugeBawls.Update((double)timerMain.Interval);
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (HugeBawls.Running)
            {
                HugeBawls.Pause();
                btnPlayPause.Text = "&Play";
            }
            else
            {
                HugeBawls.Run();
                btnPlayPause.Text = "Sto&p";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            HugeBawls.Reset();

            txtConsole.Clear();
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
        }

        private void miGravity_Click(object sender, EventArgs e)
        {
            miGravity.Checked = !miGravity.Checked;
            UI.ShowGravity = miGravity.Checked;
        }

        private void miNearest_Click(object sender, EventArgs e)
        {
            miNearest.Checked = !miNearest.Checked;
            UI.ShowNearest = miNearest.Checked;
        }

        private void miVelocity_Click(object sender, EventArgs e)
        {
            miVelocity.Checked = !miVelocity.Checked;
            UI.ShowVelocity = miVelocity.Checked;
        }

        private void miAcceleration_Click(object sender, EventArgs e)
        {
            miAcceleration.Checked = !miAcceleration.Checked;
            UI.ShowAcceleration = miAcceleration.Checked;
        }

        private void miGradient_Click(object sender, EventArgs e)
        {
            miGradient.Checked = !miGradient.Checked;
            UI.ShowGradient = miGradient.Checked;
        }


        private void miHeading_Click(object sender, EventArgs e)
        {
            miHeading.Checked = !miHeading.Checked;
            UI.ShowHeading = miHeading.Checked;

        }

        void txtMass_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                UI.SelectedMass = Convert.ToDouble(txtMass.Text);
                UI.LogMessage("(M={0})", UI.SelectedMass);
            }
            catch (Exception)
            {
                UI.SelectedMass = BawlMass.Small;
                UI.LogMessage("(M={0})", UI.SelectedMass);
            }

        }

        private void btnCursor_Click(object sender, EventArgs e)
        {
            EnableTools();
            btnCursor.Checked = true;
            Pointer.FSM.Change(State.Pointer.Select.Instance);
        }

        private void btnBawl_Click(object sender, EventArgs e)
        {
            EnableTools();
            btnBawl.Checked = true;
            Pointer.FSM.Change(State.Pointer.AddBawl.Instance);
        }

        private void btnHuman_Click(object sender, EventArgs e)
        {
            EnableTools();
            btnHuman.Checked = true;
            Pointer.FSM.Change(State.Pointer.AddHuman.Instance);
        }


        void tbUpdate_ValueChanged(object sender, System.EventArgs e)
        {
            ScaleUpdate((double)tbUpdate.Maximum / (double)tbUpdate.Value);
        }


        void tbRenderer_ValueChanged(object sender, System.EventArgs e)
        {
            ScaleRenderer((double)tbRenderer.Maximum / (double)tbRenderer.Value);
        }


        void tbGravity_ValueChanged(object sender, System.EventArgs e)
        {
            ScaleGravityMultiplier((double)tbGravity.Value / (double)tbGravity.Maximum);
        }
            
        void tbVelocity_ValueChanged(object sender, System.EventArgs e)
        {
            ScaleVelocityMultiplier((double)tbVelocity.Value / (double)tbVelocity.Maximum);
        }

        void tbAcceleration_ValueChanged(object sender, System.EventArgs e)
        {
            ScaleAccelerationMultiplier((double)tbAcceleration.Value / (double)tbAcceleration.Maximum);
        }


        #endregion CallBacks

        #region Properties

        public static Entities.Pointer Pointer
        {
            get { return s_Pointer; }
        }

        public static bool ShowVelocity
        {
            get { return s_ShowVelocity; }
            set { s_ShowVelocity = value; }
        }

        public static bool ShowAcceleration
        {
            get { return s_ShowAcceleration; }
            set { s_ShowAcceleration = value; }
        }

        public static bool ShowNearest
        {
            get { return s_ShowNearest; }
            set { s_ShowNearest = value; }
        }

        public static bool ShowGravity
        {
            get { return s_ShowGravity; }
            set { s_ShowGravity = value; }
        }

        public static bool ShowGradient
        {
            get { return s_ShowGradient; }
            set { s_ShowGradient = value; }
        }

        public static bool ShowHeading
        {
            get { return UI.s_ShowHeading; }
            set { UI.s_ShowHeading = value; }
        }

        public static Pen SelectedPen
        {
            get { return s_Pen; }
        }

        public static Brush SelectedBrush
        {
            get { return s_Brush; }
            set
            {
                s_Brush = value;
                s_Pen = new Pen(UI.SelectedBrush);
            }
        }

        public static BawlSize SelectedSize
        {
            get { return s_Size; }
            set { s_Size = value; }
        }

        public static double SelectedMass
        {
            get { return s_Mass; }
            set { s_Mass = value; }
        }

        #endregion Properties

        private void followToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pointer.FSM.Change(State.Pointer.Follow.Instance);
        }

        private void tlsRandomize_Click(object sender, EventArgs e)
        {
            HugeBawls.Reset();

            HugeBawls.Randomize();
        }


    }

}