using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace HugeBawls.Hud
{
    public class Control
    {
        private static Control s_Instance;

        private static Dictionary<int, Group> s_Groups;

        private Control()
        {
            s_Groups = new Dictionary<int, Group>();
        }

        public bool HasGroup(string group)
        {
            return (s_Groups.ContainsKey(group.GetHashCode()));
        }

        public Group GetGroup(string group)
        {
            int hashCode = group.GetHashCode();
            Group g = null;

            if (!s_Groups.ContainsKey(hashCode))
            {
                g = new Group(
                        group,
                        Settings.HudSpaceX,
                        s_Groups.Count * Settings.HudSpaceY,
                        Pens.Gray,
                        Pens.DarkBlue,
                        Pens.Black,
                        true);

                s_Groups.Add(hashCode, g);

                UI.LogMessage("Adding group: {0}", g.Name);

                return g;
            }
            else
            {
                return s_Groups[hashCode];
            }
        }


        public EntryGeneric AddGeneric(string group, string name, Object data)
        {
            int hashCode = group.GetHashCode();

            Group g = GetGroup(group);

            EntryGeneric entry = new EntryGeneric(s_Groups[hashCode], name, data);

            s_Groups[hashCode].Add(entry);

            return entry;
        }

        public void RemoveGroup(string group)
        {
            int hashCode = group.GetHashCode();

            if (!s_Groups.ContainsKey(hashCode)) return;

            s_Groups.Remove(hashCode);
        }

        public void Render()
        {
            foreach (Group g in s_Groups.Values)
            {
                g.Render();
            }
        }


        public static Control Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Control();
                }
                return s_Instance;
            }
        }
    }
}
