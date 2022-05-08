using System;
using System.Runtime.InteropServices;
using System.Threading;
using Library;
using Server.Envir;

namespace Server.Util
{
    public class AccurateTimer
    {
        private delegate void TimerEventDel(int id, int msg, IntPtr user, int dw1, int dw2);
        private const int TIME_PERIODIC = 1;
        private const int EVENT_TYPE = TIME_PERIODIC;// + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!
        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerEventDel handler, IntPtr user, int eventType);
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);

        private Action mAction;
        private int mTimerId;
        private int delay;
        private TimerEventDel mHandler;
        private Thread taskThread = null;


        public AccurateTimer(Action action, TimeSpan delay)
        {
            int calculatedDelay = Convert.ToInt32(delay.TotalMilliseconds);

            if (calculatedDelay <= 0)
                calculatedDelay = 1;

            InitTimer(action, calculatedDelay);
        }

        public AccurateTimer(Action action, int delay)
        {
            InitTimer(action, delay);
        }

        private void InitTimer(Action action, int delay)
        {
            if (delay <= 0)
                delay = 1;

            this.delay = delay;
            mAction = action;

            timeBeginPeriod(1);
            mHandler = new TimerEventDel(TimerCallback);
            taskThread = new Thread(InternalCallAction) { IsBackground = true };
            mTimerId = timeSetEvent(delay, 0, mHandler, IntPtr.Zero, EVENT_TYPE);

            SEnvir.Log(String.Format(
                "Started timer for method {0} with interval {1}ms.",

                mAction.Method.Name,
                delay
            ));
        }

        public void Stop()
        {
            int err = timeKillEvent(mTimerId);
            timeEndPeriod(1);
        }

        private void TimerCallback(int id, int msg, IntPtr user, int dw1, int dw2)
        {
            if (mTimerId != 0)
            {
                if (taskThread.ThreadState == ThreadState.Stopped)
                {
                    taskThread = new Thread(InternalCallAction) { IsBackground = true };
                    taskThread.Start();
                }
                else if (taskThread.ThreadState == ThreadState.Unstarted || taskThread.ThreadState == (ThreadState.Background | ThreadState.Unstarted))
                    taskThread.Start();
            }
        }

        private void InternalCallAction()
        {
            SEnvir.Now = Time.Now;
            mAction.Invoke();
        }
    }
}
