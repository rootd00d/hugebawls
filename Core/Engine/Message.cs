using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HugeBawls
{

    public class Message
    {
        private int m_Src;
        private int m_Dst;

        private DateTime m_Time;
        private long m_Delay;

        private object[] m_Data;

        private MessageType m_Type;
        
        public int Source
        {
            get { return m_Src; }
        }

        public int Destination
        {
            get { return m_Dst; }
        }

        public long Delay
        {
            get { return m_Delay; }
        }

        public MessageType Type
        {
            get { return m_Type; }
        }
        
        public DateTime Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        public object[] Data
        {
            get { return m_Data; }
        }


        public Message(MessageType type, int src, int dst, long delay,
            params object[] data)
        {
            m_Type = type;
            m_Src = src;
            m_Dst = dst;

            m_Delay = delay;
            m_Data = data;
        }
    }

}
