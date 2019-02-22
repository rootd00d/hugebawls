using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace HugeBawls
{
    public static class Settings
    {
        public static Vector3D WorldSize =
            new Vector3D(
                Convert.ToDouble(ConfigurationSettings.AppSettings["WorldSizeX"]),
                Convert.ToDouble(ConfigurationSettings.AppSettings["WorldSizeY"]),
                0);

        public static Vector3D MiniSize =
            new Vector3D(
                Convert.ToDouble(ConfigurationSettings.AppSettings["MiniSizeX"]),
                Convert.ToDouble(ConfigurationSettings.AppSettings["MiniSizeY"]),
                0);

        public static int WorldPartitions =
            Convert.ToInt32(ConfigurationSettings.AppSettings["NumPartitions"]);

        public static int PeriodUpdate = 1000 /
            Convert.ToInt32(ConfigurationSettings.AppSettings["FrequencyUpdate"]);

        public static int PeriodRender = 1000 /
            Convert.ToInt32(ConfigurationSettings.AppSettings["FrequencyRender"]);

        public static double BawlMaxAcceleration =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMaxAcceleration"]);

        public static double BawlMaxVelocity =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMaxVelocity"]);

        public static double HumanMaxVelocity =
            Convert.ToDouble(ConfigurationSettings.AppSettings["HumanMaxVelocity"]);

        public static double HumanMaxAcceleration =
            Convert.ToDouble(ConfigurationSettings.AppSettings["HumanMaxAcceleration"]);

        public static double VectorMinMagnitude =
            Convert.ToDouble(ConfigurationSettings.AppSettings["VectorMinMagnitude"]);

        public static double VectorMaxVisibleLength =
            Convert.ToDouble(ConfigurationSettings.AppSettings["VectorMaxLength"]);

        public static double ScaleVelocity = 
            Convert.ToDouble(ConfigurationSettings.AppSettings["ScaleVelocity"]);

        public static double ScaleAcceleration =
            Convert.ToDouble(ConfigurationSettings.AppSettings["ScaleAcceleration"]);

        public static double ScaleGravity =
            Convert.ToDouble(ConfigurationSettings.AppSettings["ScaleGravity"]);

        public static int StarsCount =
            Convert.ToInt32(ConfigurationSettings.AppSettings["StarsCount"]);

        public static double StarsMaxVelocity =
            Convert.ToDouble(ConfigurationSettings.AppSettings["StarsMaxVelocity"]);

        public static int StarsStandardSize =
            Convert.ToInt32(ConfigurationSettings.AppSettings["StarsStandardSize"]);

        public static double StarsAverage =
            Convert.ToDouble(ConfigurationSettings.AppSettings["StarsAverage"]);

        public static double StarsShooting =
            Convert.ToDouble(ConfigurationSettings.AppSettings["StarsShooting"]);

        public static int StarsMinSize =
            Convert.ToInt32(ConfigurationSettings.AppSettings["StarsMinSize"]);

        public static int StarsMaxSize =
            Convert.ToInt32(ConfigurationSettings.AppSettings["StarsMaxSize"]);

        public static int NumEntities =
            Convert.ToInt32(ConfigurationSettings.AppSettings["NumEntities"]);

        public static int NumMessages =
            Convert.ToInt32(ConfigurationSettings.AppSettings["NumMessages"]);

        public static double VelocityVectorScale = VectorMaxVisibleLength / BawlMaxVelocity;
        public static double AccelerationVectorScale = VectorMaxVisibleLength / BawlMaxAcceleration;

        public static int HudSpaceX =
            Convert.ToInt32(ConfigurationSettings.AppSettings["HudSpaceX"]);

        public static int HudSpaceY =
            Convert.ToInt32(ConfigurationSettings.AppSettings["HudSpaceY"]);

        public static bool ShowAcceleration =
            Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowAcceleration"]);

        public static bool ShowVelocity =
            Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowVelocity"]);

        public static bool ShowHeading =
            Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowHeading"]);
    }


    public enum BawlSize
    {
        Small = 8,
        Medium = 16,
        Large = 32,
        Huge = 64
    };


    public static class BawlMass
    {
        public static double Small =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMassSmall"]);

        public static double Medium =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMassMedium"]);

        public static double Large =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMassLarge"]);

        public static double Huge =
            Convert.ToDouble(ConfigurationSettings.AppSettings["BawlMassHuge"]);
    };


    public enum MessageType
    {
        Collision
    }


}
