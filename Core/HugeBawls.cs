using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

using System.Runtime;

using HugeBawls;
using HugeBawls.Entities;
using HugeBawls.Hud;

namespace HugeBawls
{

    static class HugeBawls
    {

        public static bool DEBUG = true;

        private static Regulator s_UpdateRegulator;
        private static Regulator s_RenderRegulator;

        private static DateTime s_TimeLast;
        private static TimeSpan s_TimeDelta;

        private static bool s_Running = false;

        private static EntryGeneric s_HudEnts;
        private static EntryGeneric s_HudFPS;

        private static Random s_Random = new Random();

        public static void Initialize()
        {
            s_TimeLast = DateTime.Now;

            s_UpdateRegulator = new Regulator(Settings.PeriodUpdate);
            s_RenderRegulator = new Regulator(Settings.PeriodRender);

            s_HudEnts = Hud.Control.Instance.AddGeneric("Profile", "# Entities", 0);

            s_HudFPS = Hud.Control.Instance.AddGeneric("Engine", "FPS", 0);

            World.InitializeStars();

            Run();
        }

        public static void Reset()
        {
            Pause();

            Manager.Reset();

            World.InitializePartitions();

            World.InitializeStars();

            Run();
        }

        public static void Update(double delta)
        {
            DateTime now = DateTime.Now;

            s_TimeDelta = now - s_TimeLast;

            s_HudFPS.Update(1000 / s_TimeDelta.Milliseconds);

            s_TimeLast = now;

            int deltaT = Math.Min(s_TimeDelta.Milliseconds, 30);

            //if (s_UpdateRegulator.Ready())
            //{
                //Dispatcher.Instance.DelayedDischarge();

                Steering.ComputeCenterOfGravity();

                UI.Pointer.Update(delta);

                foreach (Vehicle b in Manager.Instance.Registered())
                {
                    //b.Update((double)s_UpdateRegulator.Interval());
                    //b.Update(s_TimeDelta.Milliseconds);
                    b.Update(deltaT);
                }

                Steering.ResolveCollisions();
            //}

            //if (s_RenderRegulator.Ready())
            //{
                Renderer.Render();
            //}

                
        }

        public static void ScaleUpdate(double scale)
        {
            HugeBawls.Pause();
            
            s_UpdateRegulator.Disable();

            s_UpdateRegulator.Dispose();
            
            s_UpdateRegulator = new Regulator((int)(Settings.PeriodUpdate * scale));

            HugeBawls.Run();
        }

        public static void ScaleRenderer(double scale)
        {
            HugeBawls.Pause();

            s_RenderRegulator.Disable();

            s_RenderRegulator.Dispose();

            s_RenderRegulator = new Regulator((int)(Settings.PeriodRender * scale));

            HugeBawls.Run();
        }

        public static void Run()
        {
            s_UpdateRegulator.Enable();
            s_RenderRegulator.Enable();

            s_Running = true;
        }

        public static void Pause()
        {
            s_UpdateRegulator.Disable();

            s_Running = false;
        }

        public static List<Type> GetTypes(string name)
        {
            List<Type> types = new List<Type>();
            
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                if (type.Namespace == name)
                    types.Add(type);

            return types;
        }

        public static int CountTypes(string name)
        {
            return GetTypes(name).Count;
        }

        public static void Randomize()
        {
            for (int i = 0; i < 2000; i++)
            {
                double r = s_Random.NextDouble();
            
                Bawl b = new Bawl(
                    new Vector3D(s_Random.Next((int)Settings.WorldSize.I), s_Random.Next((int)Settings.WorldSize.J), 0),
                    new Vector3D(s_Random.NextDouble(), s_Random.NextDouble(), 0),
                    new Vector3D(0, 0, 0),
                    new Vector3D(0, 0, 0),
                    new Vector3D(0, 0, 0),
                    0,
                    Pens.Yellow,
                    Renderer.Yellow,
                    0);

                if (r < 0.01)
                {
                    b.Mass = BawlMass.Huge;
                    b.Size = BawlSize.Huge;
                    b.Brush = Brushes.Yellow;
                }
                else if (r < 0.2)
                {
                    b.Mass = BawlMass.Large;
                    b.Size = BawlSize.Large;
                    b.Brush = Brushes.Purple;
                }
                else if (r < 0.6)
                {
                    b.Mass = BawlMass.Medium;
                    b.Size = BawlSize.Medium;
                    b.Brush = Brushes.Green;
                }
                else
                {
                    b.Mass = BawlMass.Small;
                    b.Size = BawlSize.Small;
                    b.Brush = Brushes.Pink;
                }


                b.FSM.Change(State.Bawl.Float.Instance);

                Manager.Instance.Register(b);
                World.AddVehicle(b);
            }
        }

        public static Regulator UpdateRegulator
        {
            get { return s_UpdateRegulator; }
        }

        public static Regulator RenderRegulator
        {
            get { return s_RenderRegulator; }
        }

        public static bool Running
        {
            get { return s_Running; }
        }


        public static TimeSpan TimeDelta
        {
            get { return s_TimeDelta; }
        }

        public static Random RandomNumber
        {
            get { return s_Random; }
        }

        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UI());
        }
    }
}