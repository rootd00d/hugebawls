using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using HugeBawls;
using HugeBawls.Entities;

namespace HugeBawls
{


    public class Dispatcher
    {
        private static Dispatcher s_Instance = null;

        private static Dictionary<int, List<Message>> s_Messages =
            new Dictionary<int, List<Message>>(Settings.NumEntities);

        public bool DelayedDischarge()
        {
            List<Message> list;
            DateTime current = DateTime.Now;

            try
            {
                foreach (int id in s_Messages.Keys)
                {
                    list = s_Messages[id];

                    if (list.Count != 0)
                    {
                        Message m = list[0];

                        if (m.Time < current)
                        {
                            if (Discharge(Manager.Instance.GetEntity(id), m))
                            {
                                list.RemoveAt(0);
                            }
                            else
                            {
                                list.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                UI.LogMessage(e.ToString());
                return false;
            }

            return true;
        }

        private bool Discharge(Base entity, Message msg)
        {
            try
            {
                return entity.HandleMessage(msg);
            }
            catch (Exception e)
            {
                UI.LogMessage(e.ToString());
                return false;
            }
        }

        private bool Queue(Message msg)
        {
            List<Message> list;

            if (s_Messages.ContainsKey(msg.Destination))
            {
                list = s_Messages[msg.Destination];

                int idx = 0;
                foreach (Message m in list)
                {
                    if (m.Time > msg.Time)
                    {
                        list.Insert(idx, msg);
                        return true;
                    }
                    idx++;
                }

                list.Add(msg);
                return true;
            }
            else
            {
                list = new List<Message>(Settings.NumMessages);

                list.Add(msg);

                s_Messages[msg.Destination] = list;
            }

            return true;
        }

        public bool Dispatch(Message msg)
        {
            if (msg.Delay <= 0.0)
            {
                Discharge(Manager.Instance.GetEntity(msg.Destination), msg);
            }
            else
            {
                msg.Time = DateTime.Now + new TimeSpan(0, 0, 0, 0, (int)msg.Delay);
                Queue(msg);
            }
                

            return true;
        }

        public static Dispatcher Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new Dispatcher();
                }

                return s_Instance;
            }
        }


        

    }
}
