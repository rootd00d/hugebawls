using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HugeBawls.Hud;
using HugeBawls.Entities;

namespace HugeBawls.State.Pointer
{

    public class AddBawl : State<Entities.Pointer>
    {
        private enum Stage
        {
            ChoosePosition,
            SetVelocity,
            SetAcceleration,
            SetAngularVelocity,
            SetAngularAcceleration,
            AddBawl
        };

        private static AddBawl s_Instance = null;

        private static Stage s_Stage = Stage.ChoosePosition;

        private static Entities.Bawl s_Bawl = null;

        private static Vector3D s_Baseline = new Vector3D(40);

        public override void Enter(Entities.Pointer p)
        {
            //p.SetCursor(Pointer.CursorBawl);
            s_Stage = Stage.ChoosePosition;
            UI.LogMessage(s_Stage.ToString());
        }

        public override void Execute(Entities.Pointer p)
        {
            MouseEventArgs m = null;

            if (p.Queue.Count != 0) m = (MouseEventArgs)p.Queue.Dequeue();

            switch (s_Stage)
            {
                case Stage.ChoosePosition:

                    if (m == null) break;

                    s_Bawl = new Entities.Bawl(
                            World.ToWorld(new Vector3D((double)m.X, (double)m.Y, 0)),
                            new Vector3D(0, 0, 0),
                            new Vector3D(0, 0, 0),
                            new Vector3D(0, 0, 0),
                            new Vector3D(0, 0, 0),
                            UI.SelectedMass,
                            UI.SelectedPen,
                            UI.SelectedBrush,
                            UI.SelectedSize);

                    if (m.Button == MouseButtons.Left)
                    {
                        s_Stage = Stage.AddBawl;
                    }
                    else
                    {
                        s_Stage = Stage.SetVelocity;
                    }

                    break;

                case Stage.SetVelocity:

                    Vector3D v = (p.Location - s_Bawl.Position) / Settings.VelocityVectorScale;

                    v.Truncate(s_Bawl.MaxSpeed);

                    s_Bawl.Render();

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        v * Settings.VelocityVectorScale, Renderer.PenVelocity);

                    if (m == null) break;

                    if (m.Button == MouseButtons.Left)
                    {
                        s_Stage = Stage.AddBawl;
                    }
                    else if (m.Button == MouseButtons.Right)
                    {
                        s_Stage = Stage.SetAcceleration;
                    }

                    s_Bawl.Velocity = v;

                    break;

                case Stage.SetAcceleration:
                    Vector3D a = (p.Location - s_Bawl.Position) / Settings.AccelerationVectorScale;

                    a.Truncate(s_Bawl.MaxAccel);

                    s_Bawl.Render();

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.Velocity * Settings.VelocityVectorScale,
                        Renderer.PenVelocity);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        a * Settings.AccelerationVectorScale,
                        Renderer.PenAcceleration);

                    if (m == null) break;

                    if (m.Button == MouseButtons.Left)
                    {
                        s_Stage = Stage.AddBawl;
                    }
                    else if (m.Button == MouseButtons.Right)
                    {
                        s_Stage = Stage.SetAngularVelocity;
                    }

                    s_Bawl.Acceleration = a;

                    break;

                case Stage.SetAngularVelocity:
                    Vector3D av = (p.Location - s_Bawl.Position) / Settings.VelocityVectorScale;

                    av.Truncate(s_Bawl.MaxSpeed);

                    s_Bawl.Render();

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.Velocity * Settings.VelocityVectorScale,
                        Renderer.PenVelocity);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.Acceleration * Settings.AccelerationVectorScale,
                        Renderer.PenAcceleration);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        av * Settings.VelocityVectorScale,
                        Renderer.PenAngularVelocity);

                    if (m == null) break;

                    if (m.Button == MouseButtons.Left)
                    {
                        s_Stage = Stage.AddBawl;
                    }
                    else if (m.Button == MouseButtons.Right)
                    {
                        s_Stage = Stage.SetAngularAcceleration;
                    }

                    s_Bawl.AngularVelocity = av;

                    break;

                case Stage.SetAngularAcceleration:

                    Vector3D aa = (p.Location - s_Bawl.Position) / Settings.AccelerationVectorScale;

                    aa.Truncate(s_Bawl.MaxAccel);

                    s_Bawl.Render();

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.Velocity * Settings.VelocityVectorScale,
                        Renderer.PenVelocity);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.Acceleration * Settings.AccelerationVectorScale,
                        Renderer.PenAcceleration);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        s_Bawl.AngularVelocity * Settings.VelocityVectorScale,
                        Renderer.PenAngularVelocity);

                    Renderer.DrawVector(World.ToLocal(s_Bawl.Position),
                        aa * Settings.AccelerationVectorScale,
                        Renderer.PenAngularAcceleration);

                    if (m == null) break;

                    if (m.Button == MouseButtons.Left)
                    {
                        s_Stage = Stage.AddBawl;
                    }

                    s_Bawl.AngularAcceleration = aa;

                    break;

                case Stage.AddBawl:

                    s_Bawl.FSM.Change(State.Bawl.Float.Instance);

                    Manager.Instance.Register(s_Bawl);
                    World.AddVehicle(s_Bawl);

                    s_Bawl = null;

                    s_Stage = Stage.ChoosePosition;

                    break;

                default:
                    break;
            }

        }

        public override void Exit(Entities.Pointer p)
        {
            p.Queue.Clear();
        }

        public static AddBawl Instance
        {
            get
            {
                if (s_Instance == null) s_Instance = new AddBawl();
                return s_Instance;
            }
        }
    }

}