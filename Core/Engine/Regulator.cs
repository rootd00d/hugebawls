using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace HugeBawls
{
    class Regulator
    {
        /// <summary>
        /// Private instance of the Timer class
        /// </summary>
        private Timer m_tIntervalTimer;
        

        /// <summary>
        /// Indicates if the Regulator instance is "ready".
        /// To be ready means that at least <i>delta</i> milliseconds
        /// of time has transpired since the last time the 
        /// <see cref="Ready"/> method was called.
        /// </summary>
        private bool m_Ready = false;


        /// <summary>
        /// Constructor which receives an integer number of
        /// milliseconds as the regulator period length.
        /// </summary>
        /// <param name="delta"></param>
        public Regulator(int delta)
        {
            try
            {
                m_tIntervalTimer = new Timer();

                m_tIntervalTimer.Enabled = false;

                m_tIntervalTimer.Interval = delta;

                m_tIntervalTimer.Tick += new EventHandler(m_tIntervalTimer_Tick);

            }
            catch(Exception e)
            {
                UI.LogMessage(e.ToString());
            }
        }

        public int Interval()
        {
            return m_tIntervalTimer.Interval;
        }

        public bool Enable()
        {
            try
            {
                m_tIntervalTimer.Enabled = true;
                return true;
            }
            catch (Exception e)
            {
                UI.LogMessage("Can't start regulator: {0}", e.ToString());
                return false;
            }
        }

        public bool Disable()
        {
            m_tIntervalTimer.Enabled = false;
            return true;
        }

        void  m_tIntervalTimer_Tick(object sender, EventArgs e)
        {
            m_Ready = true;
        }

        public bool Ready()
        {
            if (m_Ready)
            {
                m_Ready = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Dispose()
        {
            m_tIntervalTimer.Dispose();
        }   

    }
}
