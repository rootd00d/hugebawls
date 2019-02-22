using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls
{
    public class Manager
    {
        private static Manager s_Instance = null;

        private Dictionary<int, Base> m_Entities;
        private Dictionary<Type, Dictionary<int, Base>> m_Types;

        private Manager()
        {
            m_Entities = new Dictionary<int, Base>(Settings.NumEntities);

            m_Types = new Dictionary<Type, Dictionary<int, Base>>(10);
        }

        public bool
        Register(Vehicle vehicle)
        {
            Type t = vehicle.GetType();

            try
            {
                if (!m_Types.ContainsKey(t))
                {
                    m_Types.Add(t, new Dictionary<int,Base>(Settings.NumEntities));
                }
                
                m_Types[t].Add(vehicle.ID, vehicle);

                m_Entities[vehicle.ID] = vehicle;
            }
            catch (Exception)
            {
                UI.LogMessage("Failed to register entity: {0}", vehicle.ID);
                return false;
            }

            return true;
        }

        public Dictionary<int, Base>.ValueCollection Registered()
        {
            return m_Entities.Values;
        }

        public Dictionary<int, Base>.ValueCollection Registered(Type t)
        {
            if (!m_Types.ContainsKey(t))
            {
                m_Types[t] = new Dictionary<int, Base>(Settings.NumEntities);
            }

            return m_Types[t].Values;
        }

        public bool Registered(int ID)
        {
            return m_Entities.ContainsKey(ID);
        }

        public Base GetEntity(int ID)
        {
            if (m_Entities.ContainsKey(ID))
            {
                return m_Entities[ID];
            }
            else
            {
                UI.LogMessage("Failed to look-up entity ID={0}", ID);
                return null;
            }
        }

        public static void Reset()
        {
            s_Instance = new Manager();
        }

        public static Manager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Manager();
                }

                return s_Instance;
            }
        }


    }
}
