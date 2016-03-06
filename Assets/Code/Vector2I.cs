using UnityEngine;
using System.Collections;

namespace RobotFactory
{
    public struct Vector2I
    {
        // Disabled to fit in with Unity naming
        // ReSharper disable InconsistentNaming
        public int x, y;
        // ReSharper enable InconsistentNaming

        public Vector2I(int nx, int ny)
        {
            x = nx;
            y = ny;
        }

        public static Vector2I FloorFrom(Vector3 pos)
        {
            return new Vector2I(
                Mathf.FloorToInt(pos.x),
                Mathf.FloorToInt(pos.z));
        }
    }
}