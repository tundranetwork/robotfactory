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

        public static void SafeInvoke<T>(this EventHandler<T> evt, object sender, T args) where T : EventArgs
        {
            if (evt != null)
            {
                evt.Invoke(sender, args);
            }
        }
    }
}