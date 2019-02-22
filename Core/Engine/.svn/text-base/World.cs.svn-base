using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls
{
    class World
    {
        // PERF: These would otherwise be encapsulated
        public static Vector3D GameSize = Settings.WorldSize;
        public static Vector3D ViewSize;
        public static Vector3D Origin;
        public static Vector3D MiniSize;

        public static Partition<Vehicle>[] CurrentNeighbours =
            new Partition<Vehicle>[9];

        public static List<Partition<Vehicle>> VisiblePartitions = null;

        public static Dictionary<string, List<Vehicle>> VisibleEntities = null;

        private static double s_MaxNeighbourDistance;

        public static double MiniMapRatioX;
        public static double MiniMapRatioY;

        private static int s_NumPartitions = Settings.WorldPartitions;

        private static int s_PartitionSizeI = (int)(GameSize.I / s_NumPartitions);
        private static int s_PartitionSizeJ = (int)(GameSize.J / s_NumPartitions);

        private static Partition<Vehicle>[,] s_Partitions;

        private static Vehicle[] s_Stars;

        static World()
        {
            InitializePartitions();
        }

        public static void InitializePartitions()
        {
            s_Partitions = new Partition<Vehicle>[s_NumPartitions, s_NumPartitions];

            for (int i = 0; i < s_NumPartitions; i++)
            {
                for (int j = 0; j < s_NumPartitions; j++)
                {
                    s_Partitions[i, j] =
                        new Partition<Vehicle>(i, j, s_PartitionSizeI, s_PartitionSizeJ);
                }
            }

            VisibleEntities = new Dictionary<string, List<Vehicle>>();

            foreach (Type type in HugeBawls.GetTypes("HugeBawls.Entities"))
                VisibleEntities.Add(
                    type.Name,
                    new List<Vehicle>(Settings.NumEntities / (Settings.WorldPartitions * Settings.WorldPartitions)));
        }

        public static void InitializeStars()
        {
            Star star = null;

            s_Stars = new Star[Settings.StarsCount];

            for (int i = 0; i < Settings.StarsCount; i++)
            {
                Vector3D pos = new Vector3D(
                    HugeBawls.RandomNumber.NextDouble() * World.GameSize.I,
                    HugeBawls.RandomNumber.NextDouble() * World.GameSize.J,
                    0);

                int size;
                 
                if (HugeBawls.RandomNumber.NextDouble() < Settings.StarsAverage)
                    size = HugeBawls.RandomNumber.Next(Settings.StarsMinSize, Settings.StarsStandardSize);
                else
                    size = HugeBawls.RandomNumber.Next(Settings.StarsMinSize, Settings.StarsMaxSize);

                double speed = Settings.StarsMaxVelocity * (double)(size) / Settings.StarsMaxSize;

                if (HugeBawls.RandomNumber.NextDouble() < Settings.StarsShooting)
                {
                    star = new Star(pos,
                        new Vector3D(
                            (HugeBawls.RandomNumber.NextDouble() > 0.5 ? -1 : 1) * speed,
                            (HugeBawls.RandomNumber.NextDouble() > 0.5 ? -1 : 1) * speed,
                            0),
                            size);

                    Manager.Instance.Register(star);
                }
                else
                {
                    star = new Star(pos,
                        new Vector3D(0),
                        size);
                }

                s_Stars[i] = star;
    
                World.AddVehicle(star);
            }
        }


        public static void
            Initialize(Vector3D sizeGame, Vector3D sizeView, Vector3D sizeMini, Vector3D posOrigin)
        {
            GameSize = sizeGame;
            ViewSize = sizeView;
            MiniSize = sizeMini;
            Origin = posOrigin;

            MiniMapRatioX = GameSize.I / MiniSize.I;
            MiniMapRatioY = GameSize.J / MiniSize.J;

            s_MaxNeighbourDistance = Math.Max(s_PartitionSizeI, s_PartitionSizeJ) * 3;

            VisiblePartitions = new List<Partition<Vehicle>>((int)(ViewSize.I / s_PartitionSizeI * ViewSize.J  / s_PartitionSizeJ));
        }

        public static void AddVehicle(Vehicle vehicle)
        {
            vehicle.Partition = GetVehiclePartition(vehicle.Position);
            vehicle.Partition.Add(vehicle);
        }


        public static Partition<Vehicle>
            GetVehiclePartition(Vector3D position)
        {
            int i = (int)(position.I / s_PartitionSizeI);
            int j = (int)(position.J / s_PartitionSizeJ);

            return s_Partitions[i, j];
        }


        public static void
            PrepareVehicleNeighbours(Partition<Vehicle> partition)
        {
            int n = 0;

            for (int i = partition.Row - 1;
                i <= partition.Row + 1;
                i++)
            {
                int row = (i + s_NumPartitions) % s_NumPartitions;

                for (int j = partition.Column - 1;
                    j <= partition.Column + 1;
                    j++)
                {
                    int col = (j + s_NumPartitions) % s_NumPartitions;

                    CurrentNeighbours[n++] = s_Partitions[row, col];
                }
            }
        }

        public static Partition<Vehicle>
            VehicleReport(Vehicle vehicle)
        {
            vehicle.Position.I = (vehicle.Position.I + GameSize.I) % GameSize.I;
            vehicle.Position.J = (vehicle.Position.J + GameSize.J) % GameSize.J;

            Partition<Vehicle> current = GetVehiclePartition(vehicle.Position);

            if (vehicle.Partition == current)
            {
                return current;
            }
            else
            {
                if (vehicle.Partition != null)
                {
                    vehicle.Partition.Remove(vehicle);
                }

                current.Add(vehicle);
            }

            return current;
        }

        public static Vector3D NeighbourDirection(Vector3D p1, Vector3D p2)
        {   
            Vector3D rel = p2 - p1;

            double distX = Math.Abs(rel.I);
            double distY = Math.Abs(rel.J);

            if (distX > s_MaxNeighbourDistance)
            {
                // v1 and v2 are at the left and right edges of the world

                if (distX < 0)
                {
                    // v1(R) v2(L) -> v1(L) v2(R)

                    rel.I = GameSize.I - p2.I;
                }
                else
                {
                    // v1(L) v2(R) -> v1(R) v1(L)

                    rel.I = GameSize.I - p1.I;
                }
            }

            if (distY > s_MaxNeighbourDistance)
            {
                // v1 and v2 are at the top and bottom edges of the world

                if (distY < 0)
                {
                    // v1(B) v2(T) -> v1(T) v2(B)

                    rel.J = GameSize.J - p2.J;
                }
                else
                {
                    rel.J = GameSize.J - p1.J;
                }
            }

            return rel;
        }

        public static void SetOrigin(Vector3D o)
        {
            if (o.I < 0)
            {
                o.I = 0;
            }
            else if (o.I > (GameSize.I - ViewSize.I))
            {
                o.I = GameSize.I - ViewSize.I;
            }

            if (o.J < 0)
            {
                o.J = 0;
            }
            else if (o.J > (GameSize.J - ViewSize.J))
            {
                o.J = GameSize.J - ViewSize.J;
            }

            Origin = o;
        }

        public static Vehicle FindNearestNeighbour(Vector3D pos)
        {
            Vehicle nearest = null;

            double smallest = double.MaxValue;

            World.PrepareVehicleNeighbours(GetVehiclePartition(pos));

            foreach (Partition<Vehicle> p in World.CurrentNeighbours)
            {
                foreach (Vehicle vehicle in p.Members)
                {
                    Bawl bawl = vehicle as Bawl;

                    if (bawl == null) continue;

                    double distance = pos.DistanceSq(bawl.Position);

                    if (smallest > distance)
                    {
                        smallest = distance;
                        nearest = bawl;
                    }
                }
            }

            return nearest;
        }

        public static bool PrepareVisiblePartitions()
        {
            VisiblePartitions.Clear();

            double max_i = Math.Min(Origin.I + ViewSize.I + s_PartitionSizeI, GameSize.I);
            double max_j = Math.Min(Origin.J + ViewSize.J + s_PartitionSizeJ, GameSize.J);

            for (double i = Origin.I; i < max_i; i += s_PartitionSizeI)
            {
                for (double j = Origin.J; j < max_j; j += s_PartitionSizeJ)
                {
                    VisiblePartitions.Add(GetVehiclePartition(new Vector3D(i, j, 0)));
                }
            }

            return true;
        }

        public static void PrepareVisibleEntities()
        {
            foreach (List<Vehicle> list in VisibleEntities.Values)
                list.Clear();

            foreach (Partition<Vehicle> partition in VisiblePartitions)
                foreach (Vehicle vehicle in partition.Members)
                    VisibleEntities[vehicle.GetType().Name].Add(vehicle);
        }

        public static bool Visible(Vector3D pos, double rad)
        {
            return (
                (pos.I + rad) > Origin.I &&
                (pos.I - rad) < (Origin.I + GameSize.I) &&
                (pos.J + rad) > Origin.J &&
                (pos.J - rad) < (Origin.I + GameSize.J)
            );
        }

        public static Vehicle FindNearestVehicle(Vector3D pos)
        {
            Vehicle nearest = null;

            double smallest = double.MaxValue;


            foreach (Vehicle vehicle in Manager.Instance.Registered())
            {
                Bawl bawl = vehicle as Bawl;

                if (bawl == null) continue;

                double distance = pos.DistanceSq(bawl.Position);

                if (smallest > distance)
                {
                    smallest = distance;
                    nearest = bawl;
                }
            }

            return nearest;
        }

        public static void Render()
        {
            float minX = s_PartitionSizeI - (float)(Origin.I % s_PartitionSizeI);
            float minY = s_PartitionSizeJ - (float)(Origin.J % s_PartitionSizeJ);

            float maxX = (float)ViewSize.I;
            float maxY = (float)ViewSize.J;

            while (minX < maxX)
            {
                Renderer.DrawPartitionLine(minX, 0, minX, maxY);

                Renderer.DrawString( ((int)(Math.Ceiling(Origin.I) + minX)).ToString(), minX + 5, 10, false, Brushes.White);
                            
                minX += s_PartitionSizeI;
            }

            while (minY < maxY)
            {
                Renderer.DrawPartitionLine(0, minY, maxX, minY);

                Renderer.DrawString( ((int)(Math.Ceiling(Origin.J) + minY)).ToString(), 10, minY + 5, false, Brushes.White);

                minY += s_PartitionSizeJ;
            }

            
            PrepareVisiblePartitions();
            PrepareVisibleEntities();

            foreach (Vehicle vehicle in VisibleEntities["Star"])
                vehicle.Render();
            
            foreach (Vehicle vehicle in VisibleEntities["Bawl"])
                vehicle.Render();

            foreach (Vehicle vehicle in VisibleEntities["Human"])
                vehicle.Render();

            foreach (Star star in s_Stars)
            {
                star.IncrementOffset();
                World.VehicleReport(star);
            }
        }

        public static Vector3D ToLocal(Vector3D posGlobal)
        {
            return (posGlobal - Origin);
        }

        public static Vector3D ToWorld(Vector3D posLocal)
        {
            return (Origin + posLocal);
        }

    }
}
