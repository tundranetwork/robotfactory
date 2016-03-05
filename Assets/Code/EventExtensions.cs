using System;

namespace RobotFactory
{
    public static class EventExtensions
    {
        public static void SafeInvoke(this EventHandler evt, object sender, EventArgs args)
        {
            if (evt != null)
            {
                evt.Invoke(sender, args);
            }
        }
    }
}